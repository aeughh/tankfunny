using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private TurretMouseController controller = null;
    [SerializeField] private Rigidbody turretRigidBody;
    [SerializeField] private Rigidbody barrelRigidBody;

    [Header("Physics")]
    public float turretTurnTorque;
    public float barrelTurnTorque;

    [Header("Autopilot")]
    [Tooltip("Sensitivity for autopilot flight.")] public float sensitivity = 5f;
    [Tooltip("Angle at which airplane banks fully into target.")] public float aggressiveTurnAngle = 10f;

    [Header("Input")]
    [SerializeField] [Range(-1f, 1f)] private float pitch = 0f;
    [SerializeField] [Range(-1f, 1f)] private float yaw = 0f;

    public float Pitch { set { pitch = Mathf.Clamp(value, -1f, 1f); } get { return pitch; } }
    public float Yaw {
        set {
            yaw = Mathf.Clamp(value, -1f, 1f);
        } get {
            return yaw;
        }
    }

    private void Awake() {
        if(controller == null) {
            Debug.LogError(name + ": Plane - Missing reference to TurretMouseController!");
        }
    }

    // private void RunAutopilot(Vector3 flyTarget, out float yaw, out float pitch, out float roll) {
    //     // This is my usual trick of converting the fly to position to local space.
    //     // You can derive a lot of information from where the target is relative to self.
    //     var localTarget = transform.InverseTransformPoint(flyTarget).normalized * sensitivity;
    //     var angleOffTarget = Vector3.Angle(transform.forward, flyTarget - transform.position);

    //     // IMPORTANT!
    //     // These inputs are created proportionally. This means it can be prone to
    //     // overshooting. The physics in this example are tweaked so that it's not a big
    //     // issue, but in something with different or more realistic physics this might
    //     // not be the case. Use of a PID controller for each axis is highly recommended.

    //     // ====================
    //     // PITCH AND YAW
    //     // ====================

    //     // Yaw/Pitch into the target so as to put it directly in front of the aircraft.
    //     // A target is directly in front the aircraft if the relative X and Y are both
    //     // zero. Note this does not handle for the case where the target is directly behind.
    //     yaw = Mathf.Clamp(localTarget.x, -1f, 1f);
    //     pitch = -Mathf.Clamp(localTarget.y, -1f, 1f);

    //     // ====================
    //     // ROLL
    //     // ====================

    //     // Roll is a little special because there are two different roll commands depending
    //     // on the situation. When the target is off axis, then the plane should roll into it.
    //     // When the target is directly in front, the plane should fly wings level.

    //     // An "aggressive roll" is input such that the aircraft rolls into the target so
    //     // that pitching up (handled above) will put the nose onto the target. This is
    //     // done by rolling such that the X component of the target's position is zeroed.

    //     // A "wings level roll" is a roll commands the aircraft to fly wings level.
    //     // This can be done by zeroing out the Y component of the aircraft's right.

    //     // Blend between auto level and banking into the target.
    //     var wingsLevelInfluence = Mathf.InverseLerp(0f, aggressiveTurnAngle, angleOffTarget);
    // }

    private void FixedUpdate() {
        // Ultra simple flight where the plane just gets pushed forward and manipulated
        // with torques to turn.
        turretRigidBody.AddRelativeTorque(new Vector3(0, turretTurnTorque * pitch, 0), ForceMode.Force);
        barrelRigidBody.AddRelativeTorque(new Vector3(barrelTurnTorque * pitch, 0, 0), ForceMode.Force);
    }
}