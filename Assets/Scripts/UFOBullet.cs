using System;
using UnityEngine;

public class UFOBullet : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;
    public static event EventHandler ReturnWithoutSpawn;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        //CheckOutOfBounds();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.UFO)) return;

        ReturnWithoutSpawn?.Invoke(this.gameObject, EventArgs.Empty);
    }

    //private void CheckOutOfBounds()
    //{
    //    if (Constants.CheckOutOfBounds(transform.position))
    //    {
    //        ReturnWithoutSpawn?.Invoke(this.gameObject, EventArgs.Empty);
    //    }
    //}
}
