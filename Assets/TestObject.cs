using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestObject : MonoBehaviour
{
    [SerializeField]
    private string m_descriptionKey = null;
    [SerializeField]
    public string m_name = null;
    public ModuleButton m_button = null;
    private Rigidbody2D m_rigidbody = null;
    public Rigidbody2D RigidBody => m_rigidbody ?? (m_rigidbody = GetComponent<Rigidbody2D>());
    public string DescriptionKey => m_descriptionKey;
    public Vector3 m_hitPoint;
    public int ID { get; set; } = 0;
    public ModuleButton Button { get; set; } = null;
    public Sprite ModuleIcon = null;

    public Renderer m_highlighter = null;

    public void SetHighlight(bool val)
    {
        if (m_highlighter != null)
        {
            m_highlighter.material.color = val ? new Color(1f, 133f/255f, 0f, 1f) : new Color(1f, 133f / 255f, 0f, 0f);
        }
    }

    public bool IsPlacing
    {
        get; set;
    } = false;

    public bool IsActive
    {
        get; set;
    } = false;

    public List<TestObject> m_activeColliders = null;

    public TestObject ActiveHotSpot => m_activeColliders.Count == 0 ? null : m_activeColliders[m_activeColliders.Count - 1];

    public virtual void OnActivate()
    {
    }

    public virtual void OnDeactivate()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag == "Ground")
        {
            TestCreate.Instance.Lose("lose.crash");
        }
    }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        m_activeColliders = new List<TestObject>();
        SetHighlight(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    protected virtual void FixedUpdate() { }
    public virtual Type GetJointType()
    {
        return typeof(FixedJoint2D);
    }

    public void TriggerEnter(TestObject other, Vector3 hitPoint)
    {
        if (other != this && !m_activeColliders.Contains(other))
        {
            m_hitPoint = hitPoint;
            m_activeColliders.Add(other);
        }
    }

    public void TriggerExit(TestObject other)
    {
        m_activeColliders.Remove(other);
    }

    void OnMouseDown()
    {
        TestCreate.Instance.Select(this, true);
    }

    private void OnMouseEnter()
    {
        LibraryController.Instance.OnEnter(this);
    }

    private void OnMouseExit()
    {
        LibraryController.Instance.OnExit();
    }
}
