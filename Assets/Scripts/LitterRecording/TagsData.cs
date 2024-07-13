using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CleanUp/TagsData")]
public class TagsData : ScriptableObject
{
    [System.Serializable]
    public struct TagCategoryData
    {
        public string ID;
        public TagData[] TagData;
    }

    [System.Serializable]
    public struct TagData
    {
        public string ID;
        public string RecycleNumber;
        public string RecycleAbbreviation;
    }

    [SerializeField] private List<TagCategoryData> m_tagCategories = new List<TagCategoryData>();

    private Dictionary<string, TagCategoryData> m_categoryMap = new Dictionary<string, TagCategoryData>();
    private Dictionary<string, TagData> m_tagsMap = new Dictionary<string, TagData>();

    public IEnumerable<TagCategoryData> AllCategories => m_categoryMap.Values;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        m_categoryMap = new Dictionary<string, TagCategoryData>();
        m_tagsMap = new Dictionary<string, TagData>();

        foreach (TagCategoryData category in m_tagCategories)
        {
            if (!m_categoryMap.ContainsKey(category.ID))
            {
                m_categoryMap.Add(category.ID, category);
            }

            foreach (TagData tag in category.TagData)
            {
                if (!m_tagsMap.ContainsKey(tag.ID))
                {
                    m_tagsMap.Add(tag.ID, tag);
                }
            }
        }
    }

    public TagCategoryData GetTagCategoryForID(string id)
    {
        return m_categoryMap[id];
    }

    public TagCategoryData GetTagCategoryForTagID(string id)
    {
        foreach (TagCategoryData category in m_tagCategories)
        {
            foreach (TagData tag in category.TagData)
            {
                if (tag.ID == id)
                {
                    return category;
                }
            }
        }

        return default;
    }

    public TagData GetDataForTagID(string id)
    {
        return m_tagsMap[id];
    }
}
