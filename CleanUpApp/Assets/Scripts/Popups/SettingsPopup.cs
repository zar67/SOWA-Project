using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : BasePopup
{
    public static event Action<bool> OnLocationPinsEnabledChanged;
    public static event Action<bool> OnHeatmapEnabledChanged;
    public static event Action<int> OnLitterTimelineChanged;

    public static readonly string[] TIMELINE_NAMES = { "Last 24 Hours", "Last Week", "Last 4 Weeks", "Last 6 Months", "Last Year" };
    public static readonly int[] TIMELINE_HOURS = { 24, 168, 672, 4368, 8736};

    [Header("Settings Options")]
    [SerializeField] private Toggle m_locationPinsToggle;
    [SerializeField] private Toggle m_heatmapToggle;
    [SerializeField] private TextMeshProUGUI m_litterTimelineText;
    [SerializeField] private Button m_previousTimelineButton;
    [SerializeField] private Button m_nextTimelineButton;

    [Header("App Info References")]
    [SerializeField] private TextMeshProUGUI m_versionText;

    private int m_currentLitterTimelineIndex = 0;

    public override PopupType Type => PopupType.SETTINGS;

    public override void Open()
    {
        base.Open();

        m_versionText.text = $"Version {Application.version}";

        m_locationPinsToggle.isOn = PlayerPrefs.GetInt(PrefsKeys.LOCATION_PINS_ENABLED_KEY, 1) == 1;
        m_heatmapToggle.isOn = PlayerPrefs.GetInt(PrefsKeys.HEATMAP_ENABLED_KEY, 1) == 1;

        m_currentLitterTimelineIndex = PlayerPrefs.GetInt(PrefsKeys.LITTER_TIMELINE_KEY, 2);
        m_litterTimelineText.text = TIMELINE_NAMES[m_currentLitterTimelineIndex];

        m_locationPinsToggle.onValueChanged.AddListener(LocationPinsToggleChanged);
        m_heatmapToggle.onValueChanged.AddListener(HeatmapToggleChanged);

        m_previousTimelineButton.onClick.AddListener(HandlePreviousTimeline);
        m_nextTimelineButton.onClick.AddListener(HandleNextTimeline);
    }

    public override void Close(string closeResult)
    {
        base.Close();

        m_locationPinsToggle.onValueChanged.RemoveListener(LocationPinsToggleChanged);
        m_heatmapToggle.onValueChanged.RemoveListener(HeatmapToggleChanged);

        m_previousTimelineButton.onClick.RemoveListener(HandlePreviousTimeline);
        m_nextTimelineButton.onClick.RemoveListener(HandleNextTimeline);
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

    private void HandlePreviousTimeline()
    {
        m_currentLitterTimelineIndex--;
        if (m_currentLitterTimelineIndex < 0)
        {
            m_currentLitterTimelineIndex = TIMELINE_NAMES.Length - 1;
        }

        m_litterTimelineText.text = TIMELINE_NAMES[m_currentLitterTimelineIndex];
        PlayerPrefs.SetInt(PrefsKeys.LITTER_TIMELINE_KEY, m_currentLitterTimelineIndex);
        OnLitterTimelineChanged(TIMELINE_HOURS[m_currentLitterTimelineIndex]);
    }

    private void HandleNextTimeline()
    {
        m_currentLitterTimelineIndex++;
        if (m_currentLitterTimelineIndex >= TIMELINE_NAMES.Length)
        {
            m_currentLitterTimelineIndex = 0;
        }

        m_litterTimelineText.text = TIMELINE_NAMES[m_currentLitterTimelineIndex];
        PlayerPrefs.SetInt(PrefsKeys.LITTER_TIMELINE_KEY, m_currentLitterTimelineIndex);
        OnLitterTimelineChanged(TIMELINE_HOURS[m_currentLitterTimelineIndex]);
    }
}
