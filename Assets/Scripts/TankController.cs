using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody rigidBody;

    [Header("Treads")]
    [SerializeField] private GameObject treadPrefab;
    [SerializeField] private Transform point;

    [SerializeField] private int numberOfTreads;
    [SerializeField] private float treadSpacingDistance;
    [SerializeField] private float tensioningSpeed; // unused currently

    [SerializeField] private List<GameObject> treadObjects = new List<GameObject>();

    void Start() {
        for(int i = 0; i < numberOfTreads; i++) {
            var spawnPos = point.position + new Vector3(0, i * treadSpacingDistance, 0); // Radius is just the distance away from the point

            GameObject treadInstance = Instantiate(treadPrefab, spawnPos, Quaternion.identity);
            
            // treadInstance.transform.LookAt(point.position);
            treadInstance.transform.Rotate(90, 0, 0);

            treadObjects.Add(treadInstance);
            
            Debug.Log(i);

            if(i == 0) {
                Debug.Log("Wow");
            } else {
                treadObjects[i - 1].GetComponent<HingeJoint>().connectedBody = treadObjects[i].GetComponent<Rigidbody>();

                // if(i == numberOfTreads - 1) {
                //     treadObjects[numberOfTreads - 1].GetComponent<HingeJoint>().connectedBody = treadObjects[0].GetComponent<Rigidbody>();
                // }
            }
        }
    }
}