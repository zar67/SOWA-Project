using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private string m_startingSceneName;

    private void Start()
    {
        SceneManager.LoadScene(m_startingSceneName, LoadSceneMode.Single);
    }
}
