using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuleButton : MonoBehaviour
{
    public TestObject m_module = null;
    public int ID { get; set; } = 0;

    public GameObject m_on = null;
    public GameObject m_off = null;
    public Image m_gaugeFill = null;
    public GameObject m_gaugeOver = null;
    private Rocket m_rocket = null;
    private Magnet m_magnet = null;

    public GameObject m_magnetLocked = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (m_rocket != null)
        {
            m_gaugeFill.fillAmount = m_rocket.FuelRatio;
        }

        if(m_magnet != null)
        {
            m_magnetLocked.SetActive(!TestCreate.Instance.IsNavigating);
            m_off.SetActive(TestCreate.Instance.IsNavigating);
        }
    }

    public void Highlight()
    {
        m_module.SetHighlight(true);
    }

    public void Lowlight()
    {
        m_module.SetHighlight(false);
    }

    public void OnHoverMagnet()
    {
        if (m_magnet != null && TestCreate.Instance.IsNavigating)
        {
            m_on.SetActive(true);
        }
    }

    public void OnExitMagnet()
    {
        if (m_magnet != null)
        {
            m_on.SetActive(false);
        }
    }

    public void Init(TestObject module, int id)
    {
        ID = id;
        m_module = module;
        m_module.Button = this;

        if (m_gaugeFill != null)
        {
            if (module is Rocket rocket)
            {
                m_gaugeFill.gameObject.SetActive(rocket.m_hasFuel);
                m_gaugeOver.gameObject.SetActive(rocket.m_hasFuel);
                m_rocket = rocket;
            }
        }

        if (module is Magnet magnet)
        {
            m_magnet = magnet;
            m_magnetLocked.SetActive(true);
            m_on.SetActive(false);
            m_off.SetActive(false);
        }

        GetComponent<Button>().onClick.AddListener(OnClick);
        if (m_module.IsActive)
        {
            m_on.SetActive(true);
            m_off.SetActive(false);
        }
        else
        {
            m_on.SetActive(false);
            m_off.SetActive(true);
        }
    }

    private void OnClick()
    {
        if(m_magnet != null && !TestCreate.Instance.IsNavigating)
        {
            return;
        }

        m_module.IsActive = !m_module.IsActive;
        if (m_module.IsActive)
        {
            m_module.OnActivate();
            m_on.SetActive(true);
            m_off.SetActive(false);
        } else
        {
            m_module.OnDeactivate();
            m_on.SetActive(false);
            m_off.SetActive(true);
        }
    }
}
