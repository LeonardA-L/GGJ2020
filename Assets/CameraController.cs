using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public Camera m_cam = null;
    public float m_lerp = 0.1f;

    private Vector3 m_workshopPosition;
    private Vector3 m_baseDiff;
    private float m_baseZ;

    void Awake()
    {
        m_workshopPosition = m_cam.transform.position;
        m_baseDiff = TestCreate.Instance.m_base.transform.position - m_workshopPosition;
        m_baseDiff.x = 0;
        m_baseZ = m_cam.transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TestCreate.Instance.IsNavigating)
        {
            Vector3 lerped = Vector3.Lerp(m_cam.transform.position, TestCreate.Instance.m_base.transform.position - m_baseDiff, m_lerp);
            lerped.y = Mathf.Clamp(lerped.y, 0, 325f);
            float limitLeft = -4;
            lerped.x = Mathf.Max(limitLeft, lerped.x);
            lerped.z = m_baseZ;
            m_cam.transform.position = lerped;
        } else
        {
            m_cam.transform.position = m_workshopPosition;
        }
    }
}
