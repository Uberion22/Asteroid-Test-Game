using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class UFOController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private GameObject redBullet;
    public static ObjectPool<GameObject> UFOBulletPool;
    public static event Action<GameObject> OutOfBounds;
    private Transform playerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        SetBulletPool();
        playerTransform = GameObject.Find("Player").transform;
        UFOBullet.ReturnToUFOBulletPool += ReturnToBulletPool;
    }

    void OnEnable()
    {
        StartCoroutine(NextShot());
    }

    void OnDestroy()
    {
        StopAllCoroutines();
        UFOBullet.ReturnToUFOBulletPool -= ReturnToBulletPool;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    private void ShotToPlayer()
    {
        var rotation = Quaternion.Euler(redBullet.transform.rotation.eulerAngles.x, redBullet.transform.rotation.eulerAngles.y, Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg - 90);
        var bulletClone = UFOBulletPool.Get();
        bulletClone.transform.position = transform.position;
        bulletClone.transform.rotation = rotation;
    }

    private IEnumerator NextShot()
    {
        if (!gameObject.activeSelf) yield break;

        var delay = Random.Range(2, 5);
        yield return new WaitForSeconds(delay);
        ShotToPlayer();
        StartCoroutine(NextShot());
    }

    private void SetBulletPool()
    {
        UFOBulletPool = new ObjectPool<GameObject>(createFunc: () =>
                Instantiate(redBullet),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            defaultCapacity: 5, collectionCheck: false,
            maxSize: 10);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.UFOBulletTag)) return;

        OutOfBounds?.Invoke(this.gameObject);
    }
    private void ReturnToBulletPool(GameObject ufoBullet)
    {
        UFOBulletPool.Release(ufoBullet);
    }
}
