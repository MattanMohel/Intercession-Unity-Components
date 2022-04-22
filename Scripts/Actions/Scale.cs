using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ActionManager))]
public class Scale : Action {
    [SerializeField]
    public Vector2 scale;

    [SerializeField]
    public Conditional cond = new Conditional();

    int curTransform = 0;

    public override bool condition() {
        return cond.isTrue(gameObject);
    }

    public override void doAction() {
        gameObject.transform.localScale = scale;
    }

    public override void serialize() {
        cond.serialize();
    }

    public override void deserialize() {
        cond.deserialize();
    }
}

[CustomEditor(typeof(Scale))]
public class ScaleEditor : Editor {
    Scale action;

    private void OnEnable() {
        action = (Scale)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Change object scale", MessageType.None);

        GUIUtil.CondMenu("Change Scale", action.cond);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("scale"), new GUIContent("Set Scale"));

        serializedObject.ApplyModifiedProperties();
    }
}
