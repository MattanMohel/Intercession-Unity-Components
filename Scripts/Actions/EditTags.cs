using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TagAction {
    Add,
    Remove
}

[RequireComponent(typeof(ActionManager))]
public class EditTags : Action
{
    [SerializeField]
    public ConditionalManager edit = new ConditionalManager();
    
    [SerializeField]
    public TagAction action;
    
    [SerializeField]
    public List<int> tagName = new List<int>();

    public override bool condition() {
        return edit.evaluate(gameObject);
    }

    public override void doAction() {
        
        foreach(int tag in tagName) {
            switch(action) {
                case TagAction.Add:
                    if(!manager.tags.Contains(tag)) {
                        manager.tags.Add(tag);
                    }
                    break;
                case TagAction.Remove:
                    if(manager.tags.Contains(tag)) {
                        manager.tags.Remove(tag);
                    }
                    break;
            }
            
        }
    }

    public override void serialize() {
        ConditionalManager.Serialize(ref edit);
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

        GUIUtil.CondMenu(action.action.ToString() + " tag", action, ref action.edit);
        action.action = (TagAction) EditorGUILayout.EnumPopup("Tag Action", action.action);
        GUIUtil.tagMenu(ref action.tagName);
    }
}
