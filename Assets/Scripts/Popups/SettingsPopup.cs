using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : BasePopup
{
    public static event Action<bool> OnLocationPinsEnabledChanged;
    public static event Action<bool> OnHeatmapEnabledChanged;

    [Header("Settings Options")]
    [SerializeField] private Toggle m_locationPinsToggle;
    [SerializeField] private Toggle m_heatmapToggle;

    [Header("App Info References")]
    [SerializeField] private TextMeshProUGUI m_versionText;

    public override PopupType Type => PopupType.SETTINGS;

    public override void Open()
    {
        base.Open();

        m_versionText.text = $"Version {Application.version}";

        m_locationPinsToggle.isOn = PlayerPrefs.GetInt(PrefsKeys.LOCATION_PINS_ENABLED_KEY, 1) == 1;
        m_heatmapToggle.isOn = PlayerPrefs.GetInt(PrefsKeys.HEATMAP_ENABLED_KEY, 1) == 1;

        m_locationPinsToggle.onValueChanged.AddListener(LocationPinsToggleChanged);
        m_heatmapToggle.onValueChanged.AddListener(HeatmapToggleChanged);
    }

    public override void Close(string closeResult)
    {
        base.Close();

        m_locationPinsToggle.onValueChanged.RemoveListener(LocationPinsToggleChanged);
        m_heatmapToggle.onValueChanged.RemoveListener(HeatmapToggleChanged);
    }

    private void LocationPinsToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(PrefsKeys.LOCATION_PINS_ENABLED_KEY, isOn ? 1 : 0);
        OnLocationPinsEnabledChanged?.Invoke(isOn);
    }

    private void HeatmapToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(PrefsKeys.HEATMAP_ENABLED_KEY, isOn ? 1 : 0);
        OnHeatmapEnabledChanged?.Invoke(isOn);
    }
}
