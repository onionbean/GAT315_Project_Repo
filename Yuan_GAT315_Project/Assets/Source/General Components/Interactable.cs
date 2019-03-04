using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class Interactable : MonoBehaviour {
    [SerializeField] public GameObject PlayerPrefab;
    public SpriteRenderer InteractSprite;
    [SerializeField] public UnityEvent OnTriggerEvent;

    public bool isTriggered { get; set; }
    
    CircleCollider2D _circle;
    bool _inRadius = false;


    private void Awake()
    {
        _circle = GetComponent<CircleCollider2D>();
        if (InteractSprite != null)
            InteractSprite.enabled = false;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_inRadius && Input.GetButtonDown("Jump") && !isTriggered)
        {
            isTriggered = true;
            if (OnTriggerEvent != null)
                OnTriggerEvent.Invoke();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PlayerPrefab.tag)
        {
            _inRadius = true;

            if (InteractSprite != null)
                InteractSprite.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PlayerPrefab.tag)
        {
            _inRadius = false;
            isTriggered = false;

            if (InteractSprite != null)
                InteractSprite.enabled = false;
        }
    }
}
