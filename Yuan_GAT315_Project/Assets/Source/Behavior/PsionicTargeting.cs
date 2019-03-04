using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Targeting for the psionic enemy
public class PsionicTargeting : TargetAbilities {
    [SerializeField] GameObject PlayerPrefab;

    BasicStatManager _target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetTargets();
	}

    protected override void GetTargets()
    {
        base.GetTargets();

        _abilityManager.BasicAttack.target = null;
        _abilityManager.Ability1.target = null;

        // If returned object is valid set target for a1
        if (!_target)
            _target = GetLowestTargetHealth();

        // when target found ONLY STICK WITH TARGET UNTIL HP == MAX HP
        if (_target != null)
        {
            _abilityManager.Ability1.target = _target.gameObject;

            // LOCK 
            if (_target.GetHP() >= _target.MaxHP)
                _target = null;
        }
    }

    BasicStatManager GetLowestTargetHealth()
    {
        float minHP = float.MaxValue;
        BasicStatManager lowestHPObject = null;

        foreach (GameObject target in _targeter.GetTargets())
        {
            // If object tag is player and their distance is less than attack range, 
            // return them [BASIC ATTACK IS ATTACK, A1 IS HEALING] 

            // If ally within range then find lowest health one
            var bStat = target.GetComponent<BasicStatManager>();

            //if (bStat != null && CheckRange(_basicAttackRange.radius, target) && target.tag == PlayerPrefab.tag)
            //    return bStat;

            if (bStat != null && target.tag != PlayerPrefab.tag && CheckRange(_a1Range.radius, target) && bStat.GetHP() < minHP)
            {
                minHP = bStat.GetHP() / bStat.MaxHP; // Set min hp
                lowestHPObject = bStat;
            }
        }

        return lowestHPObject;
    }
}
