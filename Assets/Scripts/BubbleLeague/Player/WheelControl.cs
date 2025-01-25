﻿using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public Transform wheelModel;

    [HideInInspector] public WheelCollider WheelCollider;
    public bool steerable;
    public bool motorized;
    public bool isLeftWheel;

    private Vector3 position;
    private Quaternion rotation;

    private void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        WheelCollider.GetWorldPose(out position, out rotation);
        //wheelModel.transform.position = position;
        //wheelModel.transform.rotation = rotation;
    }
}