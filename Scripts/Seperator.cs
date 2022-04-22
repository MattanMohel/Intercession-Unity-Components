using UnityEngine;
using UnityEditor;

public class Seperator : MonoBehaviour {
    public string label;
}

[CustomEditor(typeof(Seperator))]
public class SeperatorEditor : Editor {
    Seperator sep;
    bool editing = false;

    public void OnEnable() {
        sep = (Seperator)target;
    }

    public override void OnInspectorGUI() {
        sep.label = EditorGUILayout.TextField(sep.label);
    }
}
