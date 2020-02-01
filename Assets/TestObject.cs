using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestObject : MonoBehaviour
{
    [SerializeField]
    private string m_descriptionKey = null;
    public ModuleButton m_button = null;
    private Rigidbody2D m_rigidbody = null;
    public Rigidbody2D RigidBody => m_rigidbody ?? (m_rigidbody = GetComponent<Rigidbody2D>());
    public string DescriptionKey => m_descriptionKey;

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

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        m_activeColliders = new List<TestObject>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    protected virtual void FixedUpdate() { }

    public void TriggerEnter(TestObject other)
    {
        if (other != this && !m_activeColliders.Contains(other))
        {
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
