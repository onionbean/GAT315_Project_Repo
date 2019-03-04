using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/* Controls squad movement and switching between heroes */
[RequireComponent(typeof(Rigidbody2D))]
public class HeroController : MonoBehaviour {
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */
    [Tooltip("Squad Movement Speed")]
    [SerializeField] public float MovementSpeed;

    [Tooltip("Hero swap keys")]
    [SerializeField] public KeyCode SwapRight;
    [SerializeField] public KeyCode SwapLeft;

    [Tooltip("Hero Swap Lerp Speed")]
    [SerializeField] public float SwapTime = .1f;

    [Tooltip("Rotate speed towards angle of mouse")]
    [SerializeField] public float RotateSpeed = 20f;

    [Tooltip("Hero Parent Game object")]
    [SerializeField] public GameObject HeroParent;

    [Tooltip("Hero Position Parent object")]
    [SerializeField] public GameObject HeroPositionParent;

    [Header("GIZMO SETTINGS")]
    [Tooltip("Draw Gizmo for hero positions")]
    [SerializeField] public bool DrawPositionGizmos = false;
    [SerializeField] public bool GizmoUpdateHeroPositions = false;

    public List<BasicStatManager> Heroes;   // Hero array

    public bool canMove { get; set; }           // If we can move
    public BasicStatManager currentHero { get; set; }   // Current hero -- SHOULD ALWAYS BE FIRST ELEMENT OF HEROES

    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */

    private Rigidbody2D _rb;        // Rigidbody reference
    private int _currHeroIndex = 0; // Current hero index

    private CircleCollider2D _positionParentCCollider;  // Circle collider of position parent object
    private Transform[] _heroPositionSlots;               // Positions squad members can be in

    private float _currSwapTime = 0;    // Current swap time
    // Set up references
    void Awake()
    {
        // Get rigidbody
        _rb = GetComponent<Rigidbody2D>();
        canMove = true;

        // Get Heroes from parent
        if (HeroParent != null)
        {
            Heroes = new List<BasicStatManager>(HeroParent.GetComponentsInChildren<BasicStatManager>());
            currentHero = Heroes[0];
            _currHeroIndex = 0;
        }

        _positionParentCCollider = HeroPositionParent.GetComponent<CircleCollider2D>();

        // Setup position points
        CreatePositionSlots();

        // Move heroes to each position
        ResetHeroPositions();
    }

    // Set up event listening
    private void OnEnable()
    {
        
    }
    // Unregister events
    private void OnDisable()
    {

    }

    // Init values
    void Start()
    {
        Heroes[_currHeroIndex].isResting = false;
    }

    // Update every frame
    void Update()
    {
        SwapHero();
    }

    // Physics update
    void FixedUpdate()
    {
        if (canMove)
        {
            Move();

            Rotate();
        }
    }

    // Movement 
    private void Move()
    {
        Vector2 moveVec = new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, Input.GetAxis("Vertical") * MovementSpeed  );
        _rb.velocity = moveVec;
    }

    // Rotate towards mouse
    void Rotate()
    {
        // Get rotation between player and mouse
        Vector2 mouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mouseVec.Normalize();

        float rot = Mathf.Atan2(mouseVec.y, mouseVec.x) * Mathf.Rad2Deg - 90;
        Quaternion lookrot = Quaternion.AngleAxis(rot, Vector3.forward);

        // Rotate transform over time 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookrot, Time.deltaTime * RotateSpeed);
    }

    void SwapHero()
    {
        if (InputUtility.GetAxisDown("HeroSwitch", true))
        {
            // Pop off front hero and insert them at back
            BasicStatManager front = Heroes[0];
            front.isResting = true;
            Heroes.RemoveAt(0);
            Heroes.Add(front);

            Heroes[0].isResting = false;

            EventManager.TriggerEvent("OnHeroSwitch", Heroes[0].gameObject);
        }
        if (InputUtility.GetAxisDown("HeroSwitch", false))
        {
            Heroes[0].isResting = true;
            BasicStatManager back = Heroes[Heroes.Count - 1];
            Heroes.RemoveAt(Heroes.Count - 1);
            Heroes.Insert(0, back);

            back.isResting = false;

            EventManager.TriggerEvent("OnHeroSwitch", Heroes[0].gameObject);
        }

        currentHero = Heroes[0];

        // Lerp all heroes to destination position (hero slot positions)
        for (int i = 0; i < Heroes.Count; ++i)
        {
            BasicStatManager hero = Heroes[i];
            Debug.Log(hero.name + "SWITCHING HERO POSITION ");
            hero.gameObject.transform.position = Vector3.Lerp(hero.gameObject.transform.position, _heroPositionSlots[i].position, Time.deltaTime * SwapTime);
        }
    }

    
    // Create hero position slots based on the amount of heroes 
    void CreatePositionSlots()
    {
        Vector3[] slots = GameUtility.SubdivideCircleEdgePoints(_positionParentCCollider.offset, _positionParentCCollider.radius, Heroes.Count, 90);

        // Clear position slot parent's children
        GameUtility.DestroyChidren(HeroPositionParent.transform, true);

        // Instantiate game objects and attach them to the hero position parent
        foreach (Vector3 slot in slots)
        {
            GameObject emptySlot = new GameObject("PositionSlot");
            // Attach to heropositionparent
            emptySlot.transform.parent = HeroPositionParent.transform;
            // Set their positions
            emptySlot.transform.position = slot;
            Debug.Log("HERO POSITION PARENT CHILDREN ADDED: " + GameUtility.GetChildren(HeroPositionParent.transform).Length);
        }

        _heroPositionSlots = GameUtility.GetChildren(HeroPositionParent.transform);
        Debug.Log("HERO POSITION SLOTS SIZE: " + _heroPositionSlots.Length + ", HERO POSITION PARENT NAME: " + HeroPositionParent.name);
    }

    // Move heroes to their position slots
    void ResetHeroPositions()
    {
        if (_heroPositionSlots.Length == Heroes.Count)
        {
            for (int i = 0; i < Heroes.Count; ++i)
                Heroes[i].transform.position = _heroPositionSlots[i].position;

            currentHero = Heroes[0];
        }
        else
            Debug.LogError("HERO CONTROLLER ERROR: Hero count and position slots are different!");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!DrawPositionGizmos || !Application.isEditor)
            return;

        _positionParentCCollider = HeroPositionParent.GetComponent<CircleCollider2D>();
        Heroes = new List<BasicStatManager>(HeroParent.GetComponentsInChildren<BasicStatManager>());
        if (_positionParentCCollider != null)
        {
            CreatePositionSlots();
            
            // Draw position points as a sphere, with first being different colored
            for(int i = 0; i < _heroPositionSlots.Length; ++i)
            {
                if (i == 0)
                    Gizmos.color = Color.red;
                else 
                    Gizmos.color = Color.green;

                Gizmos.DrawWireSphere(_heroPositionSlots[i].position, .3f);
            }

            if (GizmoUpdateHeroPositions)
                ResetHeroPositions();
        }
    }
#endif
}
