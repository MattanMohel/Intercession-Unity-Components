using UnityEngine;
using UnityEditor;

public class Seperator : MonoBehaviour {
    public string label = "";
}

[CustomEditor(typeof(Seperator))]
public class SeperatorEditor : Editor {
    Seperator sep;

    private void Awake() {
        sep = (Seperator)target;
    }

    bool editing = false;

    public override void OnInspectorGUI() {
        GUILayout.Space(10);

        GUILayout.Label(GUIUtil.WithStyle(sep.label), GUIUtil.style);

        if (editing) {
            sep.label = EditorGUILayout.TextField(sep.label);

            if (GUILayout.Button("Finish")) {
                editing = false;
            }
        }
        else {
            if (GUILayout.Button("Edit")) {
                editing = true;
            }
        }

        GUILayout.Space(10);
    }
}
