using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LitterObject : MonoBehaviour
{
    public static event Action<LitterObject> OnLitterButtonClicked;

    [SerializeField] private Button m_button;
    [SerializeField] private TextMeshProUGUI m_text;

    [SerializeField] private GameObject m_toolTipHolder;
    [SerializeField] private TextMeshProUGUI m_timestampText;

    [SerializeField] private TagsData m_tagsData;
    [SerializeField] private TagObject m_tagPrefab;
    [SerializeField] private LayoutGroup[] m_tagsLayoutGroups;
    [SerializeField] private Transform m_tagsHolder;

    [SerializeField] private Button m_leftButton;
    [SerializeField] private Button m_rightButton;

    private List<LitterData> m_data;
    private int m_currentDisplayIndex = 0;

    public static void ClearToolTip()
    {
        OnLitterButtonClicked?.Invoke(null);
    }

    private void Awake()
    {
        m_toolTipHolder.SetActive(false);
        m_tagsHolder.DestroyChildren();
    }

    private void OnEnable()
    {
        m_button.onClick.AddListener(HandleButtonClicked);

        m_leftButton.onClick.AddListener(HandleLeftClicked);
        m_rightButton.onClick.AddListener(HandleRightClicked);

        OnLitterButtonClicked += HandleLitterObjectClicked;
    }

    private void OnDisable()
    {
        m_button.onClick.RemoveListener(HandleButtonClicked);

        m_leftButton.onClick.RemoveListener(HandleLeftClicked);
        m_rightButton.onClick.RemoveListener(HandleRightClicked);

        OnLitterButtonClicked -= HandleLitterObjectClicked;
    }

    public void SetData(List<LitterData> data)
    {
        m_data = data;
        m_text.text = data.Count.ToString();

        m_leftButton.gameObject.SetActive(data.Count > 1);
        m_rightButton.gameObject.SetActive(data.Count > 1);
    }

    private void UpdateDisplay(LitterData litter)
    {
        m_timestampText.text = litter.Timestamp.ToString();

        m_tagsHolder.DestroyChildren();
        foreach (string tag in litter.Tags)
        {
            TagObject newTag = Instantiate(m_tagPrefab, m_tagsHolder);
            newTag.PopulateTag(m_tagsData.GetDataForTagID(tag));
        }

        RefreshTagsLayout();
    }

    private void HandleButtonClicked()
    {
        OnLitterButtonClicked?.Invoke(this);
    }

    private void HandleLeftClicked()
    {
        m_currentDisplayIndex++;
        if (m_currentDisplayIndex >= m_data.Count)
        {
            m_currentDisplayIndex = 0;
        }

        UpdateDisplay(m_data[m_currentDisplayIndex]);
    }

    private void HandleRightClicked()
    {
        m_currentDisplayIndex--;
        if (m_currentDisplayIndex < 0)
        {
            m_currentDisplayIndex = m_data.Count - 1;
        }

        UpdateDisplay(m_data[m_currentDisplayIndex]);
    }

    private void HandleLitterObjectClicked(LitterObject obj)
    {
        m_toolTipHolder.SetActive(obj == this);
        if (obj == this)
        {
            m_currentDisplayIndex = 0;
            UpdateDisplay(m_data[m_currentDisplayIndex]);
        }
    }

    private IEnumerator RefreshTagsLayout()
    {
        foreach (LayoutGroup layoutGroup in m_tagsLayoutGroups)
        {
            layoutGroup.enabled = false;
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        foreach (LayoutGroup layoutGroup in m_tagsLayoutGroups)
        {
            layoutGroup.enabled = true;
        }
    }
}
