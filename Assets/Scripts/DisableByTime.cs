using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableByTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DoDisable", 2.0f);   
    }

    void DoDisable()
    {
        gameObject.SetActive(false);
    }
}
