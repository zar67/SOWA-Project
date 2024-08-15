using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ResetMapController : MonoBehaviour
{
    public static event Action RestMapClicked;

    [SerializeField] private Button m_resetButton;
    [SerializeField] private AbstractMap m_map;

    private AbstractLocationProvider m_locationProvider = null;

    private void OnEnable()
    {
        m_resetButton.onClick.AddListener(HandleResetClicked);
    }

    private void OnDisable()
    {
        m_resetButton.onClick.RemoveListener(HandleResetClicked);
    }

    private void HandleResetClicked()
    {
        if (m_locationProvider == null)
        {
            m_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        }

        RestMapClicked?.Invoke();
        m_map.UpdateMap(m_locationProvider.CurrentLocation.LatitudeLongitude, m_map.Zoom);
    }
}
