using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class IntroScreen : MonoBehaviour
{
    [SerializeField] private string m_mapSceneName;

    private void Awake()
    {
#if UNITY_ANDROID
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            MoveToMap();
            return;
        }

        PopupManager.Instance.OpenPopup(new PopupData()
        {
            Type = PopupType.DEFAULT,
            ShowCloseButton = false,
            BodyText = "This app requires location permissions to work, please allow location permissions when prompted.",
            ButtonDatas = new PopupButtonData[]
            {
                new PopupButtonData()
                {
                    Action = RequestLocationPermission,
                    Text = "Continue",
                    CloseOnClick = true
                }
            }
        });
#else
        MoveToMap();
#endif
    }

    private void RequestLocationPermission()
    {
        var callbacks = new PermissionCallbacks();
        callbacks.PermissionDenied += PermissionDenied;
        callbacks.PermissionDeniedAndDontAskAgain += PermissionDenied;
        callbacks.PermissionGranted += PermissionGranted;

        Permission.RequestUserPermission(Permission.FineLocation, callbacks);
    }

    private void PermissionGranted(string _)
    {
        MoveToMap();
    }

    private void PermissionDenied(string _)
    {
        PopupManager.Instance.OpenPopup(new PopupData()
        {
            Type = PopupType.DEFAULT,
            ShowCloseButton = false,
            BodyText = "This app requires location permissions to work. Please enable location permissions in settings to enable this app to work.",
            ButtonDatas = new PopupButtonData[]
            {
                new PopupButtonData()
                {
                    Action = () => Application.Quit(),
                    Text = "Exit",
                    CloseOnClick = true
                }
            }
        });
    }

    private void MoveToMap()
    {
        SceneManager.LoadScene(m_mapSceneName, LoadSceneMode.Single);
    }
}
