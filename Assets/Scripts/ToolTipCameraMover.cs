using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipCameraMover : MonoBehaviour
{
    [SerializeField] private AbstractMap m_map;

    private void OnEnable()
    {
        LitterObject.OnLitterButtonClicked += HandleToolTipClicked;  
    }

    private void OnDisable()
    {
        LitterObject.OnLitterButtonClicked -= HandleToolTipClicked;
    }

    private void HandleToolTipClicked(LitterObject obj)
    {
        if (!obj.IsShowing)
        {
            var pos = m_map.WorldToGeoPosition(obj.transform.position);
            m_map.UpdateMap(pos, m_map.Zoom);
        }
    }
}
