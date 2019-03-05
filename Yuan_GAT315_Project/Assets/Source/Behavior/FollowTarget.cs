using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Target))]
public class FollowTarget : MonoBehaviour
{
    // Movement Speed 
    [SerializeField] float FollowSpeed = 5f;

    // Stopping distance 
    [SerializeField] float StoppingDist = 0f;

    // Whether the object needs a range to follow the target
    [SerializeField] bool HasFollowRange = false;

    // Range to follow the player
    [SerializeField] float FollowRange = 20f;

    // Turns to rotate 
    [SerializeField] bool RotateMoves = true;
    [SerializeField] float RotationSpeed = 150;

    // Flag that can be set outside to whether the target can be followed
    public bool CanFollow = true;

    // Follower's rigidbody 
    Rigidbody2D rb;

    // Targeting ref
    Target targeting;

    Vector2 _velocity;
    Vector2 _lastMoveDir;
    Vector2 _lastPos; 

	// Use this for initialization
	void Start ()
    {
        // Get rigidbody of owner
        rb = GetComponent<Rigidbody2D>();
        targeting = GetComponent<Target>();
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        TrackVelocity();

        if (CanFollow)
            Follow();
    }

    // Move to target object destination
    void Follow()
    {
        // Get nearest target
        GameObject currTarget = targeting.Nearest();

        // If current target exists, see if we can follow it
        if (currTarget)
        {
            // Get distance between target and follower
            float dist = targeting.TargetDistance();
            //Debug.Log(gameObject.name + "Distance: " + dist);
            // If target needs range to follow and range is out of range, return
            if (HasFollowRange && (dist > FollowRange))
                return;

            // Get vector between target and follower
            Vector2 followDir = targeting.TargetDirection();

            if (RotateMoves)
            {
                // Rotate
                float rot = Vector2.SignedAngle(Vector2.right, followDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(new Vector3(0, 0, rot - 90)), Time.deltaTime * RotationSpeed);
            }

            // Move if not at stoppingm  distance 
            if (dist >= StoppingDist)
            {
                // Move
                Vector2 moveVec = transform.up * FollowSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + moveVec);

                rb.velocity = followDir * FollowSpeed;

            }
            else
                rb.velocity = Vector2.zero;
        }
        else
        {
            Vector2 dir = rb.velocity.normalized;
            float rot = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.Euler(new Vector3(0, 0, rot - 90)), Time.deltaTime * RotationSpeed * 3);
        }
    }

    void TrackVelocity()
    {
        _velocity = (rb.position - _lastPos) * (1 / Time.deltaTime);
        _lastMoveDir = _velocity;
        _lastMoveDir.Normalize();

        _lastPos = rb.position;
    }

    public Vector2 GetVelocity()
    {
        return _velocity;
    }

    public Vector2 GetMoveDir()
    {
        Vector2 dir = _lastMoveDir;
        dir.Normalize();
        return dir;
    }
}


