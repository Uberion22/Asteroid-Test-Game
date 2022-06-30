using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    [SerializeField] private float topBound = 8.0f;
    [SerializeField] private float lowerBound = -8.0f;
    [SerializeField] private float leftBound = -16.0f;
    [SerializeField] private float rightBound = 16.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > topBound || transform.position.y < lowerBound || transform.position.x < leftBound || transform.position.x > rightBound)
        {
            
            if (gameObject.CompareTag("Asteroid") || gameObject.CompareTag("Small Asteroid") || gameObject.CompareTag("Big Asteroid"))
            {
                AsteroidController.AsteroidCount--;
                //PlayerController.playerLives--;
                // Debug.Log($"Player Lives: {PlayerController.playerLives}");
            }
            Destroy(gameObject);

            //if (PlayerController.playerLives <= 0)
            //{
            //    //Debug.Log("Игра окончена!");
            //}
        }
    }
}
