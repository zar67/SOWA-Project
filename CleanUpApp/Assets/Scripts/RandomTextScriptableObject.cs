using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Utilities/RandomTextSelector")]
public class RandomTextScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct CategoryData
    {
        public string ID;
        public List<string> Texts;
    }

    public const string DEFAULT_CATEGORY = "default";

    [SerializeField] private List<string> m_defaultTexts = new List<string>();
    [SerializeField] private List<CategoryData> m_categories = new List<CategoryData>();

    private Dictionary<string, List<string>> m_categoryTextsMap = new Dictionary<string, List<string>>();

    private List<string> m_defaultUnusedTexts = new List<string>();
    private Dictionary<string, List<string>> m_unusedTextsMap = new Dictionary<string, List<string>>();

    public string ChooseRandomText(string category = DEFAULT_CATEGORY)
    {
        CheckForEmptyUsedTexts();

        if (category == DEFAULT_CATEGORY || !m_categoryTextsMap.ContainsKey(category))
        {
            string defaultText = ChooseRandomText(m_defaultUnusedTexts);
            m_defaultUnusedTexts.Remove(defaultText);
            return defaultText;
        }

        string text = ChooseRandomText(m_unusedTextsMap[category]);
        m_unusedTextsMap[category].Remove(text);
        return text;
    }

    private string ChooseRandomText(List<string> texts)
    {
        if (texts.Count == 0)
        {
            CheckForEmptyUsedTexts();
        }

        if (texts.Count == 1)
        {
            return texts[0];
        }

        int index = Random.Range(0, texts.Count);
        string text = texts[index];

        return text;
    }

    private void Awake()
    {
        OnValidate();
        CheckForEmptyUsedTexts();
    }

    private void OnValidate()
    {
        m_categoryTextsMap = new Dictionary<string, List<string>>();
        foreach (CategoryData category in m_categories)
        {
            if (!m_categoryTextsMap.ContainsKey(category.ID))
            {
                m_categoryTextsMap.Add(category.ID, category.Texts);
            }

            if (!m_unusedTextsMap.ContainsKey(category.ID))
            {
                m_unusedTextsMap.Add(category.ID, category.Texts);
            }
        }
    }

    private void CheckForEmptyUsedTexts()
    {
        if (m_defaultUnusedTexts.Count == 0)
        {
            m_defaultUnusedTexts = new List<string>(m_defaultTexts);
        }

        foreach (KeyValuePair<string, List<string>> categoryMap in m_unusedTextsMap)
        {
            if (categoryMap.Value.Count == 0)
            {
                m_unusedTextsMap[categoryMap.Key] = new List<string>(m_categoryTextsMap[categoryMap.Key]);
            }
        }
    }
}
