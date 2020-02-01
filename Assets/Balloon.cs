using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : TestObject
{
    public float m_force = 1;

    public Rigidbody2D _balloonBody = null;

    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        _balloonBody.AddForce(m_force * Vector3.up);
    }

    public override Type GetJointType()
    {
        return typeof(HingeJoint2D);
    }
}
