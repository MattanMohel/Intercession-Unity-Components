using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ActionManager))]
public class Jump : Action {
    [SerializeField]
    public Conditional jump = new Conditional();

    [SerializeField]
    public Conditional jumpReset = new Conditional();

    [SerializeField]
    public float force;

    bool hasJump = true;

    public override bool condition() {
        if (jumpReset.isTrue(gameObject)) {
            hasJump = true;
        }

        return hasJump && jump.isTrue(gameObject);
    }

    public override void doAction() {
        rb.velocity += Vector2.up * force;
        hasJump = false;
    }

    public override void serialize() {
        jump.serialize();
        jumpReset.serialize();
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

        GUIUtil.CondMenu("Jump", action.jump);
        GUIUtil.CondMenu("Reset Jump", action.jumpReset);
        action.force = EditorGUILayout.FloatField("Force", action.force);
    }
}
