using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PopupType
{
    NONE,
    GENERIC_INFO,
    LITTER_RECORDING,
    SETTINGS
}

public class PopupManager : SingletonMonoBehaviour<PopupManager>
{
    [SerializeField] private BasePopup[] m_popupPrefabs;
    private Dictionary<PopupType, BasePopup> m_popupPrefabsByType;

    private Dictionary<PopupType, List<BasePopup>> m_openPopups;
    private Dictionary<PopupType, List<BasePopup>> m_closedPopups;

    public bool HasOpenPopup()
    {
        foreach (List<BasePopup> openPopups in m_openPopups.Values)
        {
            if (openPopups.Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    public void OpenPopup(BasePopupData data)
    {
        if (!m_closedPopups.ContainsKey(data.Type))
        {
            m_closedPopups.Add(data.Type, new List<BasePopup>());
        }

        if (m_closedPopups[data.Type].Count == 0)
        {
            bool instantiated = InstantiatePopup(data.Type);
            if (!instantiated)
            {
                Debug.LogError($"Could not open popup of type {data.Type}: Failed to instantiate new popup.");
                return;
            }
        }

        BasePopup popupToOpen = m_closedPopups[data.Type][0];

        if (!m_openPopups.ContainsKey(data.Type))
        {
            m_openPopups.Add(data.Type, new List<BasePopup>());
        }

        m_openPopups[data.Type].Add(popupToOpen);
        m_closedPopups[data.Type].Remove(popupToOpen);

        popupToOpen.Init(data);
        popupToOpen.Open();
    }

    protected override void Awake()
    {
        base.Awake();

        // Clear any leftover popups from editing.
        gameObject.DestroyChildren();

        m_openPopups = new Dictionary<PopupType, List<BasePopup>>();
        m_closedPopups = new Dictionary<PopupType, List<BasePopup>>();

        BasePopup.OnPopupClose += HandlePopupClosed;

        OnValidate();
    }

    private void OnValidate()
    {
        m_popupPrefabsByType = new Dictionary<PopupType, BasePopup>();
        foreach (BasePopup popup in m_popupPrefabs)
        {
            if (popup == null)
            {
                continue;
            }

            if (!m_popupPrefabsByType.ContainsKey(popup.Type))
            {
                m_popupPrefabsByType.Add(popup.Type, popup);
            }
        }
    }

    private bool InstantiatePopup(PopupType type)
    {
        if (!m_popupPrefabsByType.ContainsKey(type))
        {
            Debug.LogError($"Popup of type {type} does not have associated prefab.");
            return false;
        }

        BasePopup newPopup = Instantiate(m_popupPrefabsByType[type], transform);

        if (!m_closedPopups.ContainsKey(type))
        {
            m_closedPopups.Add(type, new List<BasePopup>());
        }

        m_closedPopups[type].Add(newPopup);

        return true;
    }

    private void HandlePopupClosed(BasePopup popup)
    {
        if (m_openPopups.ContainsKey(popup.Type))
        {
            if (m_openPopups[popup.Type].Contains(popup))
            {
                m_openPopups[popup.Type].Remove(popup);
            }
        }

        if (!m_closedPopups.ContainsKey(popup.Type))
        {
            m_closedPopups.Add(popup.Type, new List<BasePopup>());
        }

        m_closedPopups[popup.Type].Add(popup);
    }   
}
