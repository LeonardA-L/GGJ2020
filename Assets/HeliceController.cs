using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliceController : MonoBehaviour
{
    public Rocket m_rocket;
    public Transform m_helice;
    public float m_rotSpeed = 2;
    // Start is called before the first frame update
    void Start()
    {
        m_rocket = GetComponent<Rocket>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rocket.Functionning)
        {
            m_helice.Rotate(m_helice.forward * m_rotSpeed * Time.deltaTime);
        }
    }
}
