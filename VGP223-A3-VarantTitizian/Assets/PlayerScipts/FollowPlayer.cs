using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform PlayerTransform;
    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public float speedH = 2.0f;
    public float speedF = 2.0f;

    private float yaw = 0.0f;
    private float hew = 0.0f;

    private bool RotateAroundPlayer = true;

    public float rotationSpeed = 5.0f;

    public float minHeight = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
        minHeight = transform.position.y;
        minHeight = 10f;
    }

    private void Update()
    {
        //Vector3 relativePos = PlayerTransform.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(relativePos);

        yaw += speedH * Input.GetAxis("Mouse X");
        hew += speedF * Input.GetAxis("Mouse Y");

       

        transform.eulerAngles = new Vector3(hew, yaw, 0.0f);
    }

    //LateUpdate is called after Update methods
    void LateUpdate()
    {
        if (RotateAroundPlayer)
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);

            _cameraOffset = camTurnAngle * _cameraOffset;
        }
        Vector3 newPos = PlayerTransform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if(RotateAroundPlayer)
        {
            transform.LookAt(PlayerTransform);
        }
    }
}
