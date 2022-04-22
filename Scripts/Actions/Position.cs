using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]
public class Position : Action {
    [SerializeField]
    public Conditional spawn = new Conditional();

    [SerializeField]
    public Transform pos;

    public override bool condition() {
        return spawn.isTrue(gameObject);
    }

    public override void doAction() {
        gameObject.transform.position = pos.position;
    }

    public override void serialize() {
        spawn.serialize();
    }

    public override void deserialize() {
        spawn.deserialize();
    }
}

[CustomEditor(typeof(Position))]
public class PositionEditor : Editor {
    Position action;

    private void OnEnable() {
        action = (Position)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Move object to position", MessageType.None);

        GUIUtil.CondMenu("Position", action.spawn);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("pos"), new GUIContent("Spawn Position"));

        serializedObject.ApplyModifiedProperties();
    }
}