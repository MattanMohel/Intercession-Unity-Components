using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ActionType {
    Add,
    Remove
}

[RequireComponent(typeof(ActionManager))]
public class EditTags : Action
{
    [SerializeField]
    public Conditional edit = new Conditional();
    
    [SerializeField]
    public ActionType action;
    
    [SerializeField]
    public List<int> tagName = new List<int>();

    public override bool condition() {
        return edit.isTrue(gameObject);
    }

    public override void doAction() {
        
        foreach(int tag in tagName) {
            switch(action) {
                case ActionType.Add:
                    if(!manager.tags.Contains(tag)) {
                        manager.tags.Add(tag);
                    }
                    break;
                case ActionType.Remove:
                    if(manager.tags.Contains(tag)) {
                        manager.tags.Remove(tag);
                    }
                    break;
            }
            
        }
    }

    public override void serialize() {
        edit.serialize();
    }

    public override void deserialize() {
        edit.deserialize();
    }
}

[CustomEditor(typeof(EditTags))]
public class EditTagsEditor : Editor {
    EditTags action;

    private void OnEnable() {
        action = (EditTags)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Adds/Removes tag once a condition has been met", MessageType.None);

        GUIUtil.CondMenu(action.action.ToString() + " tag", action.edit);
        action.action = (ActionType) EditorGUILayout.EnumPopup("Tag Action", action.action);
        GUIUtil.tagMenu(ref action.tagName);
    }
}
