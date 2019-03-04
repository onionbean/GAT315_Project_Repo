using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Target types to identify
    [SerializeField] GameObject[] TargetTypes;
    
    List<GameObject> _targets; // List of targets to identify
    GameObject _nearestTarget; // Nearest target

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        IdentifyTargets();
    }

    public List<GameObject> GetTargets()
    {
        return _targets;
    }

    void IdentifyTargets()
    {
        // Get all objects of the target types
        _targets = new List<GameObject>();

        foreach (GameObject targetType in TargetTypes)
        {
            if (targetType)
                _targets.AddRange(GameObject.FindGameObjectsWithTag(targetType.tag));
        }

        // Get the nearest target
        _nearestTarget = TargetingUtil.GetNearestTarget(gameObject, ref _targets);

        //Debug.Log(_nearestTarget.name);
    }

    // Get direction to the target
    public Vector2 TargetDirection()
    {
        if (_nearestTarget)
        {
            Vector2 dir = _nearestTarget.transform.position - transform.position;
            dir.Normalize();

            return dir;
        }

        return Vector2.zero;
    }

    // Get distance to the target
    public float TargetDistance()
    {
        if (_nearestTarget)
            return Vector2.Distance(_nearestTarget.transform.position, transform.position);

        return Mathf.Infinity;
    }

    public static float TargetDistance(GameObject origin, GameObject target)
    {
        if (target)
            return Vector2.Distance(target.transform.position, origin.transform.position);

        return Mathf.Infinity;
    }
    public GameObject Nearest()
    {
        if (_nearestTarget)
            return _nearestTarget;

        return null;    
    }
}
