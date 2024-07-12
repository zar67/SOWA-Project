using Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagManagementPopupData : BasePopupData
{
    public List<string> ActiveTags = new List<string>();
}

public class TagManagementPopup : BasePopup
{
    [Header("Category References")]
    [SerializeField] private ToggleGroup m_tabToggleGroup;
    [SerializeField] private Transform m_categoryTabsHolder;
    [SerializeField] private TagCategoryTab m_categoryTabPrefab;

    [Header("Tag References")]
    [SerializeField] private TagsData m_tagsData;
    [SerializeField] private Transform m_tagsHolder;
    [SerializeField] private TagObject m_tagPrefab;

    private List<string> m_activeTags = new List<string>();

    public override PopupType Type => PopupType.TAG_MANAGEMENT;

    public override void Init(BasePopupData data)
    {
        base.Init(data);

        var tagData = (TagManagementPopupData)data;

        m_activeTags = tagData.ActiveTags;

        string firstCategory = string.Empty;

        m_categoryTabsHolder.DestroyChildren();
        foreach (TagsData.TagCategoryData category in m_tagsData.AllCategories)
        {
            TagCategoryTab newCategoryTab = Instantiate(m_categoryTabPrefab, m_categoryTabsHolder);
            newCategoryTab.Populate(category.ID, m_tabToggleGroup, string.IsNullOrWhiteSpace(category.ID));

            if (string.IsNullOrWhiteSpace(firstCategory))
            {
                firstCategory = category.ID;
            }
        }
    }

    public override void Open()
    {
        base.Open();

        TagObject.AddTagClicked += HandleTagAdded;
        TagObject.RemoveTagClicked += HandleTagRemoved;

        TagCategoryTab.OnTabSelected += PopulateWithCategory;
    }

    public override void Close(string closeResult = "close")
    {
        base.Close(closeResult);

        TagObject.AddTagClicked -= HandleTagAdded;
        TagObject.RemoveTagClicked -= HandleTagRemoved;

        TagCategoryTab.OnTabSelected -= PopulateWithCategory;
    }

    private void PopulateWithCategory(string id)
    {
        m_tagsHolder.DestroyChildren();

        TagsData.TagCategoryData category = m_tagsData.GetTagCategoryForID(id);
        foreach (TagsData.TagData tag in category.TagData)
        {
            TagObject newTag = Instantiate(m_tagPrefab, m_tagsHolder);
            newTag.PopulateTag(tag);
            newTag.SetActive(m_activeTags.Contains(tag.ID));
        }
    }

    private void HandleTagAdded(string tag)
    {
        if (!m_activeTags.Contains(tag))
        {
            m_activeTags.Add(tag);
        }
    }

    private void HandleTagRemoved(string tag)
    {
        if (m_activeTags.Contains(tag))
        {
            m_activeTags.Remove(tag);
        }
    }
}
