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

    [SerializeField] private TagObject m_tagPrefab;
    [SerializeField] private LayoutGroup[] m_tagsLayoutGroups;
    [SerializeField] private Transform m_tagsHolder;

    public bool IsShowing => m_toolTipHolder.activeInHierarchy;

    private void Awake()
    {
        m_toolTipHolder.SetActive(false);
        m_tagsHolder.DestroyChildren();
    }

    private void OnEnable()
    {
        m_button.onClick.AddListener(HandleButtonClicked);
        OnLitterButtonClicked += HandleLitterObjectClicked;
    }

    private void OnDisable()
    {
        m_button.onClick.RemoveListener(HandleButtonClicked);
        OnLitterButtonClicked += HandleLitterObjectClicked;
    }

    public void SetData(LitterData data)
    {
        m_timestampText.text = data.Timestamp.ToString();
        m_text.text = data.MergedAmount.ToString();

        m_tagsHolder.DestroyChildren();
        foreach (string tag in data.Tags)
        {
            TagObject newTag = Instantiate(m_tagPrefab, m_tagsHolder);
            newTag.PopulateTag(tag);
        }

        StartCoroutine(RefreshTagsLayout());
    }

    private void HandleButtonClicked()
    {
        OnLitterButtonClicked?.Invoke(this);
    }

    private void HandleLitterObjectClicked(LitterObject obj)
    {
        if (obj == this && obj.IsShowing)
        {
            m_toolTipHolder.SetActive(false);
            return;
        }

        m_toolTipHolder.SetActive(obj == this);
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
