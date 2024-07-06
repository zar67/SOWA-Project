using Firebase.Auth;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticRecordingManager : SingletonMonoBehaviour<StatisticRecordingManager>
{
    public const string STATISTICS_KEY = "STATISTICS_DATA";

    public UserStatistics UserStatistics { get; private set; }

    private string m_userStatisticsKey;

    private void OnEnable()
    {
        m_userStatisticsKey = STATISTICS_KEY + "/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        FirebaseDatabaseManager.Instance.AddValueChangedListener(m_userStatisticsKey, HandleStatisticsDatabaseUpdated);
    }

    private void OnDisable()
    {
        FirebaseDatabaseManager.Instance.RemoveValueChangedListener(m_userStatisticsKey, HandleStatisticsDatabaseUpdated);
    }

    public void RecordStatistics(string[] tags)
    {
        foreach (string tag in tags)
        {
            if (!UserStatistics.RecordedLitterByTag.ContainsKey(tag))
            {
                UserStatistics.RecordedLitterByTag.Add(tag, 0);
            }

            UserStatistics.RecordedLitterByTag[tag]++;
        }

        FirebaseDatabaseManager.Instance.SaveData(m_userStatisticsKey, UserStatistics);
    }

    private void HandleStatisticsDatabaseUpdated(object data, Firebase.Database.ValueChangedEventArgs args)
    {
        if (args.Snapshot.Value == null)
        {
            UserStatistics = new UserStatistics();
            return;
        }

        UserStatistics = JsonConvert.DeserializeObject<UserStatistics>(args.Snapshot.Value as string);

        UserStatistics.TotalRecordedLitter = 0;
        foreach (long tagAmount in UserStatistics.RecordedLitterByTag.Values)
        {
            UserStatistics.TotalRecordedLitter += tagAmount;
        }
    }
}
