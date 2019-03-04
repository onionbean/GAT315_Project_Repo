using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif 

/* Damage the target by a specified amount */
[CreateAssetMenu(fileName = "SO_EffectDamage_HeroName_AbilityName", menuName = "ScriptableObjects/Effects/Damage", order = 1)]
public class E_Damage : Effect {
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */
    [SerializeField] public Vector2 DamageRange;
    [SerializeField] public bool IgnoreArmor = false;
    [SerializeField] public bool UseDeltaTime = false;

    [Range(0, 100)]
    [SerializeField] public float CritChance = 0;

    [SerializeField] public float CritMultiplier = 2;

    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */

    private BasicStatManager basicStats;

    protected override void Affect(GameObject target)
    {
        base.Affect(target);

        // Apply damage to the target
        if (basicStats == null)
            basicStats = target.GetComponent<BasicStatManager>();

        if (basicStats != null)
        {
            float dmg = Random.Range(DamageRange.x, DamageRange.y);

            // Crit chance
            if (Random.Range(0, 100) < CritChance)
                dmg *= CritMultiplier;

            // Incase we use Damage Over Time
            if (UseDeltaTime)
                dmg *= Time.deltaTime;

            basicStats.TakeDamage(dmg, IgnoreArmor);
            
        }
    }
}
