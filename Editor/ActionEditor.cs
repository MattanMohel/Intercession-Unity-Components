
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionManager))]
public class ActionEditor : Editor {
    ActionManager action;

    // states
    bool creatingTag = false;
    string curTag = string.Empty;

    private void OnEnable() {
        action = (ActionManager)target;
    }

    public override void OnInspectorGUI() {

        GUIUtil.tagMenu(ref action.tags);

        // Creating new tags...

        if (creatingTag) {
            if (GUILayout.Button("Cancel")) {
                creatingTag = false;
                curTag = string.Empty;
            }
        }
        else {
            if (GUILayout.Button("Create Tag")) {
                creatingTag = true;
            }
        }

        if (creatingTag) {
            GUILayout.BeginHorizontal();

            curTag = GUILayout.TextField(curTag);

            if (GUILayout.Button("Confirm")) {
                if (curTag == string.Empty) {
                    Debug.LogWarning("Tried to add an empty tag!");
                }
                else if (!IOUtil.AddTag(curTag)) {
                    Debug.LogWarning(string.Format("Tag '{0}' already exists!", curTag));
                }

                curTag = string.Empty;
            }

            GUILayout.EndHorizontal();
        }

    }
}