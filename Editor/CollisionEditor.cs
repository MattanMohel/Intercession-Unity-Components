
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Collision))]
public class CollisionEditor : Editor {
    Collision action;

    private void OnEnable() {
        action = (Collision)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Control object behaviour on collision", MessageType.None);

        GUIUtil.CondMenu("Respond to Other Collider", action, ref action.collisionCond);
        base.OnInspectorGUI();
    }
}