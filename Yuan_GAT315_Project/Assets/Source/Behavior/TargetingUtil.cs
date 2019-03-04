using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static class for targeting AI
public class TargetingUtil : MonoBehaviour
{
    static public List<GameObject> GetObjectsWithTagInRadius(Vector2 origin, float radius, string tag)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius); // Get colliders within a radius

        List<GameObject> objects = new List<GameObject>();

        // Iterate through colliders getting ones that are the correct tag
        foreach (Collider2D hit in hits)
        {
            if (hit.transform.gameObject.tag == tag)
                objects.Add(hit.transform.gameObject);
        }

        return objects;
    }

    // Get nearest target 
    static public GameObject GetNearestTarget(GameObject origin, ref List<GameObject> targets)
    {
        // if list is size 1 return the only object in that list 
        if (targets.Count == 1)
            return targets[0];

        GameObject nearest = null; // The nearest target

        // min distance
        float minDist = Mathf.Infinity; 

        // Find minimum distance between target and current object
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
                continue;
            }

            float curDist = Vector2.Distance(targets[i].transform.position, origin.transform.position);

            if (curDist < minDist)
            {
                // Set mindist for next comparison
                minDist = curDist;

                // Bookmark current object as shortest dist object
                nearest = targets[i];
            }
        }

        return nearest;
    }
}