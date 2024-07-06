using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatObject : MonoBehaviour
{
    [SerializeField] private Image m_crownImage;
    [SerializeField] private GameObject m_nextLevelHolder;
    [SerializeField] private Image m_nextLevelCrownImage;
    [SerializeField] private Image m_nextLevelProgressImage;
    [SerializeField] private TextMeshProUGUI m_amountText;
    [SerializeField] private TextMeshProUGUI m_typeText;

    [SerializeField] private StatisticLevelsScriptableObject m_statisticLevels;

    public void Populate(string type, long amount)
    {
        m_typeText.text = type;
        m_amountText.text = amount.ToString();

        StatisticLevelsScriptableObject.StatisticLevel currentLevel = m_statisticLevels.GetCurrentLevelData(amount);
        StatisticLevelsScriptableObject.StatisticLevel nextLevel = m_statisticLevels.GetNextLevelData(amount);

        m_crownImage.color = currentLevel.Color;

        m_nextLevelHolder.SetActive(currentLevel != m_statisticLevels.GetLastLevel());

        m_nextLevelCrownImage.color = nextLevel.Color;
        float progress = (float)(amount - currentLevel.StatisticAmount) / nextLevel.StatisticAmount;
        m_nextLevelProgressImage.fillAmount = progress;
    }
}
