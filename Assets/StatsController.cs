using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public TextMeshProUGUI m_distanceText = null;
    public TextMeshProUGUI m_weightText = null;
    public TextMeshProUGUI m_altitudeText = null;
    public TextMeshProUGUI m_topAltitudeText = null;
    public TextMeshProUGUI m_speedText = null;
    public TextMeshProUGUI m_topSpeedText = null;

    public TestObject m_base = null;

    private float m_topSpeed = 0;
    private float m_topAltitude = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = TestCreate.Instance.Distance;
        float speed = m_base.RigidBody.velocity.magnitude;
        float altitude = m_base.transform.position.y;
        m_topSpeed = Mathf.Max(speed, m_topSpeed);
        m_topAltitude = Mathf.Max(altitude, m_topAltitude);

        m_distanceText.text = $"{distance.ToString("####0.00")}m";
        m_altitudeText.text = $"{altitude.ToString("####0.00")}m";
        m_topAltitudeText.text = $"{m_topAltitude.ToString("####0.00")}m";
        m_speedText.text = $"{speed.ToString("####0.00")}mps";
        m_topSpeedText.text = $"{m_topSpeed.ToString("####0.00")}mps";
    }
}
