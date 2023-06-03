using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TankControllerOld))]
public class TankControllerInspectorOld : Editor { 
    private TankControllerOld tankController;

    public void OnEnable() {
        tankController = target as TankControllerOld;
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