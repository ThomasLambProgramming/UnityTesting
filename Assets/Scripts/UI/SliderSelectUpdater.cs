using UnityEngine;
using UnityEngine.EventSystems;

public class SliderSelectUpdater : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private CustomSliderUI m_sliderUi;
    
    public void OnSelect(BaseEventData eventData)
    {
        m_sliderUi.SetSelectedColor();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        m_sliderUi.SetDeSelectedColor();
    }
}
