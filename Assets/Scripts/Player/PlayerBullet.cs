using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlayerBullet : MonoBehaviour
{ 
    [SerializeField] float speed = 4.0f;
    public static event EventHandler ReturnToPool;
    public static event EventHandler GetScorePoint;

    void OnEnable()
    {
        var time = GetDestroyTime();
        StartCoroutine(WaitBeforeDestroy(time));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    void OnDestroy()
    {
        ReturnToPool = null;
        GetScorePoint = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PlayerTag)) return;

        Debug.Log("Trigger---------------");
        GetScorePoint?.Invoke(other.tag, EventArgs.Empty);
        ReturnToPool?.Invoke(this.gameObject, EventArgs.Empty);
    }
    
    private IEnumerator WaitBeforeDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log($"Destroyed {gameObject.tag}");
        ReturnToPool?.Invoke(this.gameObject, EventArgs.Empty);
    }

    private float GetDestroyTime()
    {
        return Constants.CornerX * 2 / speed;
    }
}
