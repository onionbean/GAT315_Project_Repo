using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages how an object is destroyed when colliding with something
public class DestroyOnCollide : MonoBehaviour {
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */

    // Whether object is breakable
    [SerializeField] bool IsBreakable = true;

    // Whether the object has an object in particular that destroys it
    [SerializeField] bool HasKiller = false;
    [SerializeField] GameObject[] KillerObjects;

    // Whether there is a delay before the object dies
    [SerializeField] bool HasDeathDelay = false;
    [SerializeField] float DeathDelay = 0;

    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */

    // For trigger collisions
    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (HasKiller)
        {
            // Check tags for if the collider is one of the killers
            foreach (GameObject killer in KillerObjects)
            {
                if (killer.layer == collision.gameObject.layer)
                {
                    Kill();
                    return;
                }
            }
        }
        else
        {
            Kill();
        }
    }

    void Kill()
    {
        if (!IsBreakable)
            return;

        if (HasDeathDelay)
        {
            Destroy(gameObject, DeathDelay);
            return;
        }

        Destroy(gameObject);
    }
}
