using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScreen : MonoBehaviour
{
    [SerializeField] private Button m_recordLitterButton;

    private void OnEnable()
    {
        m_recordLitterButton.onClick.AddListener(OpenRecordingPanel);
    }

    private void OnDisable()
    {
        m_recordLitterButton.onClick.RemoveListener(OpenRecordingPanel);
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
