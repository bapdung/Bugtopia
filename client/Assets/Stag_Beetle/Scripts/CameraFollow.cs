using UnityEngine;
using System.Collections;

public class Stag_Beetle_CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float somoothSpeed = 0.125f;


    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 soothedPosition = Vector3.Lerp(transform.position, desiredPosition, somoothSpeed * Time.deltaTime);
        transform.position = soothedPosition;

        transform.LookAt(target);
    }
}

