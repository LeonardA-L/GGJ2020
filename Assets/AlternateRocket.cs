using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateRocket : TestObject
{
    public SpriteRenderer m_flame = null;
    public GameObject m_flameObj = null;
    public ParticleSystem m_flamePart = null;
    public float m_force = 1;
    public bool m_hasFuel = false;
    public float m_fuel = 45;
    public Transform m_flameWrapper = null;
    private bool Depleted { get; set; } = false;

    public float boostDuration = 2;
    public float betweenBoostDuration = 4;

    protected override void Awake()
    {
        base.Awake();
    }

    protected float Timer()
    {
        return Time.time - TestCreate.Instance.NavigatingStartTime;
    }

    protected override void Update()
    {
        base.Update();
        if (m_flameObj != null)
        {
            m_flameObj.SetActive(Functionning || IsPlacing);
        }
        //if (m_flame != null)
        //{
        //    m_flame.color = (Functionning) ? Color.white : (IsPlacing ? new Color(1, 1, 1, 0.8f) : new Color(0, 0, 0, 0));
        //    m_flameWrapper.transform.localScale = (Timer() % (boostDuration + betweenBoostDuration) <= boostDuration) ? 1f * new Vector3(1, 1, 1) : Vector3.zero;
        //}
        if (m_flamePart != null)
        {
            if (Functionning && !m_flamePart.isPlaying && (Timer() % (boostDuration + betweenBoostDuration) <= boostDuration))
            {
                m_flamePart.Play();
                AudioManager.Instance.PlaySound("GlouglouFX");
            }
            if (!Functionning && m_flamePart.isPlaying || !(Timer() % (boostDuration + betweenBoostDuration) <= boostDuration))
            {
                m_flamePart.Stop();
                AudioManager.Instance.PlaySound("GlouglouFX");
            }
        }

        if (Functionning && m_hasFuel)
        {
            m_fuel -= Time.deltaTime;
            if (m_fuel <= 0)
            {
                Depleted = true;
            }
        }
    }

    private void OnDestroy()
    {

            AudioManager.Instance.StopSound("GlouglouFX");
    }

    public bool Functionning => (TestCreate.Instance.IsNavigating && !Depleted);

    protected override void FixedUpdate()
    {
        if (Functionning && (Timer() % (boostDuration + betweenBoostDuration) <= boostDuration))
        {
            RigidBody.AddRelativeForce(new Vector2(0, m_force));
        }
    }
}