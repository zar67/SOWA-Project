using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] private string m_mapSceneName;

    private void Awake()
    {
        PopupManager.Instance.OpenPopup(new PopupData()
        {
            Type = PopupType.DEFAULT,
            ShowCloseButton = false,
            BodyText = "This app requires location permissions to work, please allow location permissions when prompted.",
            ButtonDatas = new PopupButtonData[]
            {
                new PopupButtonData()
                {
                    Action = MoveToMap,
                    Text = "Continue",
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
