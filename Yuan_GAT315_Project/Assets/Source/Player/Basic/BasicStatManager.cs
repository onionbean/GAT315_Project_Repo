using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicStatManager : MonoBehaviour {
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */
    [Header("BASIC STATS")]
    // HP 
    [SerializeField] public float MaxHP = 150;
    // Armor 
    [SerializeField] public float Armor = 0;
    // PP
    [SerializeField] public float MaxPP = 100;
    // Resting PP Regen
    [SerializeField] public float PPRegen = 3;
    // Resting HP Regen
    [SerializeField] public float HPRegen = 0;

    // Whether this unit is a hero
    public bool isHero = false;

    // Reference to hero controller array 
    public HeroController HeroControl;


    public bool destroyOnDeath = false;
    public UnityEvent DeathEvent;

    // Whether the unit is "resting" or "active" 
    public bool isResting { get; set; }
    // Whether unit is dead
    public bool isDead { get; set; }

    [Header("UI ELEMENTS")]
    public string UnitName = "SCHMUCK";
    public Sprite UIPortrait;

    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */
    // Current PP 
    private float _pp;
    // Current HP
    private float _hp;
    // Current status effects 
    private List<Effect> _effects;
    // Set up references
    void Awake()
    {
        isResting = true; // Resting unless specified otherwise 
        isDead = false; // NOT DEAD

        // If hero control is null, find it and grab it 
        if (HeroControl == null && isHero)
            HeroControl = FindObjectOfType<HeroController>();

        // Set stats 
        _hp = MaxHP;
        _pp = MaxPP;

        _effects = new List<Effect>();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isResting)
            RestingTick();

        EffectTick();	
	}

    // HP Regen, PP Regen, 
    void RestingTick()
    {
        // HP and PP Regen
        _hp += HPRegen * Time.deltaTime;
        _pp += PPRegen * Time.deltaTime;

        _hp = Mathf.Clamp(_hp, 0, MaxHP);
        _pp = Mathf.Clamp(_pp, 0, MaxPP);
    }

    // Update existing status effects on the current unit
    void EffectTick()
    {
        int i = 0;
        while (i < _effects.Count)
        {
            Effect e = _effects[i];

            e.EffectTick(gameObject);

            // If finished, remove element and shutdown effect, otherwise update index
            if (e.finished)
            {
                e.Shutdown(gameObject);
                _effects.Remove(e);
            }
            else
                ++i;
        }
    }

    // Add an effect
    public void AddEffect(Effect e)
    {
        _effects.Add(e);
    }

    public bool HasEffect(Effect e)
    {
        return _effects.Contains(e);
    }

    public Effect FindEffect(Effect e)
    {
        int i = _effects.IndexOf(e);

        if (i >= 0 && i < _effects.Count)
            return _effects[i];

        return null;
    }
    // Take damage factoring in armor 
    public void TakeDamage(float dmg, bool ignoreArmor = false)
    {
        // Multiply armor by 0 if ignore is set to true
        float modifiedDmg = dmg * GameUtility.HyperbolicArmor(Armor * GameUtility.ToInt(!ignoreArmor), 100);

        _hp -= modifiedDmg;

        Debug.Log("BASIC STAT " + gameObject.name + " \n\tDAMAGE TAKEN: " + dmg + "\n\tMODIFIED DMG TAKEN: " + modifiedDmg + "\n REMAINING HP: " + _hp);

        // If ded then die 
        if (_hp <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }

            if (DeathEvent != null)
                DeathEvent.Invoke();
            
            isDead = true;
        }
    }

    public float GetPP()
    {
        return _pp;
    }

    public void AddPP(float val)
    {
        _pp += val;
    }

    public float GetHP()
    {
        return _hp;
    }

    public void AddHP(float val)
    {
        _hp += val;
    }
}
