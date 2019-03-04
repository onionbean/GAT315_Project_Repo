using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicStatManager))]
public class BasicAttack : MonoBehaviour
{
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */
    // Basic Attack Section
    [Header("BASIC ATTACK")]
    // Basic ATTK PPCost 
    [SerializeField] public float BasicPPCost = 0;
    // Basic ATTK DMG Range
    [SerializeField] public Vector2 BasicATTKDamageRange;
    // Basic ATTK SPD
    [SerializeField] public float BasicATTKSpeed = 1;
    // Basic ATTK Ranged 
    [SerializeField] public bool BasicATTKIsRanged = false;
    // Basic ATTK Crit Chance
    [SerializeField] public float BasicATTKCritChance = 0;
    // Basic ATTK Crit Multiplier 
    [SerializeField] public float BasicATTKCritMult = 2;

    [Header("ATTACK OBJECTS")]
    // Basic ATTK Object 
    [SerializeField] public GameObject Weapon;
    [SerializeField] public float WeaponTimeSpan = .2f; // Time span the object exists for 
    [SerializeField] public GameObject Projectile;      // Projectile if ranged 
    [SerializeField] public float ProjectileSpeed = 5;  // Projectile speed

    // Reference to basic stat
    public BasicStatManager BasicStats; 
    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */
    private float currATTKCooldown = 0;     // Attack cooldown

    private void Awake()
    {
        currATTKCooldown = BasicATTKSpeed;

        if (BasicStats == null)
            BasicStats = GetComponent<BasicStatManager>();

        if (Weapon != null)
            Weapon.SetActive(false);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        currATTKCooldown += Time.deltaTime;

        if (!BasicStats.isResting)
            AttackTick();
	}

    void AttackTick()
    {
        // If active and past attack cooldown Attack
        if (BasicStats != null && !BasicStats.isResting && 
            Input.GetAxisRaw("Fire1") != 0 && currATTKCooldown >= BasicATTKSpeed && BasicStats.GetPP() >= BasicPPCost)
        {
            currATTKCooldown = 0;
            StartCoroutine(Attack());
        }
    }

    // Attack 
    IEnumerator Attack()
    {
        // Enable collider or if attack is ranged spawn the object 
        if (BasicATTKIsRanged && Projectile != null)
        {
            // Shoot projectile from weapon if exists
            Transform spawn = transform;
            if (Weapon != null)
                spawn = Weapon.transform;

            GameUtility.ShootProjectileMouse(Projectile, ProjectileSpeed, spawn);
        }
        // If not ranged disable collider after some time
        else
        {
            if (Weapon != null)
            {
                Weapon.SetActive(true);
                yield return new WaitForSeconds(WeaponTimeSpan);
                Weapon.SetActive(false);
            }
            else
                Debug.Log("BASIC ATTACK: No Weapon Exists");
        }

        yield return null;
    }
}
