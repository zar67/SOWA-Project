using Extensions;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsPopup : BasePopup
{
    [Header("Stat References")]
    [SerializeField] private Transform m_statsHolder;
    [SerializeField] private StatObject m_statObjectPrefab;

    public override PopupType Type => PopupType.STATISTICS;

    public override void Open()
    {
        base.Open();

        m_statsHolder.gameObject.DestroyChildren();

        StatObject totalStatObject = Instantiate(m_statObjectPrefab);
        totalStatObject.transform.SetParent(m_statsHolder);
        totalStatObject.transform.localScale = Vector3.one;
        totalStatObject.Populate("Total", StatisticRecordingManager.Instance.UserStatistics.TotalRecordedLitter);

        foreach (KeyValuePair<string, long> stat in StatisticRecordingManager.Instance.UserStatistics.RecordedLitterByTag)
        {
            StatObject newStatObject = Instantiate(m_statObjectPrefab);
            newStatObject.transform.SetParent(m_statsHolder);
            newStatObject.transform.localScale = Vector3.one;
            newStatObject.Populate(stat.Key, stat.Value);
        }
    }
}
