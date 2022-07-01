using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBullet : MonoBehaviour
{ 
    [SerializeField] float speed = 4.0f;
    public static event EventHandler ReturnToPool;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Player)) return;

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
