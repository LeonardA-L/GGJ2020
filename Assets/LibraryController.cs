using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryController : Singleton<LibraryController>
{
    public LocalizedText m_descriptionText = null;

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
}
