using System;
using System.Collections;
using UnityEngine;

public class FTUEManager : SingletonMonoBehaviour<FTUEManager>
{
    private const string FTUE_PLAYER_PREFS_KEY = "LastFTUEStage";
    public const string FTUE_COMPLETED_VALUE = "Completed";

    [SerializeField] private FTUEStagesData m_stagesData;

    private BasePopup m_currentOpenPopup = null;

    public Action<string> OnSetFTUEHighlight;

    public void BeginFTUE()
    {
        string currentStageID = PlayerPrefs.GetString(FTUE_PLAYER_PREFS_KEY, m_stagesData.GetStageAtIndex(0).ID);

        if (currentStageID != FTUE_COMPLETED_VALUE)
        {
            LoadFTUEStage(currentStageID);
        }
    }

    private void LoadFTUEStage(string id)
    {
        FTUEStagesData.FTUEStage ftueStage = m_stagesData.GetStageForID(id);

        if (ftueStage.PopupType != PopupType.NONE && !PopupManager.Instance.HasOpenPopup(ftueStage.PopupType))
        {
            m_currentOpenPopup = PopupManager.Instance.OpenPopup(new BasePopupData()
            {
                Type = ftueStage.PopupType
            });
        }

        PopupManager.Instance.OpenPopup(new FTUEPopupData()
        {
            Type = PopupType.FTUE_INFO,
            ShowCloseButton = true,
            CloseResultActions = new System.Collections.Generic.Dictionary<string, Action>()
            {
                { BasePopup.CLOSE_RESULT_CLOSE_BUTTON, () => CompleteFTUEStage(id)}
            },
            FTUEText = ftueStage.Text,
            CurrentStageNumber = m_stagesData.GetIndexOfStage(ftueStage.ID) + 1,
            TotalStagesNumber = m_stagesData.GetTotalStagesCount()
        });

        StartCoroutine(UpdateHighlightComponents(id));
    }

    private void CompleteFTUEStage(string id)
    {
        FTUEStagesData.FTUEStage currentFTUEStage = m_stagesData.GetStageForID(id);
        FTUEStagesData.FTUEStage nextFTUEStage = m_stagesData.GetNextStage(id, out bool hasNextStage);

        if (m_currentOpenPopup != null && (nextFTUEStage.PopupType != currentFTUEStage.PopupType || !hasNextStage))
        {
            m_currentOpenPopup.Close();
            m_currentOpenPopup = null;
        }

        if (hasNextStage)
        {
            PlayerPrefs.SetString(FTUE_PLAYER_PREFS_KEY, nextFTUEStage.ID);
            LoadFTUEStage(nextFTUEStage.ID);
        }
        else
        {
            PlayerPrefs.SetString(FTUE_PLAYER_PREFS_KEY, FTUE_COMPLETED_VALUE);
        }
    }

    private IEnumerator UpdateHighlightComponents(string id)
    {
        // Wait for frame to enable any highlight components in opening popups
        yield return new WaitForEndOfFrame();

        OnSetFTUEHighlight?.Invoke(id);
    }
}
