using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LitterObjectData : BasePopupData
{
    public List<LitterData> Data;
}

public class LitterObjectPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI m_timestampText;
    [SerializeField] private GameObject m_noTagsHolder;

    [SerializeField] private TagsData m_tagsData;
    [SerializeField] private TagObject m_tagPrefab;
    [SerializeField] private LayoutGroup[] m_tagsLayoutGroups;
    [SerializeField] private Transform m_tagsHolder;

    [SerializeField] private Button m_leftButton;
    [SerializeField] private Button m_rightButton;

    private List<LitterData> m_data;
    private int m_currentDisplayIndex = 0;

    public override PopupType Type => PopupType.LITTER_OBJECT;

    private void Awake()
    {
        m_tagsHolder.DestroyChildren();
    }

    private void OnEnable()
    {
        m_leftButton.onClick.AddListener(HandleLeftClicked);
        m_rightButton.onClick.AddListener(HandleRightClicked);
    }

    private void OnDisable()
    {
        m_leftButton.onClick.RemoveListener(HandleLeftClicked);
        m_rightButton.onClick.RemoveListener(HandleRightClicked);
    }

    private void UpdateDisplay(LitterData litter)
    {
        string timeText = string.Empty;
        if (DateTime.TryParseExact(litter.Timestamp, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var time))
        {
            timeText = time.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        }

        m_timestampText.text = timeText;

        m_tagsHolder.DestroyChildren();
        foreach (string tag in litter.Tags)
        {
            TagObject newTag = Instantiate(m_tagPrefab, m_tagsHolder);
            newTag.PopulateTag(m_tagsData.GetDataForTagID(tag));
        }

        m_noTagsHolder.SetActive(litter.Tags.Length == 0);

        m_leftButton.interactable = m_currentDisplayIndex > 0;
        m_rightButton.interactable = m_currentDisplayIndex < m_data.Count - 1;
    }

    private void HandleRightClicked()
    {
        m_currentDisplayIndex++;
        if (m_currentDisplayIndex >= m_data.Count)
        {
            m_currentDisplayIndex = m_data.Count - 1;
        }

        UpdateDisplay(m_data[m_currentDisplayIndex]);
    }

    private void HandleLeftClicked()
    {
        m_currentDisplayIndex--;
        if (m_currentDisplayIndex < 0)
        {
            m_currentDisplayIndex = 0;
        }

        UpdateDisplay(m_data[m_currentDisplayIndex]);
    }

    public override void Init(BasePopupData data)
    {
        base.Init(data);

        var litterObjData = (LitterObjectData)data;

        m_data = litterObjData.Data;

        m_leftButton.gameObject.SetActive(m_data.Count > 1);
        m_rightButton.gameObject.SetActive(m_data.Count > 1);

        m_currentDisplayIndex = 0;
        UpdateDisplay(m_data[m_currentDisplayIndex]);
    }
}
