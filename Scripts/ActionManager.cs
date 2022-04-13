using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ActionManager: MonoBehaviour {
    [SerializeField]
    public Action[] actions;

    [SerializeField]
    public List<int> tags = new List<int>();

    private void Awake() {
        actions = gameObject.GetComponents<Action>();
    }

    private void Update() {
        foreach (var action in actions) {
            action.doAction();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        var comp = collision.gameObject.GetComponent<ActionManager>();

        if (comp != null) {
            foreach (var action in actions) {
                action.onCollisionEnter(comp);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        var comp = collision.gameObject.GetComponent<ActionManager>();

        if (comp != null) {
            foreach (var action in actions) {
                action.onCollisionExit(comp);
            }
        }
    }

    public static ActionManager[] FindAll() {
        return FindObjectsOfType<ActionManager>();
    }
}

public class Node : MonoBehaviour {
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected ActionManager manager;

    private void OnEnable() {
        deserialize();
    }

    public void Awake() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        manager = GetComponent<ActionManager>();
    }

    public virtual void serialize() { }
    public virtual void deserialize() { }
}

[RequireComponent(typeof(ActionManager))]
public class Action : Node {
    public virtual void doAction() { }
   
    public virtual void onCollisionEnter(ActionManager manager) { }
    public virtual void onCollisionExit(ActionManager manager) { }
}

[RequireComponent(typeof(ActionManager))]
public class Trigger : Node {
    [HideInInspector]
    public bool state = false;
    [HideInInspector]
    public bool stateChanged = false;

    private void Update() {
        if (state) {
            if (!stateChanged) {
                stateChanged = true;
            }
            else {
                stateChanged = false;
                state = false;
            }
        }
    }
}


public enum KeyAlias {
    None = 0,

    Space = 32,
    Enter = 13,

    // Cardinal Directions 

    Up = 273,
    Down,
    Right,
    Left,

    // Alphanumerics

    A = 97,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    P,
    Q,
    R,
    S,
    T,
    U,
    V,
    W,
    X,
    Y,
    Z,

    Zero = 48,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,

    // Mouse & Trackpad 

    LeftClick = 323,
    RightClick = 324,
}