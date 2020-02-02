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
    public TextMeshProUGUI m_pressAnyKey = null;

    void Awake()
    {
        TestCreate.Instance.m_currentLevel = m_firstLevel;
    }

    void Update()
    {
        if (!IsInit && Input.anyKeyDown)
        {
            StartCoroutine(Go());
        }

        Color col = m_pressAnyKey.color;
        col.a = Mathf.Abs(Mathf.Sin(Time.time));
        m_pressAnyKey.color = col;
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
}
