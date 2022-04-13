using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ActionManager: MonoBehaviour {
    [SerializeField]
    public List<int> tags = new List<int>();
}

public class Node : MonoBehaviour {
    protected Camera cam;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected ActionManager manager;

    private void OnEnable() {
        deserialize();
    }

    public void Awake() {
        cam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        manager = GetComponent<ActionManager>();
    }

    public virtual void serialize() { }
    public virtual void deserialize() { }
}

[RequireComponent(typeof(ActionManager))]
public class Action : Node {
    private void Update() {
        if (condition()) {
            doAction();
        }
    }

    public virtual void doAction() { }
    public virtual bool condition() { return false; }
}

[RequireComponent(typeof(ActionManager))]
public class Trigger : Node {
    [HideInInspector]
    public bool state = false;
    [HideInInspector]
    public bool stateChanged = false;

    public void Update() {
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