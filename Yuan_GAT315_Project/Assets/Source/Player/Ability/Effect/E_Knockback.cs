using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_EffectKnockback_HeroName_AbilityName", menuName = "ScriptableObjects/Effects/Knockback", order = 2)]
public class E_Knockback : Effect {
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */

    [SerializeField] public float Force = 10;
    [SerializeField] public bool KnockbackOnce = true;
    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */

    private Rigidbody2D _rb;

    private FollowTarget _follow;
    private HeroController _controller; 

    protected override void Affect(GameObject target)
    {
        base.Affect(target);

        _follow = target.GetComponent<FollowTarget>();
        _controller = target.GetComponent<HeroController>();
        _rb = target.GetComponent<Rigidbody2D>();

        // Disable movement code 
        if (_follow != null)
            _follow.enabled = false;
        if (_controller != null)
            _controller.enabled = false;

        if (_rb != null && source != null)
        {
            // Add force relative to source
            Vector2 dir = source.transform.up;
            dir.Normalize();

            _rb.velocity += dir * Force;
        }
    }

    public override void Shutdown(GameObject target)
    {
        base.Shutdown(target);

        // Reset movement references        
        if (_follow != null)
            _follow.enabled = true;
        if (_controller != null)
            _controller.enabled = true;
    }
}
