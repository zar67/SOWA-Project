using System;
using TMPro;
using UnityEngine;

[Serializable]
public class FTUEPopupData : BasePopupData
{
    public string FTUEText;
    public int CurrentStageNumber;
    public int TotalStagesNumber;
}

public class FTUEInfoPopup : BasePopup
{
    public override PopupType Type => PopupType.FTUE_INFO;

    [Header("Text References")]
    [SerializeField] protected TextMeshProUGUI m_ftueText;
    [SerializeField] protected TextMeshProUGUI m_stageCountText;

    public override void Init(BasePopupData data)
    {
        base.Init(data);

        var ftueData = (FTUEPopupData)data;
        m_ftueText.text = ftueData.FTUEText;

        m_stageCountText.text = $"{ftueData.CurrentStageNumber}/{ftueData.TotalStagesNumber}";
    }
}
