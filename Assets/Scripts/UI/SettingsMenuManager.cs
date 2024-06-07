using TMPro;
using UI;
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
    [SerializeField] private CustomButtonUI m_invertXAxisLookButton;
    [SerializeField] private CustomButtonUI m_invertYAxisLookButton;
    [SerializeField] private CustomSliderUI m_XAxisSensSlider;
    [SerializeField] private CustomSliderUI m_YAxisSensSlider;
    
    [Space(5f), Header("Graphics Settings")]
    [SerializeField] private TMP_Dropdown m_graphicsPresetDropdown;
    [SerializeField] private CustomSliderUI m_graphicsBrightnessSlider;
    [SerializeField] private TMP_Dropdown m_antialiasingDropdown;
    
    [Space(5f), Header("Audio Settings")]
    [SerializeField] private CustomButtonUI m_muteAllButton;
    [SerializeField] private CustomSliderUI m_masterVolumeSlider;
    [SerializeField] private CustomSliderUI m_musicVolumeSlider;
    [SerializeField] private CustomSliderUI m_soundEffectVolumeslider;
    [SerializeField] private CustomSliderUI m_dialogueVolumeSlider;
    
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

        if (isActive)
        {
            SetAllValuesFromSaveData();
            m_gameplaySettingsNavbarButton.onClick?.Invoke();
            MakeButtonActive(m_gameplaySettingsNavbarButton, true);
        }
        else
            SettingsData.Instance.SaveData(GetDataFromUI());
    }

    /// <summary>
    /// Set all sliders, buttons and input fields to have the correct data from save data or default values
    /// </summary>
    private void SetAllValuesFromSaveData()
    {
        m_invertXAxisLookButton.SetActive(SettingsData.Instance.Data.InvertXLook);
        m_invertYAxisLookButton.SetActive(SettingsData.Instance.Data.InvertYLook);
        m_XAxisSensSlider.SetValue(SettingsData.Instance.Data.MouseXSens);
        m_YAxisSensSlider.SetValue(SettingsData.Instance.Data.MouseYSens);
        m_graphicsBrightnessSlider.SetValue(SettingsData.Instance.Data.Brightness);
        m_antialiasingDropdown.value = SettingsData.Instance.Data.Antialiasing;
        m_muteAllButton.SetActive(SettingsData.Instance.Data.MuteAllAudio);
        m_masterVolumeSlider.SetValue(SettingsData.Instance.Data.MasterVolume);
        m_musicVolumeSlider.SetValue(SettingsData.Instance.Data.MusicVolume);
        m_soundEffectVolumeslider.SetValue(SettingsData.Instance.Data.SoundEffectVolume);
        m_dialogueVolumeSlider.SetValue(SettingsData.Instance.Data.DialogueVolume);
    }

    private SettingsData.SettingsStruct GetDataFromUI()
    {
        SettingsData.SettingsStruct newData = new SettingsData.SettingsStruct();
        newData.InvertXLook = m_invertXAxisLookButton.m_active;
        newData.InvertYLook = m_invertYAxisLookButton.m_active;
        newData.MouseXSens = m_XAxisSensSlider.GetValue();
        newData.MouseYSens = m_YAxisSensSlider.GetValue();
        newData.Brightness = m_graphicsBrightnessSlider.GetValue();
        newData.Antialiasing = m_antialiasingDropdown.value;
        newData.MuteAllAudio = m_muteAllButton.m_active;
        newData.MasterVolume = m_masterVolumeSlider.GetValue();
        newData.MusicVolume = m_musicVolumeSlider.GetValue();
        newData.SoundEffectVolume = m_soundEffectVolumeslider.GetValue();
        newData.DialogueVolume = m_dialogueVolumeSlider.GetValue();
        return newData;
    }
}
