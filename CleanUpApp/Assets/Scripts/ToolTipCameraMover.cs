using Mapbox.Examples;
using Mapbox.Unity.Map;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private void Update()
    {
        if (Input.GetMouseButton(0) && Input.touchCount <= 1)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count == 0 || results[0].gameObject.GetComponentInParent<LitterObject>() == null)
            {
                LitterObject.ClearToolTip();
                QuadTreeCameraMovement.FollowLocation = true;
            }
        }
    }

    private void HandleToolTipClicked(LitterObject obj)
    {
        if (obj == null)
        {
            return;
        }

        QuadTreeCameraMovement.FollowLocation = false;

        var pos = m_map.WorldToGeoPosition(obj.transform.position);
        m_map.UpdateMap(pos, m_map.Zoom);
    }
}
