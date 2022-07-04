using System;
using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{ 
    [SerializeField] float speed = 4.0f;
    public static event Action<GameObject> ReturnToPlayerBulletPool;
    public static event Action<string> GetScorePoint;

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
        if (other.CompareTag(Constants.PlayerTag)) return;

        GetScorePoint?.Invoke(other.tag);
        ReturnToPlayerBulletPool?.Invoke(this.gameObject);
    }
    
    private IEnumerator WaitBeforeDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPlayerBulletPool?.Invoke(this.gameObject);
    }

    private float GetDestroyTime()
    {
        return Constants.CornerX * 2 / speed;
    }
}
