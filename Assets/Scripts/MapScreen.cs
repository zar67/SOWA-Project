using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScreen : MonoBehaviour
{
    [SerializeField] private Button m_recordLitterButton;
    [SerializeField] private RecordingPanelUI m_recordingPanel;

    private void OnEnable()
    {
        m_recordLitterButton.onClick.AddListener(OpenRecordingPanel);

        m_recordingPanel.ClosePanel();
    }

    private void OnDisable()
    {
        m_recordLitterButton.onClick.RemoveListener(OpenRecordingPanel);
    }

    private void OpenRecordingPanel()
    {
        m_recordingPanel.OpenPanel();
    }
}
