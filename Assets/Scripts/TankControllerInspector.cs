using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TankController))]
public class SpawnZoneInspector : Editor { 
    private TankController tankController;

    public void OnEnable() {
        tankController = target as TankController;
    }

    void OnSceneGUI() {
        foreach(var wheelInstance in tankController.tankWheels) {
            if(wheelInstance.wheel != null) {
                Handles.color = Color.red;

                Handles.DrawWireDisc(wheelInstance.wheel.position + (wheelInstance.wheel.transform.right * wheelInstance.width), wheelInstance.wheel.transform.right, wheelInstance.radius);
                Handles.DrawWireDisc(wheelInstance.wheel.position, wheelInstance.wheel.transform.right, wheelInstance.radius);
                Handles.DrawWireDisc(wheelInstance.wheel.position + (wheelInstance.wheel.transform.right * -wheelInstance.width), wheelInstance.wheel.transform.right, wheelInstance.radius);
            }
        }
    }
}