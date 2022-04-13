
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Oscillator))]
public class OscillatorEditor : Editor {
    Oscillator action;

    private void OnEnable() {
        action = (Oscillator)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Move object between set points", MessageType.None);

        base.OnInspectorGUI();

        GUIUtil.CondMenu("Move", action, ref action.move);

        action.speed = EditorGUILayout.FloatField("Speed", action.speed);

        action.precision = EditorGUILayout.FloatField(new GUIContent("Precison", 
            "The distance from the target needed to satisfy arrival at destination"), action.precision);
    }
}