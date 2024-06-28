using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class CustomButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Button m_button;
        [SerializeField] private TextMeshProUGUI m_valueDisplayText;
        [SerializeField] private List<Image> m_onSelectedImages = new List<Image>();
        [HideInInspector] public bool m_active;
        private event Action m_onClickAction;

        private void Start()
        {
            m_button.onClick.AddListener(() =>
            {
                m_active = !m_active;
                SetActive(m_active);
                m_onClickAction?.Invoke();
            });
        }

        //This is for the secondary text boxes to set the colors to be the same.
        public void SetColorsOnMenuOpen()
        {
            foreach (Image image in m_onSelectedImages)
                image.color = m_button.colors.normalColor;
        }

        public void SetActive(bool active)
        {
            SetDisplayValueText(active.ToString());
            SetColorsOnMenuOpen();
        }
        
        public void SetDisplayValueText(string text)
        {
            if (m_valueDisplayText != null)
            {
                m_valueDisplayText.text = text;
            }
        }

        public void AddCallback(Action action)
        {
            m_onClickAction += action;
        }

        public void RemoveAllCallbacks()
        {
            m_onClickAction = null;
        }
        
        public void SetSelectedColor()
        {
            foreach (Image image in m_onSelectedImages)
            {
                image.color = m_button.colors.selectedColor;
            }
        }
        
        public void SetDeSelectedColor()
        {
            foreach (Image image in m_onSelectedImages)
            {
                image.color = m_button.colors.normalColor;
            }
        }
    
        public void OnSelect(BaseEventData eventData)
        {
            SetSelectedColor();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            SetDeSelectedColor();
        }
    }
}