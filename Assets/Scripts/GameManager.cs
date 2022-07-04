using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsNewGame = true;
    public static long Score;
    public static int Lives = 3;
    public static int AsteroidCount;
    public static event Action<long> UpdateScore;
    public static event Action<int> UpdateLives;
    public static bool KeyboardOnly = true;

    void OnEnable()
    {
        AsteroidCount = 0;
        Debug.Log(AsteroidCount);
        PlayerBullet.GetScorePoint += GetScorePoint;
        PlayerController.PlayerDamaged += HitPlayer;
    }

    void OnDisable()
    {
        PlayerBullet.GetScorePoint -= GetScorePoint;
        PlayerController.PlayerDamaged -= HitPlayer;
    }

    void OnDestroy()
    {
        Score = 0;
        Lives = 3;
        UpdateScore = null;
        UpdateLives = null;
    }

    private void GetScorePoint(string enemyTag)
    {
        Constants.DestroyScorePoints.TryGetValue(enemyTag, out var point);
        Score += point;
        UpdateScore?.Invoke(Score);
    }

    private void HitPlayer()
    {
        Lives--;
        UpdateLives?.Invoke(Lives);
    }
}
