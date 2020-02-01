using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    public float _width = 17.97f;
    public float _parallax = 0;
    public float _lastCameraPosition = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _lastCameraPosition = CameraController.Instance.m_cam.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float diffX = (transform.position - CameraController.Instance.m_cam.transform.position).x;
        transform.position += new Vector3(_parallax * (CameraController.Instance.m_cam.transform.position.x - _lastCameraPosition), 0, 0);
        if (diffX >= _width)
        {
            transform.position -= new Vector3(_width * 2, 0, 0);
        } else if(diffX <= -_width)
        {
            transform.position += new Vector3(_width * 2, 0, 0);
        }
        _lastCameraPosition = CameraController.Instance.m_cam.transform.position.x;
    }
}
