using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera m_targetCamera;

    private Vector3 m_touchStart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var currentMousePosition = m_targetCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            m_touchStart = currentMousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = m_touchStart - currentMousePosition;
            m_targetCamera.transform.position += direction;
        }
    }
}
