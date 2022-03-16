using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;

    // Use this for initialization
    void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
