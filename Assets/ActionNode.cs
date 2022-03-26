using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {
    NONE,
    JUMP,
    MOVE,
}

public enum Key {
    NONE = 0,

    // Cardinal Directions 

    UP = 273,
    DOWN,
    RIGHT,
    LEFT,

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

    ZERO = 48,
    ONE, 
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX, 
    SEVEN,
    EIGHT,
    NINE,

    // Mouse/Trackpad 

    LEFT_CLICK  = 323,
    RIGHT_CLICK = 324,
}
public enum Direction { 
    RIGHT, LEFT
}


[System.Serializable]
public abstract class Action {
    public abstract void DoAction();
}

[System.Serializable]
public class Jump : Action {
    public float jump_height;
    public float jump_duration;

    public override void DoAction() {

    }
}

[System.Serializable]
public class Move : Action {
    public float speed;
    public float acceleration;
    public Direction dir;

    public override void DoAction() {
    }
}

public class ActionNode : MonoBehaviour {
    [SerializeField] public Action action;

    [SerializeField] public ActionType type;
    [SerializeField] public KeyCode key;
}
