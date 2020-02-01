using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollingRandom : MonoBehaviour
{
    public List<GameObject> m_instances = null;
    public float m_screenHalfSize = 25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in m_instances)
        {
            float diffX = (item.transform.position - CameraController.Instance.m_cam.transform.position).x;
            if (diffX >= m_screenHalfSize * 7)
            {
                item.transform.position -= new Vector3(m_screenHalfSize * 8 + Random.Range(0, m_screenHalfSize * 4 - 1), 0, 0);
            }
            else if (diffX <= -m_screenHalfSize * 7)
            {
                item.transform.position += new Vector3(m_screenHalfSize * 8 + Random.Range(0, m_screenHalfSize * 4 - 1), 0, 0);
            }
        }
    }
}
