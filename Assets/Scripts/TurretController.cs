using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    [Header("Turret")]
    [SerializeField] private TurretMouseController controller = null;
    [SerializeField] private Rigidbody turretRigidBody;
    public float turretTurnTorque;

    [Header("Barrel")]
    [SerializeField] private Rigidbody barrelRigidBody;
    public float barrelTurnTorque;


    private void Awake() {
        if(controller == null) {
            Debug.LogError(name + ": Plane - Missing reference to TurretMouseController!");
        }
    }

    private void FixedUpdate() {
        RotateTurret();
    }

    private void RotateTurret() {
        turretRigidBody.AddTorque(new Vector3(0, yaw * turretTurnTorque, 0), ForceMode.Force);
        barrelRigidBody.AddTorque(new Vector3(pitch * barrelTurnTorque, 0, 0), ForceMode.Force);
    }
}