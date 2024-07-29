using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CleanUp/Statistic Levels")]
public class StatisticLevelsScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class StatisticLevel
    {
        public long StatisticAmount;
        public Color Color;
    }

    [SerializeField] private List<StatisticLevel> m_statisticLevels = new List<StatisticLevel>();

    public StatisticLevel GetCurrentLevelData(long amount)
    {
        StatisticLevel highestLevel = m_statisticLevels[0];
        foreach (StatisticLevel level in m_statisticLevels)
        {
            if (amount >= level.StatisticAmount)
            {
                highestLevel = level;
            }

            if (amount < level.StatisticAmount)
            {
                break;
            }
        }

        return highestLevel;
    }

    public StatisticLevel GetNextLevelData(long amount)
    {
        StatisticLevel currentLevel = GetCurrentLevelData(amount);
        int currentLevelIndex = m_statisticLevels.IndexOf(currentLevel);

        if (currentLevelIndex < m_statisticLevels.Count - 1)
        {
            return m_statisticLevels[currentLevelIndex + 1];
        }

        return GetLastLevel();
    }

    public StatisticLevel GetLastLevel()
    {
        return m_statisticLevels[^1];
    }
}
