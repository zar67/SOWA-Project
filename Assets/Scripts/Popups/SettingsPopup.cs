using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : BasePopup
{
    public static event Action<bool> OnLocationPinsEnabledChanged;

    [Header("Settings Options")]
    [SerializeField] private Toggle m_locationPinsToggle;

    [Header("App Info References")]
    [SerializeField] private TextMeshProUGUI m_versionText;

    public override PopupType Type => PopupType.SETTINGS;

    public override void Open()
    {
        base.Open();

        m_versionText.text = $"Version {Application.version}";

        m_locationPinsToggle.isOn = PlayerPrefs.GetInt(PrefsKeys.LOCATION_PINS_ENABLED_KEY, 1) == 1;

        m_locationPinsToggle.onValueChanged.AddListener(LocationPinsToggleChanged);
    }

    public override void Close()
    {
        base.Close();

        m_locationPinsToggle.onValueChanged.RemoveListener(LocationPinsToggleChanged);
    }

    private void LocationPinsToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(PrefsKeys.LOCATION_PINS_ENABLED_KEY, isOn ? 1 : 0);
        OnLocationPinsEnabledChanged?.Invoke(isOn);
    }
}
