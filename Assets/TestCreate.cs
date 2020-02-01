﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreate : Singleton<TestCreate>
{
    public class TapeData
    {
        public TestObject a;
        public TestObject b;
        public GameObject tape;
    }

    public Transform m_moduleButtonsWrapper = null;

    public GameObject m_tapePref = null;

    public TestObject m_base = null;
    private TestObject m_instance = null;

    public float m_rotSpeed = 1;
    public float m_currentRotSpeed = 0;

    public List<TestObject> m_modules = null;

    public bool IsNavigating { get; set; } = false;

    public GameObject m_buildingInterface = null;

    private Vector3 m_basePosition;
    private Quaternion m_baseRotation;
    private List<TapeData> m_allTapes = null;

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
        m_allTapes = new List<TapeData>();
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
                AddLink(m_instance.ActiveHotSpot, m_instance, m_instance.ActiveHotSpot.m_hitPoint);
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
            RemoveTape(testObject);
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
            m_instance.RigidBody.MoveRotation(m_instance.RigidBody.rotation - m_currentRotSpeed * Time.fixedDeltaTime);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        return mouse;
    }

    private void AddLink(TestObject m_first, TestObject m_second, Vector3 hitPosition)
    {
        var firstJoint = m_first.gameObject.AddComponent<FixedJoint2D>();
        var secondJoint = (Joint2D)m_second.gameObject.AddComponent(m_second.GetJointType());

        firstJoint.connectedBody = m_second.RigidBody;
        firstJoint.autoConfigureConnectedAnchor = true;
        firstJoint.enabled = false;

        secondJoint.connectedBody = m_first.RigidBody;
        //secondJoint.autoConfigureConnectedAnchor = true;

        AddTape(m_first, m_second, hitPosition, (m_first.transform.position - m_second.transform.position).normalized);
    }

    public void RemoveLink(TestObject objA, TestObject objB)
    {
        var jointsA = objA.GetComponents<Joint2D>();
        foreach (var joint in jointsA)
        {
            if (joint.connectedBody == objB.RigidBody)
            {
                Destroy(joint);
            }
        }
        var jointsB = objB.GetComponents<Joint2D>();
        foreach (var joint in jointsB)
        {
            if (joint.connectedBody == objA.RigidBody)
            {
                Destroy(joint);
            }
        }

        RemoveTape(objA, objB);
    }

    public void AddTape(TestObject objA, TestObject objB, Vector3 position, Vector3 forward)
    {
        var instance = Instantiate(m_tapePref, objA.transform);
        position.z = -9;
        instance.transform.position = position;
        instance.transform.right = forward;
        m_allTapes.Add(new TapeData()
        {
            a = objA,
            b = objB,
            tape = instance,
        });
    }

    public void RemoveTape(TestObject objA, TestObject objB)
    {
        TapeData toRemove = null;
        foreach (var tape in m_allTapes)
        {
            if((tape.a == objA && tape.b == objB) || (tape.a == objB && tape.b == objA))
            {
                Destroy(tape.tape.gameObject);
                toRemove = tape;
                break;
            }
        }
        if(toRemove != null)
        {
            m_allTapes.Remove(toRemove);
        }
    }

    public void RemoveTape(TestObject objA)
    {
        List<TapeData> toRemove = new List<TapeData>();
        foreach (var tape in m_allTapes)
        {
            if ((tape.a == objA) || (tape.b == objA))
            {
                Destroy(tape.tape.gameObject);
                toRemove.Add(tape);
            }
        }
        foreach (var item in toRemove)
        {
            m_allTapes.Remove(item);
        }
    }
}
