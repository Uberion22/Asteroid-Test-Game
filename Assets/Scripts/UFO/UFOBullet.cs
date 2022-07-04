using System;
using UnityEngine;

public class UFOBullet : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;
    public static event EventHandler ReturnWithoutSpawn;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    void OnDestroy()
    { 
        ReturnWithoutSpawn = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.UFOTag)) return;

        ReturnWithoutSpawn?.Invoke(this.gameObject, EventArgs.Empty);
    }
}
