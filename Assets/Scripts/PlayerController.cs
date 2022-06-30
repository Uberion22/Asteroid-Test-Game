using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -500.0f;
    [SerializeField] private float maxSpeed = 3;
    [SerializeField] private GameObject bullet;
    private float speed = 0;

    private float acceleration = 0.002f;
    private float frictionForce = 0.003f;
    private float bulletSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
           speed = speed >= maxSpeed ? speed : speed + acceleration;
        }
        else
        {
            speed = speed <= 0 ? 0 : speed- frictionForce;
        }
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        var horizontalAxes = Input.GetAxis("Horizontal");

        transform.rotation *= Quaternion.Euler(0f, 0f, rotationSpeed * Time.deltaTime * horizontalAxes);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
