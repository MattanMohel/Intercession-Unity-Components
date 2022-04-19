using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public enum Crement{
    Increase,
    Decrease
}

public class Counter : Action
{
    [SerializeField]
    [HideInInspector]
    public ConditionalManager increaseCond = new ConditionalManager();
    [SerializeField]
    public Text displayBox;
    [SerializeField]
    public int startText;
    [SerializeField]
    public Crement change;
    [SerializeField]
    [HideInInspector]
    public int alteration;

    public override bool condition() {
        return increaseCond.evaluate(gameObject);
    }

    public override void doAction() {
        switch(change){
            case Crement.Increase:
                startText += alteration;
                break;
            case Crement.Decrease:
                startText -= alteration;
                break;
        }
        displayBox.text = startText.ToString();
    }

    public override void serialize() {
        ConditionalManager.Serialize(ref increaseCond);
    }

    public override void deserialize() {
        increaseCond.deserialize();
    }
}

[CustomEditor(typeof(Counter))]
public class CounterEditor : Editor {
    Counter action;

    private void OnEnable() {
        action = (Counter)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("A counter using a text box that changes by an amount when a condition is true.", MessageType.None);

        GUIUtil.CondMenu(action.change.ToString(), action, ref action.increaseCond);
        action.alteration = EditorGUILayout.IntField(action.change.ToString(), action.alteration);
        base.OnInspectorGUI();
    }
}
