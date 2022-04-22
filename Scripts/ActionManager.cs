using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ActionManager: MonoBehaviour {
    [SerializeField]
    public List<int> tags = new List<int>();
}

public class Node : MonoBehaviour {
    protected Camera cam;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer rend;
    protected ActionManager manager;

    [HideInInspector]
    public float xVelocity;
    
    [HideInInspector]
    public float yVelocity;

    private void OnEnable() {
        deserialize();
    }

    public void Awake() {
        cam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
        manager = GetComponent<ActionManager>();
    }

    public void FixedUpdate() {
        xVelocity = rb.velocity.x;
        yVelocity = rb.velocity.y;
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

    public virtual void Update() {
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

[CustomEditor(typeof(ActionManager))]
public class ActionEditor : Editor {
    ActionManager action;

    // states
    bool creatingTag = false;
    string curTag = string.Empty;

    private void OnEnable() {
        action = (ActionManager)target;
    }

    public override void OnInspectorGUI() {

        GUIUtil.tagMenu(ref action.tags);

        // Creating new tags...

        if (creatingTag) {
            if (GUILayout.Button("Cancel")) {
                creatingTag = false;
                curTag = string.Empty;
            }
        }
        else {
            if (GUILayout.Button("Create Tag")) {
                creatingTag = true;
            }
        }

        if (creatingTag) {
            GUILayout.BeginHorizontal();

            curTag = GUILayout.TextField(curTag);

            if (GUILayout.Button("Confirm")) {
                if (curTag == string.Empty) {
                    Debug.LogWarning("Tried to add an empty tag!");
                }
                else if (!IOUtil.AddTag(curTag)) {
                    Debug.LogWarning(string.Format("Tag '{0}' already exists!", curTag));
                }

                curTag = string.Empty;
            }

            GUILayout.EndHorizontal();
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