using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject UFOPrefab;
    [SerializeField] float UFOMinSpawnDelay = 5;
    [SerializeField] float UFOMaxSpawnDelay = 10;
    [SerializeField] float nextWaveSpawnTime = 2;
    [SerializeField] int enemysInWave = 2;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] asteroidAudioClips;
    public static ObjectPool<GameObject> AsteroidPool;
    public static ObjectPool<GameObject> UFOPool;
    private int spawnBoundShift = 20;

    // Start is called before the first frame update
    void Start()
    {
        SetAsteroidPoolSettings();
        SetUFOPoolSettings();
        StartCoroutine(WaitAndSpawnWave(nextWaveSpawnTime, AsteroidSize.BigAsteroid));
        StartCoroutine(StartSpawningUFO());
    }

    void OnEnable()
    {
        AsteroidController.ReturnAndSpawnOther += SpawnAsteroidsAfterDestroy;
        AsteroidController.ReturnWithoutSpawn += ReturnAsteroidToPool;
        UFOController.OutOfBounds += ReturnUFOToPool;
    }

    void OnDisable()
    {
        AsteroidController.ReturnAndSpawnOther -= SpawnAsteroidsAfterDestroy;
        AsteroidController.ReturnWithoutSpawn -= ReturnAsteroidToPool;
        UFOController.OutOfBounds -= ReturnUFOToPool;
    }

    void OnDestroy()
    {
        ResetStaticOnDestroy();
    }

    private void SetAsteroidPositionAndRotation(GameObject asteroid)
    {
        var spawnSide = (SpawnSide)Random.Range(0, 4);
        var spawnPos = GetSpawnPos(spawnSide);
        var zRotation = Random.Range(0, 360);
        asteroid.transform.rotation = Quaternion.Euler(asteroidPrefab.transform.rotation.x, asteroidPrefab.transform.rotation.y, zRotation);
        asteroid.transform.position = spawnPos;
    }

    private void SetScaleAndTag(GameObject asteroid, AsteroidSize? setScale = null)
    {
        var scale = setScale ?? (AsteroidSize)Random.Range(0, 3);
        asteroid.transform.localScale = Constants.AsteroidSizes[scale];
        asteroid.tag = Constants.AsteroidTags[scale];
    }

    private void SpawnNewWave(AsteroidSize? asteroidSize)
    {
        for (int i = 0; i < enemysInWave; i++)
        {
            var asteroid = AsteroidPool.Get();
            SetAsteroidPositionAndRotation(asteroid);
            SetScaleAndTag(asteroid, asteroidSize);
        }

        enemysInWave++;
    }

    private void SpawnUFO()
    {
        if(UFOPool.CountActive > 0) return;

        var ufo = UFOPool.Get();
        var spawnSide = (SpawnSide)Random.Range(2, 4);
        var spawnPos = GetSpawnPos(spawnSide, spawnBoundShift);
        var rotation = GetRotation(UFOPrefab.transform, spawnPos, -Math.Sign(spawnPos.x) * Constants.CornerX, -Math.Sign(spawnPos.y) * Constants.CornerY);
        ufo.transform.position = spawnPos;
        ufo.transform.rotation = rotation;
    }

    private Vector3 GetSpawnPos(SpawnSide spawnSide, int boundShift = 0)
    {
        var spawnPosX = 0.0f;
        var spawnPosY = 0.0f;
        var borderShiftY = (Constants.CornerY / 100.0f) * boundShift;

        switch (spawnSide)
        {
            case SpawnSide.Up:
            {
                spawnPosX = Random.Range(-(Constants.CornerX), Constants.CornerX);
                spawnPosY = Constants.SpawnBoundY;
                break;
            }
            case SpawnSide.Down:
            {
                spawnPosX = Random.Range(-(Constants.CornerX), Constants.CornerX);
                spawnPosY = -Constants.SpawnBoundY;
                break;
            }
            case SpawnSide.Left:
            {
                spawnPosX = - Constants.SpawnBoundX;
                spawnPosY = Random.Range(-(Constants.CornerY - borderShiftY), Constants.CornerY - borderShiftY);
                break;
            }
            case SpawnSide.Right:
            {
                spawnPosX = Constants.SpawnBoundX;
                spawnPosY = Random.Range(-(Constants.CornerY - borderShiftY), Constants.CornerY - borderShiftY);
                break;
            }
        }

        return new Vector3(spawnPosX, spawnPosY);
    }

    private void SpawnAsteroidsAfterDestroy(GameObject asteroid)
    {
        AsteroidPool.Release(asteroid);
        if (asteroid.tag == Constants.SmallAsteroidTag)
        {
            StartWaveIfAllDestroyed();
            return;
        }

        var spawnPos = asteroid.transform.position;
        var currentSize = asteroid.tag == Constants.BigAsteroidTag ? AsteroidSize.MediumAsteroid : AsteroidSize.SmallAsteroid;
        for (var i = 0; i < 2; i++)
        {
            var zRotation = i == 0 ? asteroid.transform.eulerAngles.z + 45 : asteroid.transform.eulerAngles.z - 45;
            var rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, zRotation);
            var current = AsteroidPool.Get();
            current.transform.position = spawnPos;
            current.transform.rotation = rotation;
            SetScaleAndTag(current, currentSize);
        }
    }

    private void ReturnAsteroidToPool(GameObject asteroid)
    {
        AsteroidPool.Release(asteroid);
        StartWaveIfAllDestroyed();
    }

    private void ReturnUFOToPool(GameObject sender)
    {
        UFOPool.Release(sender);
        PlayASteroidExplosionSound(sender.tag);
        if (UFOPool.CountActive == 0)
        {
            StartCoroutine(StartSpawningUFO());
        }
    }

    private Quaternion GetRotation(Transform objectTransform, Vector3 currentPos, float enemyX, float enemyY)
    {
        return Quaternion.Euler(objectTransform.rotation.eulerAngles.x, objectTransform.rotation.eulerAngles.y, Mathf.Atan2(enemyY - currentPos.y, enemyX- currentPos.x) * Mathf.Rad2Deg - 90);
    }

    private void SetAsteroidPoolSettings()
    {
        AsteroidPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(asteroidPrefab),
            actionOnGet: (obj) =>
            {
                obj.transform.rotation = asteroidPrefab.transform.rotation;
                GameManager.AsteroidCount++;
                obj.SetActive(true);
            },
            actionOnRelease: (obj) =>
            {
                obj.SetActive(false);
                PlayASteroidExplosionSound(obj.tag);
                GameManager.AsteroidCount--;
            },
            actionOnDestroy: (obj) => Destroy(obj),
            defaultCapacity: 10, collectionCheck: false,
            maxSize: 15);
    }

    private void SetUFOPoolSettings()
    {
        UFOPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(UFOPrefab),
            actionOnGet: (obj) =>
            {
                obj.transform.rotation = UFOPrefab.transform.rotation;
                obj.SetActive(true);
            },
            actionOnRelease: (obj) =>
            {
                obj.SetActive(false);
            },
            actionOnDestroy: (obj) => Destroy(obj),
            defaultCapacity: 1, collectionCheck: false,
            maxSize: 2);
    }

    private IEnumerator StartSpawningUFO()
    {
        var uFOspawnTime = Random.Range(UFOMinSpawnDelay, UFOMaxSpawnDelay);
        yield return new WaitForSeconds(uFOspawnTime);
        SpawnUFO();
    }

    private IEnumerator WaitAndSpawnWave(float time, AsteroidSize? asteroidSize)
    {
        yield return new WaitForSeconds(time);
        SpawnNewWave(asteroidSize);
    }

    private void StartWaveIfAllDestroyed()
    {
        if (AsteroidPool.CountActive == 0)
        {
            StartCoroutine(WaitAndSpawnWave(nextWaveSpawnTime,null));
        }
    }

    private void ResetStaticOnDestroy()
    {
        AsteroidPool = null;
        UFOPool = null;
    }

    private void PlayASteroidExplosionSound(string asteroidTag)
    {
        audioSource.clip = GetClipByAsteroidTag(asteroidTag);
        audioSource.Play();
    }

    private AudioClip GetClipByAsteroidTag(string asteroidTag)
    {
        switch (asteroidTag)
        {
            case Constants.SmallAsteroidTag:
            {
                return asteroidAudioClips[0]; 
            }
            case Constants.MediumAsteroidTag:
            {
                return asteroidAudioClips[1];
            }
            case Constants.BigAsteroidTag:
            {
                return asteroidAudioClips[2];
            }
            default:
            {
                return asteroidAudioClips[3];
            }
        }
    }
}
