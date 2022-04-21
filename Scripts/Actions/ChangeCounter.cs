using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public enum CounterAction {
    Increase,
    Decrease
}

public class ChangeCounter : Action
{
    [SerializeField]
    [HideInInspector]
    public Conditional increaseCond = new Conditional();
    [SerializeField]
    public Counter counter;
    [SerializeField]
    public CounterAction change;
    [SerializeField]
    [HideInInspector]
    public int alteration;

    public override bool condition() {
        return increaseCond.isTrue(gameObject);
    }

    public override void doAction() {
        switch(change){
            case CounterAction.Increase:
                counter.updateCounter(alteration);
                break;
            case CounterAction.Decrease:
                counter.updateCounter(-1 * alteration);
                break;
        }
    }

    public override void serialize() {
        increaseCond.serialize();
    }

    public override void deserialize() {
        increaseCond.deserialize();
    }
}

[CustomEditor(typeof(ChangeCounter))]
public class ChangeCounterEditor : Editor {
    ChangeCounter action;

    private void OnEnable() {
        action = (ChangeCounter)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Edits a counter.", MessageType.None);

        GUIUtil.CondMenu(action.change.ToString(), action.increaseCond);
        action.alteration = EditorGUILayout.IntField(action.change.ToString(), action.alteration);
        base.OnInspectorGUI();
    }
}