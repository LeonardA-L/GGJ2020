using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollider : MonoBehaviour
{
    public TestObject m_parent = null;
    public TestObject Parent => m_parent;

    private List<Collider2D> m_activeColliders = null;
    public Collider2D ActiveCollider => m_activeColliders.Count == 0 ? null : m_activeColliders[m_activeColliders.Count - 1];

    public Collider2D m_collider = null;

    private Renderer m_renderer = null;
    public static bool s_show = true;
    public static float s_lerp = 0.1f;
    private Color m_colColorAlpha;
    private Color m_colColorTransp;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        m_activeColliders = new List<Collider2D>();
        m_colColorAlpha = m_renderer.material.color;
        m_colColorTransp = m_renderer.material.color;
        m_colColorTransp.a = 0;
    }

    private void Update()
    {
        m_renderer.material.color = Color.Lerp(m_renderer.material.color, s_show ? m_colColorAlpha : m_colColorTransp, s_lerp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hotspot")
        {
            m_activeColliders.Add(collision);
            m_parent.TriggerEnter(collision.GetComponent<TestCollider>().Parent, transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hotspot")
        {
            m_activeColliders.Remove(collision);
            m_parent.TriggerExit(collision.GetComponent<TestCollider>().Parent);
        }
    }
}
