using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Constants
{
    public const float DestroyBoundX = 28;
    public const float DestroyBoundY = 19;
    public const float SpawnBoundX = 27;
    public const float SpawnBoundY = 18;
    public const float CornerX = 26.5f;
    public const float CornerY = 17;
    public const string SmallAsteroidTag = "Small Asteroid";
    public const string MediumAsteroidTag = "Medium Asteroid";
    public const string BigAsteroidTag = "Big Asteroid";
    public const string PlayerBulletTag = "Player Bullet";
    public const string UFOBulletTag = "UFO Bullet";
    public const string UFOTag = "UFO";
    public const string PlayerTag = "Player";
    public const int SmallAsteroidPoints = 100;
    public const int MediumAsteroidPoints = 50;
    public const int BigAsteroidPoints = 20;
    public const int UFOPoints = 200;

    public static Dictionary<AsteroidSize, string> AsteroidTags = new FlexibleDictionary<AsteroidSize, string>()
    {
        {AsteroidSize.SmallAsteroid, SmallAsteroidTag},
        {AsteroidSize.MediumAsteroid, MediumAsteroidTag},
        {AsteroidSize.BigAsteroid, BigAsteroidTag}
    };

    public static Dictionary<AsteroidSize, Vector3> AsteroidSizes = new FlexibleDictionary<AsteroidSize, Vector3>()
    {
        {AsteroidSize.SmallAsteroid, new Vector3(1,1,1) },
        {AsteroidSize.MediumAsteroid, new Vector3(1.5f,1.5f,1.5f)},
        {AsteroidSize.BigAsteroid, new Vector3(2f,2f,2f) }
    };

    public static Dictionary<string, int> DestroyScorePoints = new Dictionary<string, int>()
    {
        {SmallAsteroidTag, SmallAsteroidPoints},
        {MediumAsteroidTag, MediumAsteroidPoints},
        {BigAsteroidTag, BigAsteroidPoints},
        {UFOTag, UFOPoints}
    };

    public static bool CheckOutOfBounds(Vector3 position)
    {
        return Math.Abs(position.y) > DestroyBoundY || Math.Abs(position.x) > DestroyBoundX;
    }
}

public enum AsteroidSize
{
    SmallAsteroid,
    MediumAsteroid,
    BigAsteroid
}

public enum SpawnSide : int
{
    Up,
    Down,
    Left,
    Right
}