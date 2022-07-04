using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private float minSpeed = 3;
    [SerializeField] private float maxSpeed = 6;
    [SerializeField] float fixedSpeed;
    public static int AsteroidCount;
    public static event EventHandler ReturnAndSpawnOther;
    public static event EventHandler ReturnWithoutSpawn;
    private float currentSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = fixedSpeed <= 0 ? Random.Range(minSpeed, maxSpeed) : fixedSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * currentSpeed);
    }

    void OnDestroy()
    {
        ResetStaticOnDestroy();
    }

    void OnTriggerEnter(Collider other)
    {
        if (Constants.AsteroidTags.ContainsValue(other.tag))
        {
            return;
        }
        if (other.CompareTag(Constants.PlayerTag) || other.CompareTag(Constants.UFOTag))
        {
            ReturnWithoutSpawn?.Invoke(this.gameObject, EventArgs.Empty);
            return;
        }
        ReturnAndSpawnOther?.Invoke(this.gameObject, EventArgs.Empty);
    }

    private void ResetStaticOnDestroy()
    {
        AsteroidCount = 0;
        ReturnAndSpawnOther = null;
        ReturnWithoutSpawn = null;
    }
}
