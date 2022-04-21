using UnityEngine;

[RequireComponent(typeof(ActionManager))]
public class TimerTrigger : Trigger {
    [SerializeField]
    public float waitTime = 0f;

    float counter = 0f;

    public override void Update() {
        counter += Time.deltaTime;

        if (counter >= waitTime) {
            counter = 0;
            state = true;
        }

        base.Update();
    }
}