using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[Serializable]
public class Conditional {
    [NonSerialized]
    public List<IConditional> conds = new List<IConditional>();

    [SerializeField]
    public List<Op> ops = new List<Op>();

    [SerializeField]
    public List<BoolState> states = new List<BoolState>();

    [SerializeField]
    public List<string> serConds = new List<string>();

    [SerializeField]
    public List<string> serCondTypes = new List<string>();

    public bool editing = false;

    public bool isTrue(GameObject gameObject) {
        if (conds.Count == 0) { 
            return false;
        }

        bool state = conds[0].evaluate(gameObject);

        for (int i = 1; i < conds.Count; ++i) {

            bool eval = conds[i].evaluate(gameObject);

            if (states[i] == BoolState.Not) {
                eval = !eval;
            }

            switch (ops[i - 1]) {
                case Op.And:
                    state = state && eval;
                    break;
                case Op.Or:
                    state  = state || eval;
                    break;
            }
        }

        return state;
    }

    public void serialize() {
        serConds.Clear();
        serCondTypes.Clear();

        foreach (var cond in conds) {
            cond.serialize(this);
        }
    }

    public void deserialize() {
        editing = false;
        conds.Clear();

        for (int i = 0; i < serConds.Count; i++) {
            var state = (State)Enum.Parse(typeof(State), serCondTypes[i], true);

            switch (state) {
                case State.False:
                    conds.Add(new False());
                    break;
                case State.True:
                    conds.Add(new True());
                    break;
                case State.KeyPress:
                    conds.Add(JsonUtility.FromJson<KeyPress>(serConds[i]));
                    break;
                case State.HasTag:
                    conds.Add(JsonUtility.FromJson<HasTag>(serConds[i]));
                    break;
                case State.XScale:
                    conds.Add(JsonUtility.FromJson<XScale>(serConds[i]));
                    break;
                case State.YScale:
                    conds.Add(JsonUtility.FromJson<YScale>(serConds[i]));
                    break;
                case State.XVelocity:
                    conds.Add(JsonUtility.FromJson<XVelocity>(serConds[i]));
                    break;
                case State.YVelocity:
                    conds.Add(JsonUtility.FromJson<YVelocity>(serConds[i]));
                    break;
                case State.XPosition:
                    conds.Add(JsonUtility.FromJson<XPosition>(serConds[i]));
                    break;
                case State.YPosition:
                    conds.Add(JsonUtility.FromJson<YPosition>(serConds[i]));
                    break;
                case State.OnTrigger:
                    conds.Add(JsonUtility.FromJson<OnTrigger>(serConds[i]));
                    break;
                case State.CounterAmount:
                    conds.Add(JsonUtility.FromJson<CounterAmount>(serConds[i]));
                    break;
            }
        }
    }

    public static IConditional CreateFromState(State state) {
        switch (state) {
            case State.True:
                return new True();
            case State.False:
                return new False();
            case State.HasTag:
                return new HasTag();
            case State.XScale:
                return new XScale();
            case State.YScale:
                return new YScale();
            case State.KeyPress:
                return new KeyPress();
            case State.XVelocity:
                return new XVelocity();
            case State.YVelocity:
                return new YVelocity();
            case State.XPosition:
                return new XPosition();
            case State.YPosition:
                return new YPosition();
            case State.OnTrigger:
                return new OnTrigger();
            case State.CounterAmount:
                return new CounterAmount();
        }

        return null;
    }

    public static bool ApplyOp(float a, float b, Cmp cmp) {
        switch (cmp) {
            case Cmp.LessThan:
                return a < b;
            case Cmp.LessThanEq:
                return a <= b;
            case Cmp.GreaterThan:
                return a > b;
            case Cmp.GreaterThanEq:
                return a >= b;
            case Cmp.Equal:
                return a == b;
            case Cmp.NotEqual:
                return a != b;
        }

        return false;
    }
}

public interface IConditional {
    public bool evaluate(GameObject obj);
    public void serialize(Conditional manager);
    public void display();
    public State state();
}

public enum Op {
    Or,
    And,
}

public enum BoolState {
    Is,
    Not,
}

public enum Cmp {
    Equal,
    NotEqual,
    LessThan,
    LessThanEq,
    GreaterThan,
    GreaterThanEq,
}

public enum State {
    True,
    False,
    HasTag,
    KeyPress,
    XScale,
    YScale,
    XVelocity,
    YVelocity,
    XPosition,
    YPosition,
    CounterAmount,
    OnTrigger,
}

public enum PressState {
    Hold,
    Release,
    Press,
}

[Serializable]
public class OnTrigger : IConditional {
    [SerializeField]
    public Trigger trigger;

    public void display() {
        GUILayout.Label("Invoke on");
        trigger = (Trigger)EditorGUILayout.ObjectField(trigger, typeof(Trigger), true);
    }
    
    public bool evaluate(GameObject obj) {
        return trigger.state;
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.OnTrigger;
    }
}

[Serializable]
public class True : IConditional {
    public void display() {
        GUILayout.Label("True");
    }

    public bool evaluate(GameObject obj) {
        return true;
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.True;
    }
}

[Serializable]
public class False : IConditional {
    public void display() {
        GUILayout.Label("False");
    }

    public bool evaluate(GameObject obj) {
        return false;
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.False;
    }
}

[Serializable]
public class KeyPress : IConditional {
    [SerializeField]
    public KeyCode code;

    [SerializeField]
    public PressState press;

    public void display() {
        press = (PressState)EditorGUILayout.EnumPopup(press);
        code = (KeyCode)EditorGUILayout.EnumPopup((KeyAlias)code);
    }

    public bool evaluate(GameObject obj) {
        switch (press) {
            case PressState.Hold:
                return Input.GetKey(code);
            case PressState.Release:
                return Input.GetKeyUp(code);
            case PressState.Press:
                return Input.GetKeyDown(code);
        }

        return false;
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }
    public State state() {
        return State.KeyPress;
    }

}

[Serializable]
public class HasTag : IConditional {
    [SerializeField]
    public int tag;

    public void display() {
        GUILayout.Label("Has Tag: ");
        tag = EditorGUILayout.Popup(tag, IOUtil.Tags());
    }

    public bool evaluate(GameObject obj) {
        return obj.GetComponent<ActionManager>().tags.Contains(tag);
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.HasTag;
    }
}

[Serializable]
public class XScale: IConditional {
    [SerializeField]
    public float x;

    [SerializeField]
    public Cmp cmp;

    public void display() {
        GUILayout.Label("X-Scale");
        GUIUtil.SerCmp(ref cmp);
        GUIUtil.SerFloat(ref x);
    }

    public bool evaluate(GameObject obj) {
        return Conditional.ApplyOp(obj.transform.localScale.x, x, cmp);
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.XScale;
    }
}

[Serializable]
public class YScale : IConditional {
    [SerializeField]
    public float y;

    [SerializeField]
    public Cmp cmp;

    public void display() {
        GUILayout.Label("Y-Scale");
        GUIUtil.SerCmp(ref cmp);
        GUIUtil.SerFloat(ref y);
    }

    public bool evaluate(GameObject obj) {
        return Conditional.ApplyOp(obj.transform.localScale.y, y, cmp);
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.YScale;
    }
}

[Serializable]
public class XVelocity : IConditional {
    [SerializeField]
    public float xVel;

    float prevXPos = 0;

    [SerializeField]
    public Cmp cmp;

    public void display() {
        GUILayout.Label("X-Velocity");
        GUIUtil.SerCmp(ref cmp);
        GUIUtil.SerFloat(ref xVel);
    }

    public bool evaluate(GameObject obj) {
        float velocity = (obj.transform.position.x - prevXPos) / Time.deltaTime;
        if (Mathf.Abs(velocity) <= .01f) {
            velocity = 0;
        } 

        prevXPos = obj.transform.position.x;
        return Conditional.ApplyOp(velocity, xVel, cmp);
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.XVelocity;
    }
}

[Serializable]
public class YVelocity : IConditional {
    [SerializeField]
    public float yVel;

    float prevYPos = 0;

    [SerializeField]
    public Cmp cmp;

    public void display() {
        GUILayout.Label("Y-Velocity");
        GUIUtil.SerCmp(ref cmp);
        GUIUtil.SerFloat(ref yVel);
    }

    public bool evaluate(GameObject obj) {
        float velocity = (obj.transform.position.y - prevYPos) / Time.deltaTime;
        if (Mathf.Abs(velocity) <= .01f) {
            velocity = 0;
        } 

        prevYPos = obj.transform.position.y;
        return Conditional.ApplyOp(velocity, yVel, cmp);    
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.YVelocity;
    }
}

[Serializable]
public class XPosition : IConditional {
    [SerializeField]
    public float xPos;

    [SerializeField]
    public Cmp cmp;

    public void display() {
        GUILayout.Label("X-Position");
        GUIUtil.SerCmp(ref cmp);
        GUIUtil.SerFloat(ref xPos);
    }

    public bool evaluate(GameObject obj) {
        return Conditional.ApplyOp(obj.transform.position.x, xPos, cmp);
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.XPosition;
    }
}

[Serializable]
public class YPosition : IConditional {
    [SerializeField]
    public float yPos;

    [SerializeField]
    public Cmp cmp;

    public void display() {
        GUILayout.Label("Y-Position");
        GUIUtil.SerCmp(ref cmp);
        GUIUtil.SerFloat(ref yPos);
    }

    public bool evaluate(GameObject obj) {
        return Conditional.ApplyOp(obj.transform.position.y, yPos, cmp);
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.YPosition;
    }
}

[Serializable]
public class CounterAmount : IConditional {
    [SerializeField]
    public Counter counter;
    [SerializeField]
    public float req;

    [SerializeField]
    public Cmp cmp;

    public void display() {
        GUILayout.Label("Counter");
        GUIUtil.SerCmp(ref cmp);
        GUIUtil.SerFloat(ref req);
        counter = (Counter)EditorGUILayout.ObjectField(counter, typeof(Counter), true);
    }

    public bool evaluate(GameObject obj) {
        return Conditional.ApplyOp(counter.counter, req, cmp);
    }

    public void serialize(Conditional manager) {
        manager.serConds.Add(JsonUtility.ToJson(this));
        manager.serCondTypes.Add(state().ToString());
    }

    public State state() {
        return State.CounterAmount;
    }
}