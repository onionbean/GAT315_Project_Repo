using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Power Management for a particular hero */
[RequireComponent(typeof(BasicStatManager))]
public class AbilityManager : MonoBehaviour {
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */

    // Basic Attack 
    [SerializeField] public AbilitySlot BasicAttack; 
    // Ability 1
    [SerializeField] public AbilitySlot Ability1;
    // Ability 2
    
    [SerializeField] public AbilitySlot Ability2;
    // Resting ability 
    [SerializeField] public AbilitySlot[] RestingAbilities;

    // Reference to hero controller array
    public HeroController HeroControl;    
    // Reference to basic stats 
    public BasicStatManager BasicStats;


    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */

    private void Awake()
    {
        if (HeroControl == null)
            HeroControl = FindObjectOfType<HeroController>();
        if (BasicStats == null)
            BasicStats = GetComponent<BasicStatManager>();
    }

    // Use this for initialization
    void Start ()
    {
        if (BasicAttack != null)
            BasicAttack.Init(BasicStats);
        if (Ability1 != null)
            Ability1.Init(BasicStats);
        if (Ability2 != null)
            Ability2.Init(BasicStats);

        foreach (AbilitySlot ability in RestingAbilities)
            ability.Init(BasicStats);
	}
	
	// Update is called once per frame
	void Update ()
    {
        AbilityTick();

        if (BasicStats.isResting)
            RestingTick();
	}

    // Update ability cooldowns 
    void AbilityTick()
    {
        if (BasicAttack != null)
            BasicAttack.AbilityTick();
        if (Ability1 != null)
            Ability1.AbilityTick();
        if (Ability2 != null)
            Ability2.AbilityTick();

        foreach (AbilitySlot restingAbility in RestingAbilities)
            restingAbility.AbilityTick();
    }

    // Resting abilities 
    void RestingTick()
    {
        foreach (AbilitySlot restingAbility in RestingAbilities)
            restingAbility.RestingTick();
    }
}
