using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CustomButtonUI : MonoBehaviour
    {
        [SerializeField] private Button m_button;
        [SerializeField] private Color m_activeColor = Color.green;
        [SerializeField] private Color m_nonActiveColor = Color.gray;

        [SerializeField] private List<Image> m_objectsToChangeColor = new List<Image>();
        [SerializeField] private TextMeshProUGUI m_valueDisplayText;

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

        public void SetActive(bool active)
        {
            foreach (Image imageToChange in m_objectsToChangeColor)
            {
                imageToChange.color = active ? m_activeColor : m_nonActiveColor;
            }
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
    }
}