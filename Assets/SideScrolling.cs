using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    public float _width = 17.97f;
    public float _parallaxX = 0;
    public float _lastCameraPositionX = 0;
    public float _parallaxY = 0;
    public float _lastCameraPositionY = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _lastCameraPositionX = CameraController.Instance.m_cam.transform.position.x;
        _lastCameraPositionY = CameraController.Instance.m_cam.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float diffX = (transform.position - CameraController.Instance.m_cam.transform.position).x;
        transform.position += new Vector3(_parallaxX * (CameraController.Instance.m_cam.transform.position.x - _lastCameraPositionX), 0, 0);
        transform.position += new Vector3(0, _parallaxY * (CameraController.Instance.m_cam.transform.position.y - _lastCameraPositionY), 0);
        if (diffX >= _width)
        {
            transform.position -= new Vector3(_width * 2, 0, 0);
        } else if(diffX <= -_width)
        {
            transform.position += new Vector3(_width * 2, 0, 0);
        }
        _lastCameraPositionX = CameraController.Instance.m_cam.transform.position.x;
        _lastCameraPositionY = CameraController.Instance.m_cam.transform.position.y;
    }
}
