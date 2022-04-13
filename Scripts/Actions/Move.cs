using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]
public class Move : Action {
    [SerializeField]
    public ConditionalManager left = new ConditionalManager();
    [SerializeField]
    public ConditionalManager right = new ConditionalManager();

    [SerializeField] 
    public float speed;

    [SerializeField]
    public float acceleration;

    public override void doAction() {
        Vector2 vec = Vector2.right * acceleration;

        if (left.evaluate(gameObject)) {
            rb.velocity -= vec;

            if (rb.velocity.x <= -speed) {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
        }  
        if (right.evaluate(gameObject)) {
            rb.velocity += vec;

            if (rb.velocity.x >= speed) {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
        }
    }

    public override void serialize() {
        ConditionalManager.Serialize(ref left);
        ConditionalManager.Serialize(ref right);
    }

    public override void deserialize() {
        left.deserialize();
        right.deserialize();
    }
}
