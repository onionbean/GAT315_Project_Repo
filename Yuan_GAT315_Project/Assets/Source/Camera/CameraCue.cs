using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adjust camera towards direction of cue 
public class CameraCue : MonoBehaviour {
    [SerializeField] private PlayerCamera TargetCamera;
    [SerializeField] private bool UseSpriteVisibility = true;
    [SerializeField] public float CueWeight= 2;
    [SerializeField] public float CueDetectionRadius = 3;
    private SpriteRenderer _sprite;

    private bool _inView = false;

    private void Awake()
    {
        if (UseSpriteVisibility)
            _sprite = GetComponent<SpriteRenderer>();

        if (TargetCamera == null)
            TargetCamera = Camera.main.GetComponent<PlayerCamera>();

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // If in view
	}

    // Add cue shift to camera
    private void OnBecameVisible()
    {
        TargetCamera.AddCue(this);
        _inView = true;
    }

    private void OnBecameInvisible()
    {
        _inView = false;
        TargetCamera.RemoveCue(this);
    }
}
