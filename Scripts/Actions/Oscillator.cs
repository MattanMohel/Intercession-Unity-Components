using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Oscillator : Action {
    [SerializeField]
    public List<Transform> transforms = new List<Transform>();

    [SerializeField]
    [HideInInspector]
    public float speed;

    [SerializeField]
    [HideInInspector]
    public float precision;

    [SerializeField]
    [HideInInspector]
    public ConditionalManager move = new ConditionalManager();

    int curTransform = 0;

    public override void doAction() {
        if (!move.evaluate(gameObject)) {
            return;
        }

        if (Vector2.Distance(transform.position, transforms[curTransform].position) < precision) {
            curTransform = (curTransform + 1) % transforms.Count;
        } else  {
            transform.position = Vector2.Lerp(transform.position, transforms[curTransform].position, speed / 1000f);
        }
    }

    public override void serialize() {
        ConditionalManager.Serialize(ref move);
    }

    public override void deserialize() {
        move.deserialize();
    }
}
