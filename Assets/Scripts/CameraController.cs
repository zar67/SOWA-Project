using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera m_targetCamera;
    [SerializeField] private Button m_resetPositionButton;

    [Header("Zoom Config")]
    [SerializeField] private float m_zoomMinSize = 20;
    [SerializeField] private float m_zoomMaxSize = 60;
    [SerializeField] private float m_zoomScaleFactor = 10;

    private Vector3 m_defaultCameraPosition;
    private float m_defaultCameraZoom = 40;
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
        Vector3 currentMousePosition = m_targetCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            m_touchStart = currentMousePosition;
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPreviousPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePreviousPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPreviousPos - touchOnePreviousPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            HandleZoom(difference * 0.01f);
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 direction = m_touchStart - currentMousePosition;
                m_targetCamera.transform.position += direction;
            }

            float mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (mouseScrollInput != 0)
            {
                HandleZoom(mouseScrollInput);
            }
        }
    }

    private void HandleZoom(float increment)
    {
        increment = increment * m_zoomScaleFactor;
        m_targetCamera.orthographicSize = Mathf.Clamp(m_targetCamera.orthographicSize - increment, m_zoomMinSize, m_zoomMaxSize);
    }

    private void ResetCameraPosition()
    {
        m_targetCamera.transform.position = m_defaultCameraPosition;
        m_targetCamera.orthographicSize = m_defaultCameraZoom;
    }
}
