using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonCombatController : MonoBehaviour {
    [SerializeField] public float MovementSpeed;

    private Rigidbody2D _rb;        // Rigidbody reference
    // Use this for initialization
    void Start ()
    {
        _rb = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        Move();
    }
    // Movement 
    private void Move()
    {
        Vector2 moveVec = new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, Input.GetAxis("Vertical") * MovementSpeed);
        _rb.velocity = moveVec;
    }
}
