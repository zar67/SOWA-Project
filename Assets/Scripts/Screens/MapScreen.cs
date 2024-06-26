using UnityEngine;
using UnityEngine.UI;

public class MapScreen : MonoBehaviour
{
    [SerializeField] private Button m_settingsButton;
    [SerializeField] private Button m_recordLitterButton;

    private void OnEnable()
    {
        m_settingsButton.onClick.AddListener(OpenSettingsPanel);
        m_recordLitterButton.onClick.AddListener(OpenRecordingPanel);
    }

    private void OnDisable()
    {
        m_settingsButton.onClick.RemoveListener(OpenSettingsPanel);
        m_recordLitterButton.onClick.RemoveListener(OpenRecordingPanel);
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
            ShowCloseButton = true
        });
    }
}
