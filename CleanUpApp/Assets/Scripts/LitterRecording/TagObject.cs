using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TagsData;

public class TagObject : MonoBehaviour
{
    public static event Action<string> AddTagClicked;
    public static event Action<string> RemoveTagClicked;

    [SerializeField] private TextMeshProUGUI m_tagText;
    [SerializeField] private Toggle m_toggle;

    [Header("Icon References")]
    [SerializeField] private GameObject m_iconHolder;
    [SerializeField] private TextMeshProUGUI m_recycleNumberText;
    [SerializeField] private TextMeshProUGUI m_recycleAbbrehivationText;

    public void PopulateTag(TagData tag)
    {
        m_tagText.text = tag.ID;

        bool hasRecycleInfo = !string.IsNullOrWhiteSpace(tag.RecycleAbbreviation) || !string.IsNullOrWhiteSpace(tag.RecycleNumber);

        m_iconHolder.SetActive(hasRecycleInfo);
        if (hasRecycleInfo)
        {
            m_recycleNumberText.text = tag.RecycleNumber;
            m_recycleAbbrehivationText.text = tag.RecycleAbbreviation;
        }
    }

    public void SetActive(bool active)
    {
        m_toggle.SetIsOnWithoutNotify(active);
    }

    private void OnEnable()
    {
        m_toggle.onValueChanged.AddListener(HandleTagValueChanged);
    }

    private void OnDisable()
    {
        m_toggle.onValueChanged.RemoveListener(HandleTagValueChanged);
    }

    private void HandleTagValueChanged(bool active)
    {
        if (active)
        {
            AddTagClicked?.Invoke(m_tagText.text);
        }
        else
        {
            RemoveTagClicked?.Invoke(m_tagText.text);
        }
    }
}
