using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ActionManager))]
public class Jump : Action {
    [SerializeField]
    public ConditionalManager jump = new ConditionalManager();
    [SerializeField]
    public ConditionalManager jumpReset = new ConditionalManager();
    [SerializeField]
    public float force;

    bool hasJump = true;

    public override bool condition() {
        if (jumpReset.evaluate(gameObject)) {
            hasJump = true;
        }

        return hasJump && jump.evaluate(gameObject);
    }

    public override void doAction() {
        rb.velocity += Vector2.up * force;
        hasJump = false;
    }

    public override void serialize() {
        ConditionalManager.Serialize(ref jump);
        ConditionalManager.Serialize(ref jumpReset);
    }

    public override void deserialize() {
        jump.deserialize();
        jumpReset.deserialize();
    }
}

[CustomEditor(typeof(Jump))]
public class JumpEditor : Editor {
    Jump action;

    private void OnEnable() {
        action = (Jump)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Control the vertical movement of an object", MessageType.None);

        GUIUtil.CondMenu("Jump", action, ref action.jump);
        GUIUtil.CondMenu("Reset Jump", action, ref action.jumpReset);
        action.force = EditorGUILayout.FloatField("Force", action.force);
    }
}
