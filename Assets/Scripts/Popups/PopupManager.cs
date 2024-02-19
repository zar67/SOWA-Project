using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PopupType
{
    DEFAULT
}

[Serializable]
public struct PopupData
{
    public PopupType Type;

    public bool ShowCloseButton;
    public UnityAction CloseButtonAction;

    public string BodyText;

    public PopupButtonData[] ButtonDatas;
}

[Serializable]
public struct PopupButtonData
{
    public string Text;
    public UnityAction Action;
    public bool CloseOnClick;
}

public class PopupManager : SingletonMonoBehaviour<PopupManager>
{
    [SerializeField] private Popup[] m_popupPrefabs;
    private Dictionary<PopupType, Popup> m_popupPrefabsByType;

    private Dictionary<PopupType, List<Popup>> m_openPopups;
    private Dictionary<PopupType, List<Popup>> m_closedPopups;

    public bool HasOpenPopup()
    {
        foreach (List<Popup> openPopups in m_openPopups.Values)
        {
            if (openPopups.Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    public void OpenPopup(PopupData data)
    {
        if (!m_closedPopups.ContainsKey(data.Type))
        {
            m_closedPopups.Add(data.Type, new List<Popup>());
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

        Popup popupToOpen = m_closedPopups[data.Type][0];

        if (!m_openPopups.ContainsKey(data.Type))
        {
            m_openPopups.Add(data.Type, new List<Popup>());
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

        m_openPopups = new Dictionary<PopupType, List<Popup>>();
        m_closedPopups = new Dictionary<PopupType, List<Popup>>();

        OnValidate();
    }

    private void OnValidate()
    {
        m_popupPrefabsByType = new Dictionary<PopupType, Popup>();
        foreach (Popup popup in m_popupPrefabs)
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

        Popup newPopup = Instantiate(m_popupPrefabsByType[type], transform);

        if (!m_closedPopups.ContainsKey(type))
        {
            m_closedPopups.Add(type, new List<Popup>());
        }

        m_closedPopups[type].Add(newPopup);

        return true;
    }
}
