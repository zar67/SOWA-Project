using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using UnityEngine;

public class FirebaseManager : BootstrapperDependancy
{
    private void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // When this property is set to true, Crashlytics will report all
                // uncaught exceptions as fatal events. This is the recommended behavior.
                Crashlytics.ReportUncaughtExceptionsAsFatal = true;

                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                SetComplete();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }
}