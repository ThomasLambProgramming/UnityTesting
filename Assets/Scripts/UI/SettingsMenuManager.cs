using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_settingsMenuContainer;
    
    [Header("NavBar References"), Space(5f)]
    [SerializeField] private Button m_gameplaySettingsNavbarButton;
    [SerializeField] private Button m_graphicsSettingsNavbarButton;
    [SerializeField] private Button m_audioSettingsNavbarButton;
    [SerializeField] private Button m_controlsSettingsNavbarButton;
    [SerializeField] private Button m_miscSettingsNavbarButton;
    
    [SerializeField] private GameObject m_gameplaySettingsGameObject;
    [SerializeField] private GameObject m_graphicsSettingsGameObject;
    [SerializeField] private GameObject m_audioSettingsGameObject;
    [SerializeField] private GameObject m_controlsSettingsGameObject;
    [SerializeField] private GameObject m_miscSettingsGameObject;
    
    [Space(5f), Header("Gameplay Settings")]
    [SerializeField] private Button m_invertXAxisLookButton;
    [SerializeField] private Button m_invertYAxisLookButton;
    [SerializeField] private Slider m_XAxisSensSlider;
    [SerializeField] private TMP_InputField m_XAxisSensInputField;
    [SerializeField] private Slider m_YAxisSensSlider;
    [SerializeField] private TMP_InputField m_YAxisSensInputField;
    
    [Space(5f), Header("Graphics Settings")]
    [SerializeField] private TMP_Dropdown m_graphicsPresetDropdown;
    [SerializeField] private Slider m_graphicsBrightnessSlider;
    [SerializeField] private TMP_InputField m_graphicsBrightnessInputField;
    [SerializeField] private TMP_Dropdown m_antialiasingDropdown;
    
    [Space(5f), Header("Audio Settings")]
    [SerializeField] private Button m_muteAllButton;
    [SerializeField] private Slider m_masterVolumeSlider;
    [SerializeField] private TMP_InputField m_masterVolumeInputField;
    [SerializeField] private Slider m_musicVolumeSlider;
    [SerializeField] private TMP_InputField m_musicVolumeInputField;
    [SerializeField] private Slider m_soundEffectVolumeslider;
    [SerializeField] private TMP_InputField m_soundEffectVolumeInputField;
    [SerializeField] private Slider m_dialogueVolumeSlider;
    [SerializeField] private TMP_InputField m_dislogueVolumeInputField;
    
    [Space(5f), Header("Controls Settings")]
    [SerializeField] private GameObject m_notAnOptionJustLayoutForNow;
    
    [Space(5f), Header("Misc/Debug Settings")]
    [SerializeField] private GameObject m_notAnOptionJustLayoutForNowDebug;

    [Space(5f), Header("Settings Settings")] 
    [SerializeField] private Color m_activeButtonColor = Color.green;
    [SerializeField] private Color m_disabledButtonColor = Color.gray;

    void Start()
    {
        m_gameplaySettingsNavbarButton.onClick.AddListener(() =>
        {
            DisableAllSettingsMenus();
            MakeButtonActive(m_gameplaySettingsNavbarButton, true);
            m_gameplaySettingsGameObject.SetActive(true);
        });
        m_graphicsSettingsNavbarButton.onClick.AddListener(() =>
        {
            DisableAllSettingsMenus();
            MakeButtonActive(m_graphicsSettingsNavbarButton, true);
            m_graphicsSettingsGameObject.SetActive(true);
        });
        m_audioSettingsNavbarButton.onClick.AddListener(() =>
        {
            DisableAllSettingsMenus();
            MakeButtonActive(m_audioSettingsNavbarButton, true);
            m_audioSettingsGameObject.SetActive(true);
        });
        m_controlsSettingsNavbarButton.onClick.AddListener(() =>
        {
            DisableAllSettingsMenus();
            MakeButtonActive(m_controlsSettingsNavbarButton, true);
            m_controlsSettingsGameObject.SetActive(true);
        });
        m_miscSettingsNavbarButton.onClick.AddListener(() =>
        {
            DisableAllSettingsMenus();
            MakeButtonActive(m_miscSettingsNavbarButton, true);
            m_miscSettingsGameObject.SetActive(true);
        });
    }

    private void DisableAllSettingsMenus()
    {
        m_gameplaySettingsGameObject.SetActive(false);
        m_graphicsSettingsGameObject.SetActive(false);
        m_audioSettingsGameObject.SetActive(false);
        m_controlsSettingsGameObject.SetActive(false);
        m_miscSettingsGameObject.SetActive(false);
        
        MakeButtonActive(m_gameplaySettingsNavbarButton, false);
        MakeButtonActive(m_graphicsSettingsNavbarButton, false);
        MakeButtonActive(m_audioSettingsNavbarButton, false);
        MakeButtonActive(m_controlsSettingsNavbarButton, false);
        MakeButtonActive(m_miscSettingsNavbarButton, false);
    }

    private void MakeButtonActive(Button button, bool isActive)
    {
        button.image.color = isActive ? m_activeButtonColor : m_disabledButtonColor;
    }

    public void ToggleSettingsActive(bool isActive)
    {
        m_settingsMenuContainer.SetActive(isActive);
        m_gameplaySettingsNavbarButton.onClick?.Invoke();   
        MakeButtonActive(m_gameplaySettingsNavbarButton, true);
    }
}
