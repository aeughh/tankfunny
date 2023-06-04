using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody rigidBody;

    // [Header("Treads")]
    // [SerializeField] private DriveWheel[] driveWheels;

    private void FixedUpdate() {
        // foreach(var driveWheel in driveWheels) {
        //     driveWheel.rigidBody.AddRelativeTorque(Vector3.right * driveWheel.torque);
        // }
    }
}

[System.Serializable]
public class DriveWheel {
    public Rigidbody rigidBody;
    public float torque;
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