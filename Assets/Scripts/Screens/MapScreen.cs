using UnityEngine;
using UnityEngine.UI;

public class MapScreen : MonoBehaviour
{
    [SerializeField] private Button m_settingsButton;
    [SerializeField] private Button m_recordLitterButton;
    [SerializeField] private Button m_statisticsButton;

    [SerializeField] private RandomTextScriptableObject m_randomRecyclingInfoTextSelector;

    private void OnEnable()
    {
        m_settingsButton.onClick.AddListener(OpenSettingsPanel);
        m_recordLitterButton.onClick.AddListener(OpenRecordingPanel);
        m_statisticsButton.onClick.AddListener(OpenStatisticsPanel);
    }

    private void OnDisable()
    {
        m_settingsButton.onClick.RemoveListener(OpenSettingsPanel);
        m_recordLitterButton.onClick.RemoveListener(OpenRecordingPanel);
        m_statisticsButton.onClick.RemoveListener(OpenStatisticsPanel);
    }

    private void OpenSettingsPanel()
    {
        PopupManager.Instance.OpenPopup(new BasePopupData()
        {
            Type = PopupType.SETTINGS,
            ShowCloseButton = true
        });
    }

    private void OpenRecordingPanel()
    {
        PopupManager.Instance.OpenPopup(new BasePopupData()
        {
            Type = PopupType.LITTER_RECORDING,
            ShowCloseButton = true,
            OnCloseStarted = HandleCloseRecordingPopup
        });
    }

    private void OpenStatisticsPanel()
    {
        PopupManager.Instance.OpenPopup(new BasePopupData()
        {
            Type = PopupType.STATISTICS,
            ShowCloseButton = true,
        });
    }
    private void HandleCloseRecordingPopup()
    {
        PopupManager.Instance.OpenPopup(new GenericInfoPopupData()
        {
            Type = PopupType.GENERIC_INFO,
            ShowCloseButton = false,
            BodyText = $"Thank you! Your litter has been recorded.\n\nTIP: {m_randomRecyclingInfoTextSelector.ChooseRandomText()}",
            ButtonDatas = new PopupButtonData[]
            {
                new PopupButtonData()
                {
                    Text = "Continue",
                    CloseOnClick = true
                }
            }
        });
    }
}
