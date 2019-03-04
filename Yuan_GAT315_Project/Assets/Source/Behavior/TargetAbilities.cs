using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Target))]
[RequireComponent(typeof(AbilityManager))]
public class TargetAbilities : MonoBehaviour {
    // Reference to targeting script
    [SerializeField] protected Target _targeter;

    // Range of ability as a circle collider
    [SerializeField] protected CircleCollider2D _basicAttackRange;
    [SerializeField] protected CircleCollider2D _a1Range;
    [SerializeField] protected CircleCollider2D _a2Range;

    [SerializeField] protected AbilityManager _abilityManager;

    private void Awake()
    {
        if (_targeter == null)
            _targeter = GetComponent<Target>();
        if (_abilityManager == null)
            _abilityManager = GetComponent<AbilityManager>();

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetTargets();
    }

    protected virtual void GetTargets()
    {
        // If target is in range then use abilities
        GameObject nearest = _targeter.Nearest();
        _abilityManager.BasicAttack.target = null;
        _abilityManager.Ability1.target = null;
        _abilityManager.Ability2.target = null;

        if (_basicAttackRange && CheckRange(_basicAttackRange.radius, nearest))
            _abilityManager.BasicAttack.target = nearest;
        if (_a1Range && CheckRange(_a1Range.radius, nearest))
            _abilityManager.Ability1.target = nearest;
        if (_a2Range && CheckRange(_a2Range.radius, nearest))
            _abilityManager.Ability2.target = nearest;
    }

    protected bool CheckRange(float radius, GameObject target)
    {
        return (Target.TargetDistance(gameObject, target) > radius * Mathf.Min(transform.lossyScale.x, transform.lossyScale.y));
    }
}

