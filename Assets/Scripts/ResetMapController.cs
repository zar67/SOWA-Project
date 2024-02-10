using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetMapController : MonoBehaviour
{
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

        m_map.UpdateMap(m_locationProvider.CurrentLocation.LatitudeLongitude, m_map.Zoom);
    }
}
