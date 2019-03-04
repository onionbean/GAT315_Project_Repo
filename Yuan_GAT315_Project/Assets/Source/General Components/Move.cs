using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move an object in a direction at a certain speed
public class Move : MonoBehaviour
{
    [SerializeField] public Vector2 Direction;
    [SerializeField] public float Velocity;
    [SerializeField] bool UseRigidBody = false;
    [SerializeField] bool UseMouseDirection = false;

    Rigidbody2D m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (UseMouseDirection)
            Direction = (Input.mousePosition - transform.position);
        Direction.Normalize();

    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (UseRigidBody && m_rb != null)
            m_rb.velocity = Direction * Velocity;
        else
            transform.Translate(Direction * Time.deltaTime * Velocity, Space.World);
    }
}
