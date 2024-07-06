using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Utilities/RandomTextSelector")]
public class RandomTextScriptableObject : ScriptableObject
{
    [SerializeField] private List<string> m_texts = new List<string>();

    private List<string> m_remainingTexts = new List<string>();

    public string ChooseRandomText()
    {
        if (m_remainingTexts.Count == 0)
        {
            ResetUsedTexts();
        }

        if (m_remainingTexts.Count == 1)
        {
            return SelectTextWithIndex(0);
        }

        int index = Random.Range(0, m_remainingTexts.Count);
        return SelectTextWithIndex(index);
    }

    private void Awake()
    {
        ResetUsedTexts();
    }

    private void ResetUsedTexts()
    {
        m_remainingTexts = new List<string>(m_texts);
    }

    private string SelectTextWithIndex(int index)
    {
        string text = m_remainingTexts[index];

        m_remainingTexts.RemoveAt(index);

        return text;
    }
}
