using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Base class for effects */
public class Effect : ScriptableObject
{
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */
    public string name = "Effect";
    public bool hasDuration = false;    // Whether it has duration
    public float duration;              // Duration of effect 
    public bool finished { get; set; }   // If the effect has finished. If so take off list

    public GameObject source { get; set; } // The source object
    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */
    private float currDuration = 0;

    private void Start()
    {
        finished = false;
    }

    public void ResetEffect()
    {
        finished = false;

        if (hasDuration)
            currDuration = 0;
    }
    // Apply effect to target 
    public void EffectTick(GameObject target)
    {
        if (finished)
            return;

        // Affect target
        Affect(target);

        // If doesn't have duration, or if duration exceeded time, finish effect
        if (!hasDuration || currDuration > duration)
        {
            finished = true;
            return;
        }

        // Update duration
        currDuration += Time.deltaTime;
    }

    protected virtual void Affect(GameObject target)
    {
        if (target == null)
            return;
    }

    public virtual void Shutdown(GameObject target)
    {
        if (target == null)
            return;
    }
}

