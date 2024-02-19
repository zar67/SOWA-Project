using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class IntroScreen : MonoBehaviour
{
    [SerializeField] private string m_mapSceneName;
    [SerializeField] private GameObject m_tapToContinueHolder;

    private bool m_locationPermissionEnabled;

    private void Awake()
    {
#if UNITY_ANDROID

        m_locationPermissionEnabled = false;
        m_tapToContinueHolder.SetActive(false);

        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            m_locationPermissionEnabled = true;
            m_tapToContinueHolder.SetActive(true);
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
        m_locationPermissionEnabled = true;
#endif
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !PopupManager.Instance.HasOpenPopup() && m_locationPermissionEnabled)
        {
            MoveToMap();
        }
    }

#if UNITY_ANDROID
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
        m_locationPermissionEnabled = true;
        m_tapToContinueHolder.SetActive(true);
    }

    private void PermissionDenied(string _)
    {
        PopupManager.Instance.OpenPopup(new PopupData()
        {
            Type = PopupType.DEFAULT,
            ShowCloseButton = false,
            BodyText = "This app requires location permissions to work. The app will now exit, please enable location permissions to use this application.",
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
#endif

    private void MoveToMap()
    {
        SceneManager.LoadScene(m_mapSceneName, LoadSceneMode.Single);
    }
}
