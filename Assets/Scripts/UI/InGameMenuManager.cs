using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class InGameMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_mainMenuContainer;
        [SerializeField] private Button m_resumeGameButton;
        [SerializeField] private Button m_openSettingsButton;
        [SerializeField] private Button m_quitGameButton;
        private SettingsMenuManager m_settingsMenuManager;

        //Controls what is the current selected ui object, this is for controller support.
        private EventSystem m_eventSystem;
        //so the player manager knows that the menu has been told to resume, just wanted to avoid using callbacks.
        private bool m_isActive = false;
        private bool m_setup = false;
        public bool MenuActive => m_isActive;

        private void Start()
        {
            if (!m_setup)
                Setup();
        }

        private void Setup()
        {
            m_eventSystem = FindObjectOfType<EventSystem>();
            m_settingsMenuManager = GetComponent<SettingsMenuManager>();
            m_resumeGameButton.onClick.AddListener(() => { ToggleActive(); });
            m_openSettingsButton.onClick.AddListener(() =>
            {
                m_mainMenuContainer.SetActive(false);
                m_settingsMenuManager.SetActive(true);
            });
            m_quitGameButton.onClick.AddListener(() => { Application.Quit(); });
            m_setup = true;
        }

        public void ToggleActive()
        {
            if (!m_setup)
                Setup();
            m_isActive = !m_isActive;

            //If this is already open than we want to save the values.
            if (m_settingsMenuManager.m_settingsMenuContainer.activeInHierarchy)
                m_settingsMenuManager.SetActive(false);

            if (m_isActive)
            {
                m_eventSystem.SetSelectedGameObject(m_resumeGameButton.gameObject);
            }
            gameObject.SetActive(m_isActive);
            m_mainMenuContainer.SetActive(m_isActive);
        }

        private void PauseMenuInputStart(InputAction.CallbackContext callback)
        {
            ToggleActive();
        }
        public void SetupInputCallbacks()
        {
            PlayerInputProcessor.Instance.m_playerInput.Default.Pause.started += PauseMenuInputStart;
        }
        public void RemoveInputCallbacks()
        {
            PlayerInputProcessor.Instance.m_playerInput.Default.Pause.started -= PauseMenuInputStart;
        }
    }
}