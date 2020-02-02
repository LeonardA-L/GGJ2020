using System;
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

    public CanvasGroup m_mainMenu = null;
    public GameObject m_winScreen = null;
    public GameObject m_gameOverScreen = null;
    public GameObject m_tryAgain = null;
    public GameObject m_nextLevel = null;

    public LocalizedText m_gameOverReasonText = null;


    public GameObject m_tapePref = null;

    public Level m_currentLevel = null;
    public List<Level> m_allLevels = null;
    public void SetLevel(Level newLevel)
    {
        m_currentLevel = newLevel;
    }

    public TestObject m_base = null;
    private TestObject m_instance = null;

    public float m_rotSpeed = 1;
    public float m_currentRotSpeed = 0;

    public bool IsNavigating { get; set; } = false;
    public bool GameOver { get; set; } = false;

    public GameObject m_buildingInterface = null;

    private Vector3 m_basePosition;
    private Quaternion m_baseRotation;
    private List<TapeData> m_allTapes = null;

    private int m_currentId = 0;

    private List<ModuleButton> m_allButtons = null;

    public float Distance => (m_base.transform.position - m_basePosition).x;

    public void StartNavigating()
    {
        IsNavigating = true;
        m_buildingInterface.SetActive(false);

        m_base.RigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ResetGame(bool clean)
    {
        GameOver = false;
        m_base.transform.position = m_basePosition;
        m_base.transform.rotation = m_baseRotation;
        m_base.RigidBody.bodyType = RigidbodyType2D.Kinematic;
        m_base.RigidBody.velocity = Vector2.zero;
        m_base.RigidBody.angularVelocity = 0;
        IsNavigating = false;
        m_buildingInterface.SetActive(true);
        if (clean)
        {
            var buttonsToRemove = new List<ModuleButton>(m_allButtons);
            foreach (var item in buttonsToRemove)
            {
                Destroy(item.gameObject);
            }
            m_allButtons.Clear();

            foreach (var item in m_allTapes)
            {
                Destroy(item.tape.gameObject);
            }
            m_allTapes.Clear();

            foreach (Transform child in m_base.transform)
            {
                if (child.gameObject.tag == "Module")
                {
                    Destroy(child.gameObject);
                }
            }

            LibraryController.Instance.InitLevel(m_currentLevel);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        I18n.Instance.Init();
        m_allTapes = new List<TapeData>();
        m_basePosition = m_base.transform.position;
        m_baseRotation = m_base.transform.rotation;
        m_allButtons = new List<ModuleButton>();
        ResetGame(true);
    }

    public void InstantiatePart(TestObject toInstantiate)
    {
        var instance = Instantiate(toInstantiate);
        instance.ID = m_currentId;
        m_currentId++;
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
                if(m_instance.m_button != null)
                {
                    var buttonInstance = Instantiate(m_instance.m_button, m_moduleButtonsWrapper);
                    buttonInstance.Init(m_instance, m_instance.ID);
                    m_allButtons.Add(buttonInstance);
                }
            }
            else
            {
                LibraryController.Instance.Release(m_instance.m_name);
                Destroy(m_instance.gameObject);
            }
            m_instance = null;
        }

        m_currentRotSpeed = (Input.GetButton("Fire2") && m_instance != null) ? m_rotSpeed : 0;

        if(!GameOver && m_base.transform.position.x < -4)
        {
            Lose("lose.tooLeft");
        }
        if (!GameOver && m_base.transform.position.y > 320)
        {
            Lose("lose.tooHigh");
        }
        if(!GameOver && Distance > m_currentLevel.DistanceGoal)
        {
            Win();
        }
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

    public void Lose(string reasonKey)
    {
        GameOver = true;
        m_gameOverScreen.SetActive(true);
        m_gameOverReasonText.SetText(reasonKey);
        m_winScreen.SetActive(false);
        m_mainMenu.gameObject.SetActive(false);
    }

    public void Win()
    {
        GameOver = true;
        m_gameOverScreen.SetActive(false);
        m_winScreen.SetActive(true);
        int idx = m_allLevels.IndexOf(m_currentLevel);
        m_nextLevel.SetActive(idx < m_allLevels.Count - 1);
        m_tryAgain.SetActive(idx >= m_allLevels.Count - 1);
        m_mainMenu.gameObject.SetActive(false);
    }

    public void GotoMainMenu()
    {
        m_gameOverScreen.SetActive(false);
        m_winScreen.SetActive(false);
        m_mainMenu.gameObject.SetActive(true);
        m_mainMenu.alpha = 1;
        MainMenuController.Instance.IsInit = false;
    }

    public void TryAgain()
    {
        m_gameOverScreen.SetActive(false);
        m_winScreen.SetActive(false);
        ResetGame(true);
    }

    public void NextLevel()
    {
        m_gameOverScreen.SetActive(false);
        m_winScreen.SetActive(false);
        int idx = m_allLevels.IndexOf(m_currentLevel);
        idx++;
        idx = Mathf.Min(idx, m_allLevels.Count);
        m_currentLevel = m_allLevels[idx];
        ResetGame(true);
    }
}
