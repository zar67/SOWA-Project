using Firebase;
using Firebase.Auth;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Firebase.Extensions;
using UnityEngine;
using System.Threading.Tasks;

public class FirebaseManager : BootstrapperDependancy
{
    private void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(HandleFirebaseDependenciesChecked);
    }

    private async void HandleFirebaseDependenciesChecked(Task<DependencyStatus> task)
    {
        DependencyStatus dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;

            // When this property is set to true, Crashlytics will report all
            // uncaught exceptions as fatal events. This is the recommended behavior.
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;

            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            if (FirebaseAuth.DefaultInstance.CurrentUser == null)
            {
                await FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync();
                Debug.Log("Firebase new user created.");
            }
            else
            {
                Debug.Log("Firebase existing user logged in.");
            }

            SetComplete();

            Debug.Log("Firebase initialized.");
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }
    }
}