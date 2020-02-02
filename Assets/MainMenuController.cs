using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : Singleton<MainMenuController>
{
    public bool IsInit
    {
        get; set;
    } = false;

    public CanvasGroup m_canvas = null;
    public Level m_firstLevel = null;
    public Level m_freeGame = null;
    public TextMeshProUGUI m_credits = null;
    private float m_creditsGoal = 0;
    void Awake()
    {
    }

    void Update()
    {
        /*if (!IsInit && Input.anyKeyDown)
        {
            StartCoroutine(Go());
        }

        Color col = m_pressAnyKey.color;
        col.a = Mathf.Abs(Mathf.Sin(Time.time));
        m_pressAnyKey.color = col;*/

        m_credits.color = Color.Lerp(m_credits.color, new Color(1, 1, 1, m_creditsGoal), 0.1f);
    }

    private IEnumerator Go()
    {
        IsInit = true;
        float timeMax = 2f;
        float time = timeMax;
        TestCreate.Instance.ResetGame(true);
        while (time > 0)
        {
            time -= Time.deltaTime;
            m_canvas.alpha = Mathf.Lerp(0, 1, time / timeMax);
            yield return null;
        }
        m_canvas.gameObject.SetActive(false);
    }

    public void Free()
    {
        TestCreate.Instance.m_currentLevel = m_freeGame;
        StartCoroutine(Go());
    }

    public void Begin()
    {
        TestCreate.Instance.m_currentLevel = m_firstLevel;
        StartCoroutine(Go());
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        m_creditsGoal = 1;
    }
    public void HideCredits()
    {
        m_creditsGoal = 0;
    }
}
