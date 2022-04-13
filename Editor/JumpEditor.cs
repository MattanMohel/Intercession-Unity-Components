
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Jump))]
public class JumpEditor : Editor {
    Jump action;

    private void OnEnable() {
        action = (Jump)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Control the vertical movement of an object", MessageType.None);

        GUIUtil.CondMenu("Jump", action, ref action.jump);
        GUIUtil.CondMenu("Reset Jump", action, ref action.jumpReset);
        action.force = EditorGUILayout.FloatField("Force", action.force);

        base.OnInspectorGUI();
    }
}