using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LitterRecordingPopup : BasePopup
{
    [Header("Content References")]
    [SerializeField] private RectTransform m_content;

    [Header("Add Button References")]
    [SerializeField] private Button m_addButton;

    [Header("Location References")]
    [SerializeField] private Button m_changeLocationButton;

    [Header("Tags References")]
    [SerializeField] private TagsData m_tagsData;
    [SerializeField] private SearchableDropDown m_searchableInputField;
    [SerializeField] private TagObject m_tagPrefab;
    [SerializeField] private LayoutGroup[] m_tagsLayoutGroups;
    [SerializeField] private Transform m_tagsHolder;

    private List<string> m_availableTags = new List<string>();
    private List<string> m_currentTags = new List<string>();

    public override PopupType Type => PopupType.LITTER_RECORDING;

    private void Awake()
    {
        m_tagsHolder.DestroyChildren();
    }

    private void OnEnable()
    {
        m_addButton.onClick.AddListener(RecordLitterAndClose);
        m_changeLocationButton.onClick.AddListener(ChangeLocation);

        m_searchableInputField.OnSelect += HandleInputFieldSelected;
        m_searchableInputField.OnDeselect += HandleInputFieldDeselected;
        m_searchableInputField.OnValueChanged += HandleTagAdded;

        TagObject.RemoveTagClicked += HandleRemoveTagClicked;

        m_availableTags = new List<string>(m_tagsData.Tags);
        m_availableTags.Sort();
        m_currentTags = new List<string>();

        m_tagsHolder.DestroyChildren();
        m_searchableInputField.Clear();
        m_searchableInputField.SetOptions(m_availableTags.ToArray());
    }

    private void OnDisable()
    {
        m_addButton.onClick.RemoveListener(RecordLitterAndClose);
        m_changeLocationButton.onClick.RemoveListener(ChangeLocation);

        m_searchableInputField.OnSelect -= HandleInputFieldSelected;
        m_searchableInputField.OnDeselect -= HandleInputFieldDeselected;
        m_searchableInputField.OnValueChanged -= HandleTagAdded;

        TagObject.RemoveTagClicked -= HandleRemoveTagClicked;
    }

    private void RecordLitterAndClose()
    {
        LitterRecordingManager.Instance.RecordLitter(m_currentTags.ToArray());
        Close();
    }

    private void ChangeLocation()
    {
        // For future, add the ability to input a specific location.
    }

    private void HandleInputFieldSelected()
    {
        var position = m_content.localPosition;
        position.y += m_content.rect.height / 2;
        m_content.localPosition = position;
    }

    private void HandleInputFieldDeselected()
    {
        m_content.localPosition = Vector2.zero;
    }

    private void HandleTagAdded(string tag)
    {
        HandleInputFieldDeselected();

        TagObject newTag = Instantiate(m_tagPrefab, m_tagsHolder);
        newTag.PopulateTag(tag);

        if (!m_currentTags.Contains(tag))
        {
            m_currentTags.Add(tag);
        }

        m_availableTags.Remove(tag);
        m_searchableInputField.SetOptions(m_availableTags.ToArray());

        StartCoroutine(RefreshTagsLayout());
    }

    private void HandleRemoveTagClicked(string tag)
    {
        m_currentTags.Remove(tag);

        if (!m_availableTags.Contains(tag))
        {
            m_availableTags.Add(tag);
            m_availableTags.Sort();
            m_searchableInputField.SetOptions(m_availableTags.ToArray());
        }

        StartCoroutine(RefreshTagsLayout());
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
