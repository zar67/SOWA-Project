using Mapbox.Unity.Map;
using UnityEngine;
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
        if (Input.GetMouseButton(0))
        {
            LitterObject.ClearToolTip();
        }
    }

    private void HandleToolTipClicked(LitterObject obj)
    {
        if (obj == null)
        {
            return;
        }

        var pos = m_map.WorldToGeoPosition(obj.transform.position);
        m_map.UpdateMap(pos, m_map.Zoom);
    }
}
