using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : TestObject
{
    public SpriteRenderer m_flame = null;
    public float m_force = 1;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Update()
    {
        base.Update();
        m_flame.color = (IsActive && TestCreate.Instance.IsNavigating) ? Color.white : (IsPlacing ? new Color(1,1,1,0.8f) : new Color(0, 0, 0, 0));
    }

    protected override void FixedUpdate()
    {
        if (IsActive && TestCreate.Instance.IsNavigating)
        {
            RigidBody.AddRelativeForce(new Vector2(0, m_force));
        }
    }
}
