using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(SettingsMenuManager))]
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button m_StartGameButton;
    [SerializeField] private Button m_SettingsMenuButton;
    [SerializeField] private Button m_QuitGameButton;

    private SettingsMenuManager m_SettingsMenuManager;
    void Start()
    {
        m_SettingsMenuManager = GetComponent<SettingsMenuManager>();
        m_StartGameButton.onClick.AddListener(StartGame);
        m_SettingsMenuButton.onClick.AddListener(OpenSettingsMenu);
        m_QuitGameButton.onClick.AddListener(QuitGame);
    }

    private void OpenSettingsMenu()
    {
        m_SettingsMenuManager.ToggleSettingsActive();   
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
