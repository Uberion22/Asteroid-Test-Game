using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100;
    [SerializeField] private float maxSpeed = 3;
    [SerializeField] private float acceleration = 0.02f;
    [SerializeField] private float frictionForce = 0.001f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shutPerSecond = 3;

    public static ObjectPool<GameObject> PlayerBulletPool;
    public static event EventHandler PlayerDamaged;

    private float currentSpeed;
    private float timeAfterShot;
    private int flickerTicks = 3;
    private float flickerSpeed = 0.5f;
    private Transform childTransform;
    private bool invulnerabilityEnabled;
    private Vector3 directionVector;
    private float speedModifer = 100;

    // Start is called before the first frame update
    void Start()
    {
        PlayerBullet.ReturnToPool += ReturnToBulletPool;
        SetBulletPoolSettings();
        childTransform = gameObject.transform.Find("PlayerDetails");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RotatePlayer();
        MovePlayer();
        TryShot();
    }

    void OnDestroy()
    {
        PlayerBulletPool = null;
        PlayerDamaged = null;
    }

    private void TryShot()
    {
        timeAfterShot += Time.deltaTime;
        if (!IsTimeAfterShotEnded() || !Input.GetKeyDown(KeyCode.Space)) return;
       
        timeAfterShot = 0;
        GameObject myGameObject = PlayerBulletPool.Get();
        myGameObject.transform.position = transform.position;
        myGameObject.transform.rotation = transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if(invulnerabilityEnabled || other.CompareTag(Constants.PlayerBulletTag)) return;

        PlayerDamaged?.Invoke(null, EventArgs.Empty);
        StartCoroutine(EnableFlicker());
    }

    private void ReturnToBulletPool(object sender, EventArgs e)
    {
        var playerBullet = (GameObject)sender;
        PlayerBulletPool.Release(playerBullet);
    }

    private bool IsTimeAfterShotEnded()
    {
        return timeAfterShot >= 1.0f / shutPerSecond;
    }
    
    private void SetActive()
    {
        childTransform.gameObject.SetActive(!childTransform.gameObject.activeSelf);
    }

    private IEnumerator EnableFlicker()
    {
        var currentTicks = 0.0f;
        invulnerabilityEnabled = true;
        while (currentTicks <= flickerTicks)
        {
            SetActive();
            currentTicks += flickerSpeed;
            yield return new WaitForSeconds(flickerSpeed);
        }
        childTransform.gameObject.SetActive(true);
        invulnerabilityEnabled = false;
    }

    private void SetBulletPoolSettings()
    {
        PlayerBulletPool = new ObjectPool<GameObject>(createFunc: () =>
            {
                var temp = Instantiate(bullet);

                return temp;
            },
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            defaultCapacity: 10, collectionCheck: false,
            maxSize: 15);
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed = currentSpeed >= maxSpeed ? currentSpeed : currentSpeed + acceleration;
            directionVector = transform.up;
        }
        currentSpeed = currentSpeed <= 0 ? 0 : currentSpeed - frictionForce;
        transform.position += directionVector * Time.deltaTime * currentSpeed;
    }

    private void RotatePlayer()
    {
        if (GameManager.KeyboardOnly)
        {
            var horizontalAxes = Input.GetAxis("Horizontal");
            transform.rotation *= Quaternion.Euler(0f, 0f, -rotationSpeed * Time.deltaTime * horizontalAxes);
            return;
        }
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        var targetQuaternion = Quaternion.AngleAxis(angle, transform.forward);
        if (Math.Abs(targetQuaternion.z - transform.rotation.z) > 0.0001)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, Time.deltaTime * (rotationSpeed / speedModifer));
        }
    }
}
