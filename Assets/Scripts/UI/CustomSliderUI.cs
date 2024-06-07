using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomSliderUI : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    [SerializeField] private TMP_InputField m_inputField;

    private float m_minValue;
    private float m_maxValue;

    private event Action m_onValueChangedCallback ;
    
    void Start()
    {
        m_minValue = m_slider.minValue;
        m_maxValue = m_slider.maxValue;
        
        m_inputField.onValueChanged.AddListener(OnInputValueChanged);
        m_slider.onValueChanged.AddListener(OnValueChanged);
    }

    //This acts as an override in case we want to set it all through the main menu script
    public void SetLimit(float min, float max)
    {
        m_slider.minValue = min;
        m_slider.maxValue = max;

        m_minValue = min;
        m_maxValue = max;
    }

    private void OnInputValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            //this calls the OnValueChanged function
            float newValue = Mathf.Clamp(result, m_minValue, m_maxValue);
            
            //Rider had a go at me over tolerance.
            if (Math.Abs(Math.Round(newValue, 2) - Math.Round(m_slider.value, 2)) > 0.01f)
            {
                m_slider.value = newValue;
            }
        }
        else
        {
            Debug.LogError("Value didnt work, " + value);
        }
    }

    private void OnValueChanged(float newValue)
    {
        m_inputField.text = Math.Round(newValue, 2).ToString(CultureInfo.InvariantCulture);
        ((TextMeshProUGUI)m_inputField.placeholder).text = Math.Round(newValue, 2).ToString(CultureInfo.InvariantCulture);
        
        m_onValueChangedCallback?.Invoke();
    }

    public void AddCallbackOnValueChanged(Action callback)
    {
        m_onValueChangedCallback += callback;
    }

    public void SetValue(float value)
    {
        m_slider.value = value;
        m_inputField.text = Math.Round(value, 2).ToString(CultureInfo.InvariantCulture);
        ((TextMeshProUGUI)m_inputField.placeholder).text = Math.Round(value, 2).ToString(CultureInfo.InvariantCulture);
    }

    public float GetValue() => m_slider.value;
}
