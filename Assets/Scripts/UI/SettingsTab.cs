using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SettingsTab : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_uiObjects;
        [SerializeField] private GameObject m_firstObjectToSelect;

        public void Enable(ref EventSystem eventSystem)
        {
            ResetAllObjects();
            gameObject.SetActive(true);
            SelectFirstObject(ref eventSystem);
        }

        public void Disable()
        {
            ResetAllObjects();
            gameObject.SetActive(false);
        }
        
        public void ResetAllObjects()
        {
            foreach (GameObject uiObject in m_uiObjects)
            {
                CustomButtonUI buttonUI = uiObject.GetComponent<CustomButtonUI>();
                if (buttonUI != null)
                    buttonUI.SetDeSelectedColor();
                CustomSliderUI sliderUI = uiObject.GetComponent<CustomSliderUI>();
                if (sliderUI != null)
                    sliderUI.SetDeSelectedColor();
            }
        }

        public void SelectFirstObject(ref EventSystem eventSystem)
        {
            eventSystem.SetSelectedGameObject(m_firstObjectToSelect);
        }
    }
}