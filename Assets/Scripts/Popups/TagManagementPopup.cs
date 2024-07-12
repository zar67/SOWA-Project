using Extensions;
using System.Collections.Generic;
using UnityEngine;

public class TagManagementPopupData : BasePopupData
{
    public List<string> ActiveTags = new List<string>();
}

public class TagManagementPopup : BasePopup
{
    [SerializeField] private TagsData m_tagsData;
    [SerializeField] private Transform m_tagsHolder;
    [SerializeField] private TagObject m_tagPrefab;

    public override PopupType Type => PopupType.TAG_MANAGEMENT;

    public override void Init(BasePopupData data)
    {
        base.Init(data);

        var tagData = (TagManagementPopupData)data;

        m_tagsHolder.DestroyChildren();
        foreach (TagsData.TagCategoryData category in m_tagsData.AllCategories)
        {
            foreach (TagsData.TagData tag in category.TagData)
            {
                TagObject newTag = Instantiate(m_tagPrefab, m_tagsHolder);
                newTag.PopulateTag(tag);
                newTag.SetActive(tagData.ActiveTags.Contains(tag.ID));
            }
        }

    }
}
