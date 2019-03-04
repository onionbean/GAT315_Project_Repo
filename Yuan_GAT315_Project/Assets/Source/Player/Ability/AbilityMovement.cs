using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animation curves for ability movement
[RequireComponent(typeof(AbilityManager))]
public class AbilityMovement : MonoBehaviour {
    public AnimationCurve aCurve;

    public float MoveTime = 1;

    public float steps = 32;

    private AnimationCurve _stepCurve; // Integral curve

    float _endDisplacement = 0;

    public Vector3 StartPos { get; set; }
    public Vector3 EndPos { get; set; }

    // Things to disable
    [SerializeField] private AbilityManager _abilityManager;
    [SerializeField] private HeroController _heroControl;
    [SerializeField] private FollowTarget _targetFollow;

    private AbilitySlot _currentAbility;
    private Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        StartPos = transform.position;
        EndPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitCurve(AbilitySlot ability)
    {
        _currentAbility = ability;
        aCurve = ability.MovementCurve;

        StartPos = transform.position;
        EndPos = ability.MoveDestination;
        MoveTime = ability.MoveTime;
        
        // Setup curve
        float displacement = 0;
        _stepCurve = new AnimationCurve();
        _stepCurve.AddKey(0, 0);

        for (int i = 1; i <= steps; ++i)
        {
            displacement += aCurve.Evaluate(i / steps) / steps;
            _stepCurve.AddKey(i / steps, displacement);
        }

        _endDisplacement = _stepCurve.Evaluate(1);
        if (_endDisplacement == 0)
            _endDisplacement = 1;
    }

    public void Move()
    {
        // Disable hero controller, ability input, movement
        SetMovementActive(false);
        // Move along curve
        StartCoroutine(MoveAlongCurve());
    }

    void SetMovementActive(bool active)
    {
        if (_abilityManager != null)
        {
            _abilityManager.BasicAttack.CanUse = active;
            _abilityManager.Ability1.CanUse = active;
            _abilityManager.Ability2.CanUse = active;

            _currentAbility.CanUse = true;
        }
        if (_heroControl != null)
            _heroControl.canMove = active;
        if (_targetFollow != null)
            _targetFollow.CanFollow = active;

       
    }
    
    IEnumerator MoveAlongCurve()
    {
        float currTime = 0;

        _rb.freezeRotation = true;

        while (currTime <= MoveTime)
        {
            transform.position = Vector3.Lerp(StartPos, EndPos, _stepCurve.Evaluate(currTime / MoveTime) / _endDisplacement);
            currTime += Time.deltaTime;
            yield return null;
        }

        _rb.freezeRotation = false;

        // Reenable abilities
        SetMovementActive(true);
    }
}
