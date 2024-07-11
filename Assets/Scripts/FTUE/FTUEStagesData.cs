using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CleanUp/FTUEStagesData")]
public class FTUEStagesData : ScriptableObject
{
    [System.Serializable]
    public struct FTUEStage
    {
        public string ID;
        public PopupType PopupType;
        public string Text;
    }

    [SerializeField] private List<FTUEStage> m_stages = new List<FTUEStage>();

    private Dictionary<string, FTUEStage> m_stagesMap = new Dictionary<string, FTUEStage>();

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        m_stagesMap = new Dictionary<string, FTUEStage>();
        foreach (FTUEStage stage in m_stages)
        {
            if (!m_stagesMap.ContainsKey(stage.ID))
            {
                m_stagesMap.Add(stage.ID, stage);
            }
        }
    }

    public FTUEStage GetStageForID(string id)
    {
        return m_stagesMap[id];
    }

    public FTUEStage GetStageAtIndex(int index)
    {
        return m_stages[index];
    }

    public FTUEStage GetNextStage(string currentStageID, out bool hasNextStage)
    {
        hasNextStage = false;

        FTUEStage currentStage = GetStageForID(currentStageID);
        int currentStageIndex = m_stages.IndexOf(currentStage);
        int nextStageIndex = currentStageIndex + 1;

        if (nextStageIndex >= m_stages.Count)
        {
            return default;
        }

        hasNextStage = true;
        return m_stages[nextStageIndex];
    }
}
