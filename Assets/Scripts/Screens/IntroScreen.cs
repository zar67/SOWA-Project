using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class IntroScreen : MonoBehaviour
{
    private const float MIN_LOAD_TIME = 2f;

    [SerializeField] private string m_mapSceneName;
    [SerializeField] private GameObject m_tapToContinueHolder;
    [SerializeField] private Slider m_loadingSlider;
    [SerializeField] private AnimationCurve m_loadingCurve;
    [SerializeField] private RandomTextScriptableObject m_randomLitterStatisticText;
    [SerializeField] private TextMeshProUGUI m_litterStatisticText;

    private float m_loadingTimer = 0f;
    private bool m_locationPermissionEnabled;

    private void Awake()
    {
#if UNITY_ANDROID

        m_locationPermissionEnabled = false;
        m_tapToContinueHolder.SetActive(false);

        m_litterStatisticText.text = m_randomLitterStatisticText.ChooseRandomText();

        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            m_locationPermissionEnabled = true;
            LoadMap();
            return;
        }

        PopupManager.Instance.OpenPopup(new GenericInfoPopupData()
        {
            Type = PopupType.GENERIC_INFO,
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
        if (m_locationPermissionEnabled && m_loadingTimer < MIN_LOAD_TIME)
        {
            m_loadingTimer += Time.deltaTime;

            if (m_loadingTimer > MIN_LOAD_TIME)
            {
                m_loadingTimer = MIN_LOAD_TIME;
            }
        }

        m_loadingSlider.value = m_loadingCurve.Evaluate(m_loadingTimer / MIN_LOAD_TIME);

        m_tapToContinueHolder.SetActive(m_locationPermissionEnabled && m_loadingTimer >= MIN_LOAD_TIME);

        if (Input.GetMouseButtonDown(0) && !PopupManager.Instance.HasOpenPopup() && m_locationPermissionEnabled && m_loadingTimer >= MIN_LOAD_TIME)
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
        LoadMap();
    }

    private void PermissionDenied(string _)
    {
        PopupManager.Instance.OpenPopup(new GenericInfoPopupData()
        {
            Type = PopupType.GENERIC_INFO,
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

    private void LoadMap()
    {
        SceneManager.LoadScene(m_mapSceneName, LoadSceneMode.Additive);
    }

    private void MoveToMap()
    {
        _ = SceneManager.UnloadSceneAsync("Intro");
    }
}
