
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Move))]
public class MoveEditor : Editor {
    Move action;

    private void OnEnable() {
        action = (Move)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Control the horizontal movement of an object", MessageType.None);

        GUIUtil.CondMenu("Move Left", action, ref action.left);
        GUIUtil.CondMenu("Move Right", action, ref action.right);
        
        action.speed = EditorGUILayout.FloatField("Speed", action.speed);
        action.acceleration = EditorGUILayout.FloatField("Acceleration", action.acceleration);
    }
}