using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
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
    [SerializeField] private PID turretPID;
    [Range(-1f, 1f)] private float yaw;

    [Header("Barrel")]
    [SerializeField] private Rigidbody barrelRigidBody;
    public float barrelTurnTorque;
    [SerializeField] private PID barrelPID;
    [Range(-1f, 1f)] private float pitch;

    public float Pitch {
        set {
            pitch = Mathf.Clamp(value, -1f, 1f);
        } get {
            return pitch;
        }
    }

    public float Yaw {
        set {
            yaw = Mathf.Clamp(value, -1f, 1f);
        } get {
            return yaw;
        }
    }

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
        float autoTurretTurn = 0f;
        float autoBarrelTurn = 0f;
        if(controller != null) {
            RunAutopilot(controller.MouseAimPos, out autoTurretTurn, out autoBarrelTurn);
        }

        // Use either keyboard or autopilot input.
        yaw = autoTurretTurn;
        pitch = autoBarrelTurn;
    }

    private void FixedUpdate() {
        Tank();
        Turret();
    }

    private void Turret() {
        turretRigidBody.AddTorque(new Vector3(0, yaw * turretTurnTorque, 0), ForceMode.Force);
        barrelRigidBody.AddTorque(new Vector3(pitch * barrelTurnTorque, 0, 0), ForceMode.Force);
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

    private void RunAutopilot(Vector3 flyTarget, out float yaw, out float pitch) {
        //Find the rotation to the target
        var targetDirection = turretRigidBody.transform.position - testTarget.transform.position;
        Vector3 rotationDirection = Vector3.RotateTowards(turretRigidBody.transform.forward, targetDirection, 360, 0.00f);
        Quaternion targetRotation = Quaternion.LookRotation(-rotationDirection);

        // float yAngleError = Mathf.DeltaAngle(turretRigidBody.transform.rotation.eulerAngles.y, targetRotation.eulerAngles.y);
        // float yTorqueCorrection = turretPID.GetOutput(yAngleError, Time.fixedDeltaTime);
        float turretRotation = turretPID.UpdateAngle(Time.fixedDeltaTime, turretRigidBody.transform.rotation.eulerAngles.y, targetRotation.eulerAngles.y);
        yaw = turretRotation;

        float barrelRotation = barrelPID.UpdateAngle(Time.fixedDeltaTime, barrelRigidBody.transform.rotation.eulerAngles.x, targetRotation.eulerAngles.x);
        pitch = barrelRotation;

        /*
        var angleOffTarget = Vector3.Angle(turretRigidBody.transform.forward, flyTarget - turretRigidBody.transform.position);
        float turretTorqueCorrection = turretPID.GetOutput(angleOffTarget, Time.fixedDeltaTime);
        Debug.Log(turretTorqueCorrection);
        */
        // yaw = Mathf.Clamp(xTorqueCorrection, -1f, 1f); //turret

        // pitch = -Mathf.Clamp(localTarget.y, -1f, 1f);


        // This is my usual trick of converting the fly to position to local space.
        // You can derive a lot of information from where the target is relative to self.
        // var localTarget = turretRigidBody.transform.InverseTransformPoint(flyTarget).normalized * sensitivity;

        // IMPORTANT!
        // These inputs are created proportionally. This means it can be prone to
        // overshooting. The physics in this example are tweaked so that it's not a big
        // issue, but in something with different or more realistic physics this might
        // not be the case. Use of a PID controller for each axis is highly recommended.

        // ====================
        // PITCH AND YAW
        // ====================

        // Yaw/Pitch into the target so as to put it directly in front of the aircraft.
        // A target is directly in front the aircraft if the relative X and Y are both
        // zero. Note this does not handle for the case where the target is directly behind.


        // float turretAngleError = Mathf.DeltaAngle(turretRigidBody.transform.rotation.eulerAngles.x, flyTarget.rotation.x);
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