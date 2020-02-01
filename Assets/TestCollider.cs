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

    private void Awake()
    {
        m_activeColliders = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hotspot")
        {
            m_activeColliders.Add(collision);
            m_parent.TriggerEnter(collision.GetComponent<TestCollider>().Parent);
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
