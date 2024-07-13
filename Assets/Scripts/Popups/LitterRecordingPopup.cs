using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LitterRecordingPopup : BasePopup
{
    public const string CLOSE_RESULT_ADD_LITTER = "add_litter";

    [SerializeField] private RandomTextScriptableObject m_randomRecyclingInfoTextSelector;

    [Header("Content References")]
    [SerializeField] private RectTransform m_content;

    [Header("Add Button References")]
    [SerializeField] private Button m_addButton;

    [Header("Tags References")]
    [SerializeField] private TagsData m_tagsData;
    [SerializeField] private Button m_manageTagsButton;
    [SerializeField] private TextMeshProUGUI m_totalTagsText;

    private List<string> m_currentTags = new List<string>();

    public override PopupType Type => PopupType.LITTER_RECORDING;

    private void OnEnable()
    {
        m_addButton.onClick.AddListener(RecordLitterAndClose);
        m_manageTagsButton.onClick.AddListener(OpenTagManagementPopup);

        TagObject.AddTagClicked += HandleTagAdded;
        TagObject.RemoveTagClicked += HandleTagRemoved;

        m_totalTagsText.text = $"0 Selected";
    }

    private void OnDisable()
    {
        m_addButton.onClick.RemoveListener(RecordLitterAndClose);
        m_manageTagsButton.onClick.RemoveListener(OpenTagManagementPopup);

        TagObject.AddTagClicked -= HandleTagAdded;
        TagObject.RemoveTagClicked -= HandleTagRemoved;
    }

    private void RecordLitterAndClose()
    {
        LitterRecordingManager.Instance.RecordLitter(m_currentTags.ToArray());
        StatisticRecordingManager.Instance.RecordStatistics(m_currentTags.ToArray());
        Close(CLOSE_RESULT_ADD_LITTER);

        string recyclingInfoCategory = RandomTextScriptableObject.DEFAULT_CATEGORY;
        if (m_currentTags.Count >= 0)
        {
            string randomTag = m_currentTags[Random.Range(0, m_currentTags.Count)];
            recyclingInfoCategory = m_tagsData.GetTagCategoryForTagID(randomTag).ID;
        }

        m_currentTags = new List<string>();

        PopupManager.Instance.OpenPopup(new GenericInfoPopupData()
        {
            Type = PopupType.GENERIC_INFO,
            ShowCloseButton = false,
            BodyText = $"Thank you! Your litter has been recorded.\n\nTIP: {m_randomRecyclingInfoTextSelector.ChooseRandomText(recyclingInfoCategory)}",
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

    private void OpenTagManagementPopup()
    {
        PopupManager.Instance.OpenPopup(new TagManagementPopupData()
        {
            ShowCloseButton = true,
            ActiveTags = m_currentTags,
            Type = PopupType.TAG_MANAGEMENT
        });
    }

    private void HandleTagAdded(string tag)
    {
        if (!m_currentTags.Contains(tag))
        {
            m_currentTags.Add(tag);
            m_totalTagsText.text = $"{m_currentTags.Count} Selected";
        }
    }

    private void HandleTagRemoved(string tag)
    {
        if (m_currentTags.Contains(tag))
        {
            m_currentTags.Remove(tag);
            m_totalTagsText.text = $"{m_currentTags.Count} Selected";
        }
    }
}
