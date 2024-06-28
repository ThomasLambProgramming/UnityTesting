using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    private enum SettingsMenuTabs
    {
        Gameplay = 0,
        Graphics,
        Audio,
        Controls,
        Misc,
    }
    public GameObject m_settingsMenuContainer;
    
    [Header("NavBar References"), Space(5f)]
    [SerializeField] private Button m_gameplaySettingsNavbarButton;
    [SerializeField] private Button m_graphicsSettingsNavbarButton;
    [SerializeField] private Button m_audioSettingsNavbarButton;
    [SerializeField] private Button m_controlsSettingsNavbarButton;
    [SerializeField] private Button m_miscSettingsNavbarButton;
    
    [SerializeField] private SettingsTab m_gameplaySettingsTab;
    [SerializeField] private SettingsTab m_graphicsSettingsTab;
    [SerializeField] private SettingsTab m_audioSettingsTab;
    [SerializeField] private SettingsTab m_controlsSettingsTab;
    [SerializeField] private SettingsTab m_miscSettingsTab;
    
    //this is used for controller input/ e and q movement between tabs and to save last settings menu that the player
    //was on just in case they want to change graphics settings and dont want to keep going settings->gameplay->move to graphics
    [SerializeField] private SettingsMenuTabs m_currentMenuTab = SettingsMenuTabs.Gameplay;
    
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

    //Controls what is the current selected ui object, this is for controller support.
    private EventSystem m_eventSystem;
    
    void Start()
    {
        m_eventSystem = FindObjectOfType<EventSystem>();
        SetupNavbarOnClick();
    }

    private void SetupNavbarOnClick()
    {
        m_gameplaySettingsNavbarButton.onClick.AddListener(() => SetSettingTab(SettingsMenuTabs.Gameplay));
        m_graphicsSettingsNavbarButton.onClick.AddListener(() => SetSettingTab(SettingsMenuTabs.Graphics));
        m_audioSettingsNavbarButton.onClick.AddListener(() => SetSettingTab(SettingsMenuTabs.Audio));
        m_controlsSettingsNavbarButton.onClick.AddListener(() => SetSettingTab(SettingsMenuTabs.Controls));
        m_miscSettingsNavbarButton.onClick.AddListener(() => SetSettingTab(SettingsMenuTabs.Misc));
    }

    private void SubscribeInputFunctions()
    {
        PlayerInputProcessor.Instance.m_playerInput.Default.ChangeMenuTabLeft.performed += MoveToLeftTab;
        PlayerInputProcessor.Instance.m_playerInput.Default.ChangeMenuTabRight.performed += MoveToRightTab;
        PlayerInputProcessor.Instance.m_playerInput.Default.MenuBack.performed += MenuBack;
    }

    private void UnsubscribeInputFunctions()
    {
        PlayerInputProcessor.Instance.m_playerInput.Default.ChangeMenuTabLeft.performed -= MoveToLeftTab;
        PlayerInputProcessor.Instance.m_playerInput.Default.ChangeMenuTabRight.performed -= MoveToRightTab;
    }

    private void MoveToRightTab(InputAction.CallbackContext context)
    {
        MoveToNewTab(1);
    }

    private void MoveToLeftTab(InputAction.CallbackContext context)
    {
        MoveToNewTab(-1);
    }

    private void MoveToNewTab(int direction)
    {
        m_currentMenuTab += direction;
        int enumLength = Enum.GetValues(typeof(SettingsMenuTabs)).Length - 1;
        
        if (m_currentMenuTab < 0)
            m_currentMenuTab = (SettingsMenuTabs)enumLength;
        else if ((int)m_currentMenuTab > enumLength)
            m_currentMenuTab = 0;
        
        SetSettingTab(m_currentMenuTab);
    }

    private void SetSettingTab(SettingsMenuTabs settingsTab)
    {
        DisableAllSettingsMenus();
        m_currentMenuTab = settingsTab;
        switch (m_currentMenuTab)
        {
            case SettingsMenuTabs.Gameplay:
                MakeButtonActive(m_gameplaySettingsNavbarButton, true);
                m_gameplaySettingsTab.Enable(ref m_eventSystem);
                break;
            case SettingsMenuTabs.Graphics:
                MakeButtonActive(m_graphicsSettingsNavbarButton, true);
                m_graphicsSettingsTab.Enable(ref m_eventSystem);
                break;
            case SettingsMenuTabs.Audio:
                MakeButtonActive(m_audioSettingsNavbarButton, true);
                m_audioSettingsTab.Enable(ref m_eventSystem);
                break;
            case SettingsMenuTabs.Controls:
                MakeButtonActive(m_controlsSettingsNavbarButton, true);
                m_controlsSettingsTab.Enable(ref m_eventSystem);
                break;
            case SettingsMenuTabs.Misc:
                MakeButtonActive(m_miscSettingsNavbarButton, true);
                m_miscSettingsTab.Enable(ref m_eventSystem);
                break;
        }
    }
    
    private void MenuBack(InputAction.CallbackContext context)
    {
        
    }

    private void DisableAllSettingsMenus()
    {
        m_gameplaySettingsTab.Disable();
        m_graphicsSettingsTab.Disable();
        m_audioSettingsTab.Disable();
        m_controlsSettingsTab.Disable();
        m_miscSettingsTab.Disable();
        
        MakeButtonActive(m_gameplaySettingsNavbarButton, false);
        MakeButtonActive(m_graphicsSettingsNavbarButton, false);
        MakeButtonActive(m_audioSettingsNavbarButton, false);
        MakeButtonActive(m_controlsSettingsNavbarButton, false);
        MakeButtonActive(m_miscSettingsNavbarButton, false);

        m_gameplaySettingsNavbarButton.image.enabled = false;
        m_graphicsSettingsNavbarButton.image.enabled = false;
        m_audioSettingsNavbarButton.image.enabled = false;
        m_controlsSettingsNavbarButton.image.enabled = false;
        m_miscSettingsNavbarButton.image.enabled = false;
    }

    private void MakeButtonActive(Button button, bool isActive)
    {
        button.image.enabled = isActive;
    }

    public void ToggleSettingsActive()
    {
        SetActive(!m_settingsMenuContainer.activeInHierarchy);
    }

    public void SetActive(bool isActive)
    {
        m_settingsMenuContainer.SetActive(isActive);

        if (isActive)
        {
            SetAllValuesFromSaveData();
            SubscribeInputFunctions();
            switch (m_currentMenuTab)
            {
                case SettingsMenuTabs.Gameplay:
                    m_gameplaySettingsNavbarButton.onClick?.Invoke();
                    break;
                case SettingsMenuTabs.Graphics:
                    m_graphicsSettingsNavbarButton.onClick?.Invoke();
                    break;
                case SettingsMenuTabs.Audio:
                    m_audioSettingsNavbarButton.onClick?.Invoke();
                    break;
                case SettingsMenuTabs.Controls:
                    m_controlsSettingsNavbarButton.onClick?.Invoke();
                    break;
                case SettingsMenuTabs.Misc:
                    m_miscSettingsNavbarButton.onClick?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            MakeButtonActive(m_gameplaySettingsNavbarButton, true);
        }
        else
        {
            SettingsData.Instance.SaveData(GetDataFromUI());
            UnsubscribeInputFunctions();   
        }
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
