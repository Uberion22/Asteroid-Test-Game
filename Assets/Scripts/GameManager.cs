using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsNewGame = true;

    public static long Score;
    public static int Lives = 3;
    public static event EventHandler UpdateScore;
    public static event EventHandler UpdateLives;
    public static bool KeyboardOnly = true;
    // Start is called before the first frame update
    
    void Start()
    {
        PlayerBullet.GetScorePoint += GetScorePoint;
        PlayerController.PlayerDamaged += HitPlayer;
    }

    void OnDestroy()
    {
        Score = 0;
        Lives = 3;
        UpdateScore = null;
        UpdateLives = null;
    }

    private void GetScorePoint(object enemyTag, EventArgs e)
    {
        var enTag = (string)enemyTag;
        Constants.DestroyScorePoints.TryGetValue(enTag, out var point);
        Score += point;
        UpdateScore?.Invoke(Score, EventArgs.Empty);
    }

    private void HitPlayer(object enemyTag, EventArgs e)
    {
        Lives--;
        UpdateLives?.Invoke(Lives, EventArgs.Empty);
    }
}
