using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ActionManager))]
public class Jump : Action {
    [SerializeField]
    [HideInInspector]
    public ConditionalManager jump = new ConditionalManager();
    [SerializeField]
    [HideInInspector]
    public ConditionalManager jumpReset = new ConditionalManager();
    [SerializeField]
    [HideInInspector]
    public float force;

    bool hasJump = true;

    new private void Awake() {
        base.Awake();
    }

    public override void doAction() {
        Vector2 vec = Vector2.up * force;

        if (hasJump && jump.evaluate(gameObject)) {
            rb.velocity += vec;
            hasJump = false;
        }

        if (jumpReset.evaluate(gameObject)) {
            hasJump = true;
        }
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
