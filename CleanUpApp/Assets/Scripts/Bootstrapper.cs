using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private string m_startingSceneName;

    private BootstrapperDependancy[] m_dependancies;
    private HashSet<BootstrapperDependancy> m_completeDependancies;

    private void Awake()
    {
        m_completeDependancies = new HashSet<BootstrapperDependancy>();

        m_dependancies = FindObjectsOfType<BootstrapperDependancy>();
        foreach (BootstrapperDependancy dependancy in m_dependancies)
        {
            dependancy.OnDependancyComplete += HandleDependancyComplete;
        }
    }

    private void HandleDependancyComplete(BootstrapperDependancy dependancy)
    {
        m_completeDependancies.Add(dependancy);

        if (m_completeDependancies.Count == m_dependancies.Length)
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(m_startingSceneName, LoadSceneMode.Single);
    }
}
