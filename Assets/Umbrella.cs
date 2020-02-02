using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : TestObject
{
    public Animator m_animator = null;
    public float m_counterForce = 0.3f;

    public override void OnActivate()
    {
        base.OnActivate();
        m_animator.SetTrigger("Activate");
    }
    public override void OnDeactivate()
    {
        base.OnDeactivate();
        m_animator.SetTrigger("Deactivate");
    }
    protected override void FixedUpdate()
    {
        if (IsActive)
        {
            float dot = Vector3.Dot(-RigidBody.velocity.normalized, transform.up.normalized);
            RigidBody.AddRelativeForce(dot * transform.up.normalized * RigidBody.velocity.magnitude * m_counterForce);
        }
    }
}
