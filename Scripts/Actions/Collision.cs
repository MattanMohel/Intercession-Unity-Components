using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionType {
    Enter, 
    Exit, 
    Stay,
}

[RequireComponent(typeof(ActionManager))]
public class Collision : Trigger
{
    [SerializeField]
    public Collider2D usedCollider;

    [SerializeField]
    private CollisionType collisionType;

    [SerializeField]
    [HideInInspector]
    public ConditionalManager collisionCond = new ConditionalManager();  

    public override void serialize() {
        ConditionalManager.Serialize(ref collisionCond);
    }

    public override void deserialize() {
        collisionCond.deserialize();
    }

// create callback -- Reset Jump if... Trigger
// Collision would become a Trigger -- A trigger is configured in the GUI
// as a monobehaviour but can be accepted as input to a ConditionalManager
// A reset jump would occur when 'this collision is true' therefore

// increment counter -- when 'this collision is true', etc... 
// make it so it triggers only if rest of possible conds are true as well

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collisionType == CollisionType.Enter &&
            collision.otherCollider == usedCollider &&
            collisionCond.evaluate(collision.gameObject)) {

            state = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collisionType == CollisionType.Exit && 
            collision.otherCollider == usedCollider &&
            collisionCond.evaluate(collision.gameObject)) {

            state = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collisionType == CollisionType.Stay &&
            collision.otherCollider == usedCollider &&
            collisionCond.evaluate(collision.gameObject)) {

            state = true;
        }
    }
}
