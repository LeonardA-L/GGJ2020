﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuleButton : MonoBehaviour
{
    public TestObject m_module = null;
    public TextMeshProUGUI m_text = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(TestObject module)
    {
        m_module = module;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        m_module.IsActive = !m_module.IsActive;
        if (m_module.IsActive)
        {
            m_text.text = "On";
        } else
        {
            m_text.text = "Off";
        }
    }
}
