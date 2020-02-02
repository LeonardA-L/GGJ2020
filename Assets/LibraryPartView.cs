using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Level;

public class LibraryPartView : MonoBehaviour
{
    public TestObject m_part = null;
    public Button m_button = null;
    public TextMeshProUGUI m_amountText = null;
    private int m_amount = 0;
    public int Amount
    {
        get => m_amount;
        set
        {
            m_amount = value;
            m_amountText.text = $"{m_amount}";
            m_button.interactable |= m_amount > 0;
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public void Init(Entry part)
    {
        EventTrigger trigger = m_button.gameObject.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            Destroy(trigger);
        }

        if (part == null || part.ItemPrefab == null)
        {
            m_part = null;
            m_amount = 0;
            m_button.interactable = false;
            m_amountText.gameObject.SetActive(false);
            return;
        }

        m_amountText.gameObject.SetActive(true);
        m_button.interactable = true;

        m_part = part.ItemPrefab;
        m_amount = part.Amount;
        m_amountText.text = $"{m_amount}";

        trigger = m_button.gameObject.AddComponent<EventTrigger>();

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

    internal void Clear()
    {
        Init(null);
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
        if(m_amount <= 0)
        {
            return;
        }

        m_amount--;
        m_button.interactable &= m_amount > 0;
        m_amountText.text = $"{m_amount}";
        LibraryController.Instance.OnClick(m_part);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
