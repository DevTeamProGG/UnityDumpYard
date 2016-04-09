using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Trigger : MonoBehaviour {

    /*Container for a game trigger*/

    public UnityEvent triggerFunction;

    //Object affected by the trigger
    public GameObject triggerTarget;
    //Function to fire on triggering

    //On what state the trigger should trigger on transition or just ones TODO: Add more options?
    //True = ones fale = on transition
    public bool triggerType;
    //Trigger state
    public bool triggerState;

    public Sprite onSprite;
    public Sprite offSprite;

    private bool lastState;

    public bool getTriggerType()
    {
        return triggerType;
    }

    public bool getTriggerState()
    {
        return triggerState;
    }

    public bool toggleTrigger()
    {
        triggerState = !triggerState;
        return triggerState;
    }

    public void setTriggerType(bool state)
    {
        triggerType = state;
    }

    public void setTrigger(bool state)
    {
        triggerState = state;
    }

    void Start()
    {
        lastState = triggerState;
    }

    void Update()
    {
        if ((lastState != triggerState))
        {
            if (triggerType && triggerState)
                triggerFunction.Invoke();
            else if (triggerType && !triggerState)
            {
                //NATHING
            }
            else
                triggerFunction.Invoke();

            lastState = triggerState;

            if(!(triggerType && !triggerState))
                if (triggerState)
                {
                    this.GetComponent<SpriteRenderer>().sprite = onSprite;
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().sprite = offSprite;
                }

        }

    }

}
