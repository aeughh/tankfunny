//
// Copyright (c) Brian Hernandez. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using UnityEngine;

public class TurretMouseController : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private Transform turret = null;
    [SerializeField] private Transform mouseAim = null;
    [SerializeField] private Transform cameraRig = null;
    [SerializeField] private Transform cam = null;

    [Header("Options")]
    [SerializeField] private bool useFixed = true;
    [SerializeField] private float camSmoothSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float aimDistance = 500f;

    private Vector3 frozenDirection = Vector3.forward;
    private bool isMouseAimFrozen = false;

    public Vector3 BoresightPos
    {
        get
        {
            return turret == null
                    ? transform.forward * aimDistance
                    : (turret.transform.forward * aimDistance) + turret.transform.position;
        }
    }

    /// <summary>
    /// Get the position that the mouse is indicating the turret should fly, projected
    /// out to aimDistance meters. Also meant to be used to draw a mouse cursor.
    /// </summary>
    public Vector3 MouseAimPos
    {
        get
        {
            if (mouseAim != null)
            {
                return isMouseAimFrozen
                    ? GetFrozenMouseAimPos()
                    : mouseAim.position + (mouseAim.forward * aimDistance);
            }
            else
            {
                return transform.forward * aimDistance;
            }
        }
    }

    private void Awake()
    {
        if (turret == null)
            Debug.LogError(name + "MouseFlightController - No turret transform assigned!");
        if (mouseAim == null)
            Debug.LogError(name + "MouseFlightController - No mouse aim transform assigned!");
        if (cameraRig == null)
            Debug.LogError(name + "MouseFlightController - No camera rig transform assigned!");
        if (cam == null)
            Debug.LogError(name + "MouseFlightController - No camera transform assigned!");

        // To work correctly, the entire rig must not be parented to anything.
        // When parented to something (such as an turret) it will inherit those
        // rotations causing unintended rotations as it gets dragged around.
        transform.parent = null;
    }

    private void Update() {
        if(useFixed == false) {
            UpdateCameraPos();
        }
        RotateRig();
    }

    private void FixedUpdate() {
        if(useFixed == true) {
            UpdateCameraPos();
        }
    }

    private void RotateRig()
    {
        if(mouseAim == null || cam == null || cameraRig == null) {
            return;
        }

        // Freeze the mouse aim direction when the free look key is pressed.
        if(Input.GetKeyDown(KeyCode.C)) {
            isMouseAimFrozen = true;
            frozenDirection = mouseAim.forward;
        } else if  (Input.GetKeyUp(KeyCode.C)) {
            isMouseAimFrozen = false;
            mouseAim.forward = frozenDirection;
        }

        // Mouse input.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the aim target that the plane is meant to fly towards.
        // Use the camera's axes in world space so that mouse motion is intuitive.
        mouseAim.Rotate(cam.right, mouseY, Space.World);
        mouseAim.Rotate(cam.up, mouseX, Space.World);

        // The up vector of the camera normally is aligned to the horizon. However, when
        // looking straight up/down this can feel a bit weird. At those extremes, the camera
        // stops aligning to the horizon and instead aligns to itself.
        Vector3 upVec = (Mathf.Abs(mouseAim.forward.y) > 0.9f) ? cameraRig.up : Vector3.up;

        // Smoothly rotate the camera to face the mouse aim.
        cameraRig.rotation = Damp(cameraRig.rotation, Quaternion.LookRotation(mouseAim.forward, upVec), camSmoothSpeed, Time.deltaTime);
    }

    private Vector3 GetFrozenMouseAimPos() {
        if(mouseAim != null) {
            return mouseAim.position + (frozenDirection * aimDistance);
        } else {
            return transform.forward * aimDistance;
        }
    }

    private void UpdateCameraPos() {
        if (turret != null) {
            // Move the whole rig to follow the turret.
            transform.position = turret.position;
        }
    }

    private Quaternion Damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}