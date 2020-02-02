using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField]
    private string m_key = null;
    private TextMeshProUGUI m_text;

    void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        Debug.Assert(m_text != null);
        I18n.Instance.OnLocaleChange += UpdateUI;
        Debug.Assert(!string.IsNullOrEmpty(m_key), "No key provided", this);
        UpdateUI();
    }

    private void UpdateUI()
    {
        m_text.text = string.IsNullOrEmpty(m_key) ? "" : I18n.Instance.__(m_key);
    }

    public void SetText(string key)
    {
        m_key = key;
        UpdateUI();
    }

    internal void Clear()
    {
        m_key = "";
        UpdateUI();
    }
}
