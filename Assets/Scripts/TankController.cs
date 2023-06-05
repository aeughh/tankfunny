using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody rigidBody;

    [Header("Treads")]
    [SerializeField] private Tensioner[] tensioners;
    [SerializeField] private Axle[] axles;
    [SerializeField] private LayerMask treadLayerMask;
    [SerializeField] private Transform leftSprocket;
    [SerializeField] private Transform rightSprocket;
    [SerializeField] private float driveWheelRadius;
    [SerializeField] private float driveForce;

    private void Update() {
        foreach(var tensioner in tensioners) {
            // Requires Auto Configure Connected Anchor to be false
            tensioner.joint.connectedAnchor = Vector3.Lerp(tensioner.joint.connectedAnchor, tensioner.tensionerTargetPosition, tensioner.tensionSpeed);
        }
    }

    private void FixedUpdate() {
        // Left Sprocket
        Collider[] leftHitColliders = Physics.OverlapSphere(leftSprocket.position + new Vector3(0, -0.25f, 0), driveWheelRadius, treadLayerMask);
        foreach(var leftCollider in leftHitColliders) {
            Debug.DrawRay(leftCollider.transform.position, new Vector3(0, driveWheelRadius, 0), Color.green, 0.1f);

            if(Input.GetKey(KeyCode.W)) {
                leftCollider.GetComponent<Rigidbody>().AddRelativeForce(0, 0, -driveForce);
            }
            if(Input.GetKey(KeyCode.S)) {
                leftCollider.GetComponent<Rigidbody>().AddRelativeForce(0, 0, driveForce);
            }
            if(Input.GetKey(KeyCode.A)) {
                leftCollider.GetComponent<Rigidbody>().AddRelativeForce(0, 0, driveForce);
            }
            if(Input.GetKey(KeyCode.D)) {
                leftCollider.GetComponent<Rigidbody>().AddRelativeForce(0, 0, -driveForce);
            }
        }
        // Right Sprocket
        Collider[] rightHitColliders = Physics.OverlapSphere(rightSprocket.position + new Vector3(0, -0.25f, 0), driveWheelRadius, treadLayerMask);
        foreach(var rightCollider in rightHitColliders) {
            Debug.DrawRay(rightCollider.transform.position, new Vector3(0, driveWheelRadius, 0), Color.green, 0.1f);

            if(Input.GetKey(KeyCode.W)) {
                rightCollider.GetComponent<Rigidbody>().AddRelativeForce(0, 0, -driveForce);
            }
            if(Input.GetKey(KeyCode.S)) {
                rightCollider.GetComponent<Rigidbody>().AddRelativeForce(0, 0, driveForce);
            }
            if(Input.GetKey(KeyCode.A)) {
                rightCollider.GetComponent<Rigidbody>().AddRelativeForce(0, 0, -driveForce);
            }
            if(Input.GetKey(KeyCode.D)) {
                rightCollider.GetComponent<Rigidbody>().AddRelativeForce(0, 0, driveForce);
            }
        }


        //axles
        foreach(var axle in axles) {
            if(Input.GetKey(KeyCode.W)) {
                axle.leftWheel.AddRelativeTorque(Vector3.right * axle.wheelForce);
                axle.rightWheel.AddRelativeTorque(Vector3.right * axle.wheelForce);
            }
            if(Input.GetKey(KeyCode.S)) {
                axle.leftWheel.AddRelativeTorque(Vector3.right * -axle.wheelForce);
                axle.rightWheel.AddRelativeTorque(Vector3.right * -axle.wheelForce);
            }
            if(Input.GetKey(KeyCode.A)) {
                axle.leftWheel.AddRelativeTorque(Vector3.right * -axle.wheelForce);
                axle.rightWheel.AddRelativeTorque(Vector3.right * axle.wheelForce);
            }
            if(Input.GetKey(KeyCode.D)) {
                axle.leftWheel.AddRelativeTorque(Vector3.right * axle.wheelForce);
                axle.rightWheel.AddRelativeTorque(Vector3.right * -axle.wheelForce);
            }
        }
    }
}

[System.Serializable]
public class Axle {
    public Rigidbody leftWheel;
    public Rigidbody rightWheel;
    public float wheelForce;
}

[System.Serializable]
public class Tensioner {
    public ConfigurableJoint joint;
    public Vector3 tensionerTargetPosition;
    public float tensionSpeed;
}



/*
for(int i = 0; i < numberOfTreads; i++) {
    if(i == 0) {
        Debug.Log("i = 0 V1");
    } else {
        treadObjects[i - 1].GetComponent<HingeJoint>().connectedBody = treadObjects[i].GetComponent<Rigidbody>();
        spawnPos = treadObjects[i - 1].transform.position + new Vector3(0, i * treadSpacingDistance, 0);
    }

    var x = (int)numberOfTreads / 4;

    if(i <= x) {
        GameObject treadInstance = Instantiate(treadPrefab, spawnPos, Quaternion.Euler(90, 0, 0));
        treadObjects.Add(treadInstance);
    } else if(i <= 2 * x) {
        GameObject treadInstance = Instantiate(treadPrefab, spawnPos, Quaternion.Euler(0, 0, 0));
        treadObjects.Add(treadInstance);
    } else if(i <= 3 * x) {
        GameObject treadInstance = Instantiate(treadPrefab, spawnPos, Quaternion.Euler(90, 0, 0));
        treadObjects.Add(treadInstance);
    } else {
        GameObject treadInstance = Instantiate(treadPrefab, spawnPos, Quaternion.Euler(0, 0, 0));
        treadObjects.Add(treadInstance);
    }
            
    // treadInstance.transform.LookAt(point.position);

    if(i == 0) {
        Debug.Log("i = 0 V2");
    } else {
        treadObjects[i - 1].GetComponent<HingeJoint>().connectedBody = treadObjects[i].GetComponent<Rigidbody>();

        // if(i == numberOfTreads - 1) {
        //     treadObjects[numberOfTreads - 1].GetComponent<HingeJoint>().connectedBody = treadObjects[0].GetComponent<Rigidbody>();
        // }
    }
}
*/