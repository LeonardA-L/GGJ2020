using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreate : Singleton<TestCreate>
{
    public Transform m_moduleButtonsWrapper = null;

    public TestObject m_base = null;
    private TestObject m_instance = null;

    public float m_rotSpeed = 1;
    public float m_currentRotSpeed = 0;

    public List<TestObject> m_modules = null;

    public bool IsNavigating { get; set; } = false;

    public GameObject m_buildingInterface = null;

    private Vector3 m_basePosition;
    private Quaternion m_baseRotation;

    public void StartNavigating()
    {
        IsNavigating = true;
        m_buildingInterface.SetActive(false);
        m_base.RigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ResetGame()
    {
        m_base.transform.position = m_basePosition;
        m_base.transform.rotation = m_baseRotation;
        m_base.RigidBody.bodyType = RigidbodyType2D.Kinematic;
        m_base.RigidBody.velocity = Vector2.zero;
        m_base.RigidBody.angularVelocity = 0;
        IsNavigating = false;
        m_buildingInterface.SetActive(true);
        foreach (Transform child in m_base.transform)
        {
            if(child.gameObject.tag == "Module")
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        I18n.Instance.Init();
        m_modules = new List<TestObject>();
        m_basePosition = m_base.transform.position;
        m_baseRotation = m_base.transform.rotation;
        ResetGame();
    }

    public void InstantiatePart(TestObject toInstantiate)
    {
        var instance = Instantiate(toInstantiate);
        Select(instance, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1") && m_instance != null)
        {
            if(m_instance.ActiveHotSpot != null)
            {
                AddLink(m_instance.ActiveHotSpot, m_instance);
                m_instance.transform.SetParent(m_instance.ActiveHotSpot.transform);
                m_instance.IsPlacing = false;
                m_modules.Add(m_instance);
                if(m_instance.m_button != null)
                {
                    var buttonInstance = Instantiate(m_instance.m_button, m_moduleButtonsWrapper);
                    buttonInstance.Init(m_instance);
                }
            }
            else
            {
                Destroy(m_instance.gameObject);
            }
            m_instance = null;
        }

        m_currentRotSpeed = (Input.GetButton("Fire2") && m_instance != null) ? m_rotSpeed : 0;
    }

    internal void Select(TestObject testObject, bool uncollide)
    {
        if (IsNavigating)
        {
            return;
        }

        if(m_instance != null)
        {
            return;
        }

        if(testObject.RigidBody.bodyType == RigidbodyType2D.Kinematic)
        {
            return;
        }

        if(uncollide)
        {
            var joints = testObject.GetComponents<FixedJoint2D>();
            if(joints.Length != 1)
            {
                return;
            }
            var joint = joints[0];
            var otherJoints = joint.connectedBody.GetComponents<FixedJoint2D>();
            foreach (var otherJoint in otherJoints)
            {
                if(otherJoint.connectedBody == testObject.RigidBody)
                {
                    Destroy(otherJoint);
                }
            }
            Destroy(joint);
        }

        m_instance = testObject;
        m_instance.transform.position = GetMousePosition();
        m_instance.IsPlacing = true;
    }

    private void FixedUpdate()
    {
        if (m_instance != null)
        {
            m_instance.RigidBody.MovePosition(GetMousePosition());
            m_instance.RigidBody.MoveRotation(m_instance.RigidBody.rotation + m_currentRotSpeed * Time.fixedDeltaTime);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        return mouse;
    }

    private void AddLink(TestObject m_first, TestObject m_second)
    {
        var firstJoint = m_first.gameObject.AddComponent<FixedJoint2D>();
        var secondJoint = (Joint2D)m_second.gameObject.AddComponent(m_second.GetJointType());

        firstJoint.connectedBody = m_second.RigidBody;
        firstJoint.autoConfigureConnectedAnchor = true;
        firstJoint.enabled = false;

        secondJoint.connectedBody = m_first.RigidBody;
        //secondJoint.autoConfigureConnectedAnchor = true;
    }
}
