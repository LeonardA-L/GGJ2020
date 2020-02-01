using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LibraryPartView : MonoBehaviour
{
    public TestObject m_part = null;
    public Button m_button = null;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (m_part == null)
        {
            m_button.interactable = false;
            Destroy(this);
            return;
        }

        EventTrigger trigger = m_button.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener(OnClick);
        trigger.triggers.Add(pointerDown);

        var pointerHover = new EventTrigger.Entry();
        pointerHover.eventID = EventTriggerType.PointerEnter;
        pointerHover.callback.AddListener(OnHover);
        trigger.triggers.Add(pointerHover);

        var pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener(OnExit);
        trigger.triggers.Add(pointerExit);
    }

    private void OnExit(BaseEventData arg0)
    {
        LibraryController.Instance.OnExit();
    }

    private void OnHover(BaseEventData arg0)
    {
        LibraryController.Instance.OnEnter(m_part);
    }

    private void OnClick(BaseEventData arg0)
    {
        LibraryController.Instance.OnClick(m_part);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
