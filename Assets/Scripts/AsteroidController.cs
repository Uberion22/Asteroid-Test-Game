using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    public static int AsteroidCount;

    // Start is called before the first frame update
    void Start()
    {
        AsteroidCount++;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            AsteroidCount--;
            Destroy(other.gameObject);
        }
        else
        {
            Destroy(other.gameObject);
        }

        if (!gameObject.CompareTag("Small Asteroid"))
        {
            SpawnAsteroids(gameObject.tag);
        }

        Destroy(gameObject);
    }

    private void SpawnAsteroids(string current)
    {
        for (var i = 0; i < 2; i++)
        {
            var spawnPos = transform.position;
            var zRotation = i == 0 ? transform.eulerAngles.z + 45 : transform.eulerAngles.z - 45;
            var rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, zRotation);
            var clone = Instantiate(gameObject, spawnPos, rotation);
            clone.tag = current == "Big Asteroid" ? "Asteroid" : "Small Asteroid";
            var scale = current == "Big Asteroid" ? 1.5f : 1.0f;
            clone.transform.localScale = new Vector3(scale, scale, scale); ;
        }
    }
}
