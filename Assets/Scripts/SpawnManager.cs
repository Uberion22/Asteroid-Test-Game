using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] asteroidPrefab;
    //private Vector3 spawnPos = new Vector3(25, 0, 0);

    private float spawnTime = 3;

    private float currentTime = 0;
    private float startDelay = 5;
    private float repeatRate = 2;
    private float xBound = 15;
    private float yBound = 7;
    private float zRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (AsteroidController.AsteroidCount == 0)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        for (var i = 0; i < 2; i++)
        {
            var spawnPos = GetSpawnPos();
            var spawnIndex = Random.Range(0, 2);
            var rotation = Quaternion.Euler(asteroidPrefab[spawnIndex].transform.rotation.x, asteroidPrefab[spawnIndex].transform.rotation.y, zRotation);
            Instantiate(asteroidPrefab[spawnIndex], spawnPos, rotation);
        }

    }

    //private void SpawnObstacle()
    //{
    //    //if (playerControllerScript.gameOver) return;
    //    var spawnPos = GetSpawnPos();
    //    var rotation = Quaternion.Euler(asteroidPrefab.transform.rotation.x, asteroidPrefab.transform.rotation.y, zRotation);
    //    Instantiate(asteroidPrefab, spawnPos, rotation);
    //}

    private Vector3 GetSpawnPos()
    {
        var spawnSide = Random.Range(0, 3);
        var spawnPosX = 0.0f;
        var spawnPosY = 0.0f;
        switch (spawnSide)
        {
            case (int)SpawnSide.Up:
            {
                spawnPosX = Random.Range(-xBound, xBound);
                spawnPosY = yBound;
                zRotation = Random.Range(90, 270);
                break;

            }
            case (int)SpawnSide.Down:
            {
                spawnPosX = Random.Range(-xBound, xBound);
                spawnPosY = -yBound;
                zRotation = Random.Range(-90, 90);
                break;

            }
            case (int)SpawnSide.Left:
            {
                spawnPosX = -xBound;
                spawnPosY = Random.Range(-yBound, yBound);
                zRotation = Random.Range(-180, 0);
                break;
            }
            case (int)SpawnSide.Right:
            {
                spawnPosX = xBound;
                spawnPosY = Random.Range(-yBound, yBound);
                zRotation = Random.Range(0, 180);
                break;
            }
        }

        return new Vector3(spawnPosX, spawnPosY);
    }

}

public enum SpawnSide : int
{
    Up,
    Down,
    Left,
    Right
}
