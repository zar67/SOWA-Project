
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

#if UNITY_IOS
using System.Collections;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class IntroScreen : MonoBehaviour
{
    private const float MIN_LOAD_TIME = 4f;

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
#elif UNITY_IOS

        m_locationPermissionEnabled = false;
        m_tapToContinueHolder.SetActive(false);

        m_litterStatisticText.text = m_randomLitterStatisticText.ChooseRandomText();

        if (Input.location.isEnabledByUser)
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
#endif

#if UNITY_IOS
    private void RequestLocationPermission()
    {
        StartCoroutine(RequestAndWait());
    }
    private IEnumerator RequestAndWait()
    {
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            PermissionDenied();
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            PermissionDenied();
        }
        else
        {
            PermissionGranted();
        }
    }
#endif

    private void PermissionGranted(string _ = "")
    {
        m_locationPermissionEnabled = true;
        m_tapToContinueHolder.SetActive(true);
        LoadMap();
    }

    private void PermissionDenied(string _ = "")
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

    private void LoadMap()
    {
        SceneManager.LoadScene(m_mapSceneName, LoadSceneMode.Additive);
    }

    private void MoveToMap()
    {
        _ = SceneManager.UnloadSceneAsync("Intro");
        FTUEManager.Instance.BeginFTUE();
    }
}
