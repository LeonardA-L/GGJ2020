using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryController : Singleton<LibraryController>
{
    public LocalizedText m_descriptionText = null;

    public List<LibraryPartView> m_allParts = null;

    // Start is called before the first frame update
    void Start()
    {
        m_descriptionText.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void OnEnter(TestObject part)
    {
        m_descriptionText.SetText(part.DescriptionKey);
    }

    internal void OnExit()
    {
        m_descriptionText.Clear();
    }

    internal void OnClick(TestObject part)
    {
        TestCreate.Instance.InstantiatePart(part);
    }

    internal void InitLevel(Level m_currentLevel)
    {
        foreach (var part in m_allParts)
        {
            part.Clear();
        }

        for(int i = 0; i < m_currentLevel.Library.Count; i++)
        {
            m_allParts[i].Init(m_currentLevel.Library[i]);
        }
    }

    internal void Release(TestObject m_prefab)
    {
        for (int i = 0; i < m_allParts.Count; i++)
        {
            if(m_allParts[i].m_part != null && m_allParts[i].m_part == m_prefab)
            {
                m_allParts[i].Amount++;
            }
        }
    }
}
