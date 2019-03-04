using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCamera : MonoBehaviour
{
    // reference to player object
    [SerializeField] public GameObject Player;

    // Z position of camera
    [SerializeField] public float CameraZ = -10;

    // Maximum shifting distance
    [SerializeField] public float MaxShiftingDistance = 5f;

    // Rate to shift by
    [SerializeField] public float ShiftFactor = 1f;

    [SerializeField] public float CameraSmoothTime = .3f;

    [SerializeField] public float MouseShiftWeight = 1;
    [SerializeField] public float PlayerShiftWeight = 1;
    [SerializeField] public float OverallCueWeight = .1f;

    // Mouse cursor object
    [SerializeField] public GameObject CursorObject;

    // current mouse position
    private Vector2 mousePos;

    private Vector3 _targetPos; // Target camera position
    private Vector3 _camVelocity;   // Current camera velocity

    private List<CameraCue> _cues;  // Current cues

    private void Awake()
    {
        _cues = new List<CameraCue>();
    }
    // Use this for initialization
    void Start ()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        // Set cursor to invisible
        Cursor.visible = false;
        
        // Instantiate cursor object
        if (CursorObject != null)
            CursorObject = Instantiate(CursorObject, mousePos, Quaternion.identity, null);

        _targetPos = new Vector3(Player.transform.position.x, Player.transform.position.y, CameraZ);
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    private void FixedUpdate()
    {
        if (Player)
        {
            CalculateTargetPosition();
            SmoothFollow();
            SetCursor();
        }
    }

    void CalculateTargetPosition()
    {
        // Average cues, mouse, and player positions based on their weights
        Vector3 cuePositions = CheckCues() * OverallCueWeight;
        Vector3 PlayerPosition = Player.transform.position * PlayerShiftWeight;
        Vector3 MousePosition = DynamicFollow() * MouseShiftWeight;

        if (_cues.Count == 0)
            _targetPos = (PlayerPosition + MousePosition) / (PlayerShiftWeight + MouseShiftWeight);
        else 
            _targetPos = (cuePositions + PlayerPosition + MousePosition) / (OverallCueWeight + PlayerShiftWeight + MouseShiftWeight);

        _targetPos.Set(_targetPos.x, _targetPos.y, CameraZ);
    }

    // Simple following function, for testing purposes
    void Follow()
    {
        // Set to player position
        gameObject.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, CameraZ);
    }

    void SmoothFollow()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _camVelocity, CameraSmoothTime);
        //Debug.Log(_targetPos);
    }

    // Dynamic camera shifting 
    Vector3 DynamicFollow()
    {
        // Get mouse and player position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = Player.transform.position;
        //Debug.Log(mousePos);
        // Get angle between mouse and player
        Vector2 mpVec = mousePos - playerPos;
        float theta = Mathf.Atan2(mpVec.y, mpVec.x) * Mathf.Rad2Deg;
        if (theta < 0)
            theta += 360f;
        //Debug.Log(theta);

        // Find and clamp the distance between mouse and player
        float distance = Vector2.Distance(mousePos, playerPos);
        distance = Mathf.Clamp(distance, 0, MaxShiftingDistance);
        //Debug.Log(distance);
        // Find coordinates of new position
        Vector2 camTo = new Vector2(playerPos.x + Mathf.Cos(theta * Mathf.Deg2Rad) * distance,
                                    playerPos.y + Mathf.Sin(theta * Mathf.Deg2Rad) * distance);

        //Debug.Log(new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)));

        // Lerp camera position
        return new Vector3(camTo.x /*- gameObject.transform.position.x) / ShiftFactor*/,
            camTo.y /*- gameObject.transform.position.y) / ShiftFactor*/,
            0);
    }

    // Set mouse cursor position
    void SetCursor()
    {
        if (CursorObject != null)
        {
            CursorObject.transform.position = mousePos;
        }
    }

    Vector3 CheckCues()
    {
        Vector3 avgPos = new Vector3(0, 0, 0);
        float weightSum = 0;

        // For each cue get the average position based on each cue's weight
        foreach (CameraCue cue in _cues)
        {
            avgPos += cue.transform.position * cue.CueWeight;
            weightSum += cue.CueWeight;
        }

        if (weightSum > 0)
            avgPos /= weightSum;

        return new Vector3(avgPos.x, avgPos.y, 0);
    }

    public void AddCue(CameraCue cue)
    {
        _cues.Add(cue);
        // Get direction
        //Vector3 dir = cue.transform.position - transform.position;
        //dir.Normalize();

        //// Add dir and strength to target pos
        //_targetPos += dir * cue.CueWeight;
    }

    public void RemoveCue(CameraCue cue)
    {
        _cues.Remove(cue);
    }
}
