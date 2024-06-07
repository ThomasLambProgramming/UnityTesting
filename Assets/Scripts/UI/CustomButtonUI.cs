using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CustomButtonUI : MonoBehaviour
    {
        [SerializeField] private Button m_button;
        [SerializeField] private Color m_activeColor = Color.green;
        [SerializeField] private Color m_nonActiveColor = Color.gray;

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
            m_button.image.color = active ? m_activeColor : m_nonActiveColor;
        }

        public void AddCallback(Action action)
        {
            m_onClickAction += action;
        }
    }
}