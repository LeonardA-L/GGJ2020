using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopedWeight : TestObject
{
    public float m_lerp = 0.1f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override Type GetJointType()
    {
        return typeof(HingeJoint2D);
    }
}
