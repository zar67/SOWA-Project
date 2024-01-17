using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera m_targetCamera;
    [SerializeField] private Button m_resetPositionButton;

    private Vector3 m_defaultCameraPosition;
    private Vector3 m_touchStart;

    private void Awake()
    {
        m_defaultCameraPosition = m_targetCamera.transform.position;
    }

    private void OnEnable()
    {
        m_resetPositionButton.onClick.AddListener(ResetCameraPosition);
    }

    private void OnDisable()
    {
        m_resetPositionButton.onClick.RemoveListener(ResetCameraPosition);
    }

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

    private void ResetCameraPosition()
    {
        m_targetCamera.transform.position = m_defaultCameraPosition;
    }
}
