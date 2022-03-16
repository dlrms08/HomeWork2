using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpd = 0.125f;
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, new Vector3(0,desiredPos.y,desiredPos.z), smoothSpd);
        transform.position = smoothPos;

        //transform.LookAt(target);
    }
}
