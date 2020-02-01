using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alternate : MonoBehaviour
{
    public GameObject m_spark1 = null;
    public GameObject m_spark2 = null;

    public float m_speed = 1;
    private float m_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer > m_speed)
        {
            m_timer -= m_speed;
            Toggle();
        }
    }

    private void Toggle()
    {
        m_spark1.gameObject.SetActive(!m_spark1.gameObject.activeSelf);
        m_spark2.gameObject.SetActive(!m_spark2.gameObject.activeSelf);
    }
}
