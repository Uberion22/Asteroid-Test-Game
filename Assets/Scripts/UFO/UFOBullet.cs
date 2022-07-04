using System;
using System.Collections;
using UnityEngine;

public class UFOBullet : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;
    public static event Action<GameObject> ReturnToUFOBulletPool;

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
        if (other.CompareTag(Constants.UFOTag)) return;

        ReturnToUFOBulletPool?.Invoke(this.gameObject);
    }

    private IEnumerator WaitBeforeDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToUFOBulletPool?.Invoke(this.gameObject);
    }

    private float GetDestroyTime()
    {
        return Constants.CornerX * 2 / speed;
    }
}
