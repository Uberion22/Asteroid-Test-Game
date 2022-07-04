using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -500.0f;
    [SerializeField] private float maxSpeed = 3;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shuttingSpeed = 3;
    public static ObjectPool<GameObject> PlayerBulletPool;
    public static event EventHandler PlayerDamaged;

    private float speed = 0;

    private float acceleration = 0.02f;
    private float frictionForce = 0.003f;
    private float bulletSpeed = 0;
    private float timeAfterShot;
    private int flickerTicks = 3;
    private float flickerSpeed = 0.5f;
    private Transform child;
    private bool invulnerability;
    private Vector3 directionVector;
    private Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        PlayerBullet.ReturnToPool += ReturnToBulletPool;

        PlayerBulletPool = new ObjectPool<GameObject>(createFunc: () =>
            {
                var temp = Instantiate(bullet);

                return temp;
            },
            actionOnGet: (obj) =>  obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj), 
            defaultCapacity: 10, collectionCheck: false,
            maxSize:15);

        child = gameObject.transform.Find("PlayerDetails");
        //AsteroidController.HitPlayer += HitPlayer;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
           speed = speed >= maxSpeed ? speed : speed + acceleration;

           directionVector = Vector3.up - transform.up;
           
        }
        else
        {
            speed = speed <= 0 ? 0 : speed - frictionForce;
        }

        //CheckPosition();
        transform.Translate(directionVector * Time.deltaTime * speed);

        var horizontalAxes = Input.GetAxis("Horizontal");

        transform.rotation *= Quaternion.Euler(0f, 0f, rotationSpeed * Time.deltaTime * horizontalAxes);
        timeAfterShot += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && IsTimeAfterShotEnded())
        {
            timeAfterShot = 0;
            GameObject myGameObject = PlayerBulletPool.Get();
            myGameObject.transform.position = transform.position;
            myGameObject.transform.rotation = transform.rotation;
        }
    }

    void OnDestroy()
    {
        PlayerBulletPool = null;
        PlayerDamaged = null;
    }

    //private void HitPlayer(object sender, EventArgs e)
    //{
    //    var asteroid = (GameObject)sender;
    //    SpawnManager.AsteroidPool.Release(asteroid);
    //}

    void OnTriggerEnter(Collider other)
    {
        if(invulnerability || other.CompareTag(Constants.PlayerBulletTag)) return;

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
        return timeAfterShot >= 1.0f / shuttingSpeed;
    }
    
    private void SetActive()
    {
        child.gameObject.SetActive(!child.gameObject.activeSelf);
        Debug.Log(child.gameObject.activeSelf);
    }

    IEnumerator EnableFlicker()
    {
        var t = 0.0f;
        invulnerability = true;
        while (t <= flickerTicks)
        {
            SetActive();
            t += flickerSpeed;
            yield return new WaitForSeconds(flickerSpeed);
        }
        child.gameObject.SetActive(true);
       // Debug.Log("End");
        invulnerability = false;
    }
}
