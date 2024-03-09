using Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LitterRecordingPopup : BasePopup
{
    [Header("Add Button References")]
    [SerializeField] private Button m_addButton;

    [Header("Location References")]
    [SerializeField] private Button m_changeLocationButton;

    [Header("Tags References")]
    [SerializeField] private TMP_InputField m_tagInputField;
    [SerializeField] private TagObject m_tagPrefab;
    [SerializeField] private LayoutGroup[] m_tagsLayoutGroups;
    [SerializeField] private Transform m_tagsHolder;

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
        m_tagInputField.onSubmit.AddListener(HandleTagAdded);

        TagObject.RemoveTagClicked += HandleRemoveTagClicked;
    }

    private void OnDisable()
    {
        m_addButton.onClick.RemoveListener(RecordLitterAndClose);
        m_changeLocationButton.onClick.RemoveListener(ChangeLocation);
        m_tagInputField.onSubmit.RemoveListener(HandleTagAdded);

        TagObject.RemoveTagClicked -= HandleRemoveTagClicked;
    }

    private void RecordLitterAndClose()
    {
        LitterRecordingManager.Instance.RecordLitter();
        Close();
    }

    private void ChangeLocation()
    {
        // For future, add the ability to input a specific location.
    }

    private void HandleTagAdded(string tag)
    {
        TagObject newTag = Instantiate(m_tagPrefab, m_tagsHolder);
        newTag.PopulateTag(tag);

        m_tagInputField.text = string.Empty;

        m_currentTags.Add(tag);

        StartCoroutine(RefreshTagsLayout());
    }

    private void HandleRemoveTagClicked(string tag)
    {
        m_currentTags.Remove(tag);

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
