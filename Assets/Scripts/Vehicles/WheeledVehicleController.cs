using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheeledVehicleController : MonoBehaviour {
    [SerializeField] private Transform testTarget;

    [Header("Treads")]
    [SerializeField] private Tensioner[] tensioners;
    [SerializeField] private Axle[] axles;    

    [Header("Autopilot")]
    [Tooltip("Sensitivity for autopilot flight.")] public float sensitivity = 5f;
    [Tooltip("Angle at which airplane banks fully into target.")] public float aggressiveTurnAngle = 10f;

    [Header("Turret")]
    [SerializeField] private TurretMouseController controller = null;
    [SerializeField] private Rigidbody turretRigidBody;
    public float turretTurnTorque;

    [Header("Barrel")]
    [SerializeField] private Rigidbody barrelRigidBody;
    public float barrelTurnTorque;

    private void Start() {
        foreach(var tensioner in tensioners) {
            tensioner.joint.autoConfigureConnectedAnchor = false;
        }
    }

    private void Update() {
        foreach(var tensioner in tensioners) {
            tensioner.joint.connectedAnchor = Vector3.Lerp(tensioner.joint.connectedAnchor, tensioner.tensionerTargetPosition, tensioner.tensionSpeed);
        }

        // Calculate the autopilot stick inputs.
        if(controller != null) {
            RunAutopilot(controller.MouseAimPos);
        }
    }

    private void FixedUpdate() {
        Tank();
    }

    private void Tank() {
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

    private void RunAutopilot(Vector3 flyTarget) {
        //Find the rotation to the target
        var targetDirection = turretRigidBody.transform.position - testTarget.transform.position;
        Vector3 rotationDirection = Vector3.RotateTowards(turretRigidBody.transform.forward, targetDirection, 360, 0.00f);
        Quaternion targetRotation = Quaternion.LookRotation(-rotationDirection);

        Debug.Log(targetRotation);

        // float yAngleError = Mathf.DeltaAngle(turretRigidBody.transform.rotation.eulerAngles.y, targetRotation.eulerAngles.y);
        // float yTorqueCorrection = turretPID.GetOutput(yAngleError, Time.fixedDeltaTime);
        // float turretRotation = turretPID.UpdateAngle(Time.fixedDeltaTime, turretRigidBody.transform.rotation.eulerAngles.y, targetRotation.eulerAngles.y);
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
    public Joint joint;
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