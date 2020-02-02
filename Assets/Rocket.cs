using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : TestObject
{
    public SpriteRenderer m_flame = null;
    public GameObject m_flameObj = null;
    public ParticleSystem m_flamePart = null;
    public float m_force = 1;
    public bool m_hasFuel = false;
    public float m_fuel = 45;
    private bool Depleted { get; set; } = false;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Update()
    {
        base.Update();
        if (m_flameObj != null)
        {
            m_flameObj.SetActive(Functionning || IsPlacing);
        }
        if (m_flame != null)
        {
            m_flame.color = (Functionning) ? Color.white : (IsPlacing ? new Color(1, 1, 1, 0.8f) : new Color(0, 0, 0, 0));
        }
        if (m_flamePart != null)
        {
            if(Functionning && !m_flamePart.isPlaying)
            {
                m_flamePart.Play();
            }
            if (!Functionning && m_flamePart.isPlaying)
            {
                m_flamePart.Pause();
            }
        }

        if (Functionning && m_hasFuel)
        {
            m_fuel -= Time.deltaTime;
            if(m_fuel <= 0)
            {
                Depleted = true;
            }
        }
    }

    public bool Functionning => (IsActive && TestCreate.Instance.IsNavigating && !Depleted);

    protected override void FixedUpdate()
    {
        if (Functionning)
        {
            RigidBody.AddRelativeForce(new Vector2(0, m_force));
        }
    }
}
