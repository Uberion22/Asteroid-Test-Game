using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private float minSpeed = 3;
    [SerializeField] private float maxSpeed = 6;
    [SerializeField] float fixedSpeed;
    private float currentSpeed;
    public static int AsteroidCount;
    public static event EventHandler ReturnAndSpawnOther;
    public static event EventHandler ReturnWithoutSpawn;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = fixedSpeed <= 0 ? Random.Range(minSpeed, maxSpeed) : fixedSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * currentSpeed);
        //CheckOutOfBounds();
    }

    void OnTriggerEnter(Collider other)
    {
        if (Constants.AsteroidTags.ContainsValue(other.tag))
        {
            return;
        }
        if (other.CompareTag(Constants.Player) || other.CompareTag(Constants.UFO))
        {
            ReturnWithoutSpawn?.Invoke(this.gameObject, EventArgs.Empty);
            return;
        }
        ReturnAndSpawnOther?.Invoke(this.gameObject, EventArgs.Empty);
    }

    //private void CheckOutOfBounds()
    //{
    //    if (Constants.CheckOutOfBounds(transform.position))
    //    {
    //        ReturnWithoutSpawn?.Invoke(this.gameObject, EventArgs.Empty);
    //    }
    //}
}
