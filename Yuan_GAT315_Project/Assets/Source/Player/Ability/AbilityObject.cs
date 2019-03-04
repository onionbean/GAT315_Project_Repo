using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ability Object containing effects on trigger
public class AbilityObject : MonoBehaviour {

    [SerializeField] public Effect[] Effects;
    [SerializeField] public bool DetectOverlaps = true;


    private BoxCollider2D _boxCollider;           // Ref to box collider
    private CircleCollider2D _circleCollider;   // Ref to circle collider

    private List<BasicStatManager> _overlapObjects;

    private void Awake()
    {
        _overlapObjects = new List<BasicStatManager>();
        // Get all effects on object if they don't exist in array 
        if (Effects.Length == 0)
            Effects = GetComponents<Effect>();

        _boxCollider = GetComponent<BoxCollider2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (DetectOverlaps)
            CheckOverlaps();
	}

    // If trigger enabled over objects, accounts for objects inside of trigger that never call OnTriggerEnter 
    void CheckOverlaps()
    {
        // If box not null use box, else if circle not null use circle overlap
        Collider2D[] overlaps = null;

        if (_boxCollider != null)
            overlaps = Physics2D.OverlapBoxAll(transform.position, _boxCollider.size, transform.eulerAngles.z);
        else if (_circleCollider != null)
            overlaps = Physics2D.OverlapCircleAll(transform.position, _circleCollider.radius);

        // Check if the overlapped object doesn't exist in the current list, if not then affect it
        if (overlaps != null)
        {
            foreach (Collider2D obj in overlaps)
            {
                BasicStatManager bs = obj.GetComponent<BasicStatManager>();

                if (bs != null && !_overlapObjects.Contains(bs))
                {
                    AffectObject(bs);
                }
            }
        }
    }

    // Add Object to zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Administer effect to all who trigger
        BasicStatManager bStats = collision.gameObject.GetComponent<BasicStatManager>();

        if (bStats != null)
            AffectObject(bStats);
    }

    // Remove object from zone
    private void OnTriggerExit2D(Collider2D collision)
    {
        BasicStatManager bStats = collision.gameObject.GetComponent<BasicStatManager>();

        if (bStats != null)
        {
            _overlapObjects.Remove(bStats);
        }
    }

    // Add effects onto the object
    void AffectObject(BasicStatManager otherObject)
    {
        if (otherObject != null)
        {
            foreach (Effect e in Effects)
            {
                // Add effect if it doesn't already exist, otherwise reset the effect timer
                Effect otherE = otherObject.FindEffect(e);

                if (otherE == null)
                    otherObject.AddEffect(e);
                else
                    otherE.ResetEffect();

            }

            _overlapObjects.Add(otherObject);
        }
    }
}
