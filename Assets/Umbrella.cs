using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : TestObject
{
    public Animator m_animator = null;

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
}
