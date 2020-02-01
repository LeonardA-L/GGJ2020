using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : TestObject
{
    public float m_force = 1;

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
        if (TestCreate.Instance.IsNavigating)
        {
            RigidBody.AddForce(m_force * Vector3.up);
        }
    }
}
