using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControllerOld : MonoBehaviour {
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float forceStrength;

    public TankWheel[] tankWheels;
    public LayerMask tankWheelLayerMask;

    void Start() {
        rigidBody.centerOfMass = centerOfMass.position;
    }

    void FixedUpdate() {
        foreach(var wheelInstance in tankWheels) {
            // RaycastHit leftHit;
            // Physics.Raycast(wheelInstance.wheel.position + (wheelInstance.wheel.transform.right * -wheelInstance.width), transform.TransformDirection(-Vector3.up), out leftHit, Mathf.Infinity, tankWheelLayerMask);//wheelInstance.radius
            // Debug.DrawRay(wheelInstance.wheel.position + (wheelInstance.wheel.transform.right * -wheelInstance.width), transform.TransformDirection(-Vector3.up) * leftHit.distance, Color.yellow);

            RaycastHit middleHit;
            Physics.Raycast(wheelInstance.wheel.position, transform.TransformDirection(-Vector3.up), out middleHit, 5, tankWheelLayerMask);//wheelInstance.radius
            Debug.DrawRay(wheelInstance.wheel.transform.position, transform.TransformDirection(-Vector3.up) * 5, Color.yellow);
            float invertedDifference = Mathf.InverseLerp(5, 0, middleHit.distance);
            Debug.Log(invertedDifference);
            rigidBody.AddForceAtPosition(forceStrength * invertedDifference * transform.TransformDirection(-Vector3.up), middleHit.point, ForceMode.Acceleration);


            // RaycastHit rightHit;
            // Physics.Raycast(wheelInstance.wheel.position + (wheelInstance.wheel.transform.right * wheelInstance.width), transform.TransformDirection(-Vector3.up), out rightHit, Mathf.Infinity, tankWheelLayerMask);//wheelInstance.radius
            // Debug.DrawRay(wheelInstance.wheel.position + (wheelInstance.wheel.transform.right * wheelInstance.width), transform.TransformDirection(-Vector3.up) * rightHit.distance, Color.yellow);
        }
    }
}

[System.Serializable]
public class TankWheel {
    public Transform wheel;
    public float radius;
    public float width;
}