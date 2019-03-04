using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Class for abilities to inherit 
[System.Serializable]
public class AbilitySlot
{
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */
    [Header("ABILITY STATS")]
    public bool AutomaticUsage = false;
    
    public float PPCost = 30;
    // Cooldown 
    public float Cooldown = 5;
    // Is Resting ability
    public bool IsRestingAbility;
    // Event to trigger on ability use
    public UnityEvent AbilityEvent;

    [Header("ABILITY MOVEMENT")]
    // Movement curve for the unity to take 
    public AnimationCurve MovementCurve;
    [HideInInspector] public Vector3 MoveDestination;
    public float MoveDistance = 1;
    public float MoveTime;

    [SerializeField] private AbilityMovement AbilityMovementRef;  // Ref to ability movement

    [Header("ABILITY INPUT")]
    // Ability input
    public string InputAxis = "Fire1";
    // Aiming mode SO 
    [SerializeField] private AbilityAimMode AimMode;

    // Ability Prefab
    [Header("ABILITY PREFAB")]
    public GameObject AbilityPrefab;
    // public float AbilityObjectEnableTime;
    public bool isChildObject = false;
    public bool isProjectile = false;
    public float ProjectileVelocity;
    public Transform SpawnTransform;

    [Header("ABILITY UI")]
    public string AbilityName = "";
    public Sprite UISprite;

    // Target reference
    public GameObject target { get; set; }

    // Reference to stat manager
    public BasicStatManager BasicStats;

    [HideInInspector] public bool CanUse = true;

    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */
    private float _currCooldown;
    private float _currAbilityEnableTime;

    private GameObject _restingObject;    // Current resting ability object

    private bool _aiming;           // Whether we're currenty aiming
    private Vector2 _aimPosition;   // Aim target position for ability

    // Use this for initialization
    public void Init(BasicStatManager parentStat)
    {
        _currCooldown = 0;
        _currAbilityEnableTime = 0;
        _aiming = false;
        _restingObject = null;
        CanUse = true;

        BasicStats = parentStat;
    }

    public void AbilityTick()
    {
        if (!IsRestingAbility && CanUse)
            CheckUseAbility();

        _currCooldown -= Time.deltaTime;
        _currAbilityEnableTime -= Time.deltaTime;

        // If we are active then destroy the resting object if it exists 
        if (!BasicStats.isResting && _restingObject != null)
            GameObject.Destroy(_restingObject);

    }

    public void RestingTick()
    {
        // If resting object doesn't exist, create one and parent it to the current object
        if (_restingObject == null)
        {
            // Create resting object 
            _restingObject = GameObject.Instantiate(AbilityPrefab, SpawnTransform.position, SpawnTransform.rotation);

            // Parent it 
            if (isChildObject)
                _restingObject.transform.parent = BasicStats.transform;
        }
    }

    private void CheckUseAbility()
    {
        // Toggle aiming mode if we press button and if we are current active hero
        if ((AutomaticUsage || (BasicStats.isHero && InputUtility.GetAxisDown(InputAxis, true) && BasicStats.HeroControl.currentHero == BasicStats)) && CanUseAbility())
        {
            _aiming = !_aiming;
        }

        if (_aiming)
        {
            if (AimMode != null && AimMode.AimModeTick(ref _aimPosition, InputAxis, target))
            {
                _aiming = false; // Turn off aiming mode
                UseActiveAbility(_aimPosition);
            }
        }
    }

    // Apply effects to target object
    public void UseActiveAbility(Vector2 aimPosition)
    {
        if (!CanUseAbility())
            return;
        // Init ability movement if it exists
        if (AbilityMovementRef != null)
        {
            // Calculate endposition to move in
            Vector3 dir = new Vector3(_aimPosition.x, _aimPosition.y) - BasicStats.transform.position;
            dir.Normalize();

            MoveDestination = BasicStats.transform.position + dir * MoveDistance;
            AbilityMovementRef.InitCurve(this);
            AbilityMovementRef.Move();
        }

        // Trigger event
        if (AbilityEvent != null)
            AbilityEvent.Invoke();

        // Use cooldown and pp 
        BasicStats.AddPP(-PPCost);
        _currCooldown = Cooldown;

        // Create ability object if creating instead of enabling
        GameObject abilityObject = null;
        abilityObject = GameObject.Instantiate(AbilityPrefab, SpawnTransform.position, SpawnTransform.rotation);

        // If projectile launch it in 
        if (isProjectile)
        {
            Rigidbody2D rb = abilityObject.GetComponent<Rigidbody2D>();

            if (rb == null)
                abilityObject.AddComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = (aimPosition - new Vector2(SpawnTransform.position.x, SpawnTransform.position.y)).normalized * ProjectileVelocity;
        }
        else if (isChildObject)
        {
            abilityObject.transform.parent = SpawnTransform;
        }
    }

    // Whether we can use the ability given pp coost and cooldown
    public bool CanUseAbility()
    {
        return BasicStats.GetPP() >= PPCost && _currCooldown <= 0;
    }

    public float GetCurrentCooldown()
    {
        return _currCooldown;
    }

    public AbilityAimMode GetAimMode()
    {
        return AimMode;
    }
}