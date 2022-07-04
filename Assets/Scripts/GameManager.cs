using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsNewGame = true;

    public static long Score;
    public static int Lives = 3;
    public static event EventHandler UpdateScore;
    public static event EventHandler UpdateLives;
    // Start is called before the first frame update
    void Start()
    {
        PlayerBullet.GetScorePoint += GetScorePoint;
        PlayerController.PlayerDamaged += HitPlayer;
    }

    void OnDestroy()
    {
        //IsNewGame = true;
        Score = 0;
        Lives = 3;
        UpdateScore = null;
        UpdateLives = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetScorePoint(object enemyTag, EventArgs e)
    {
        

        Debug.Log("Score!!!!");
        var enTag = (string)enemyTag;
        Constants.DestroyScorePoints.TryGetValue(enTag, out var point);
        Score += point;
        UpdateScore?.Invoke(Score, EventArgs.Empty);
    }

    private void HitPlayer(object enemyTag, EventArgs e)
    {
        if (Lives <= 0) return;
        Lives--;
        Debug.Log("Hit");
        UpdateLives?.Invoke(Lives, EventArgs.Empty);
    }
}
