using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackImage : MonoBehaviour
{
    [SerializeField] private Image m_FadingImage;
    private Coroutine m_currentFadingCoroutine = null;

    public void StartFading(bool fadeIn, float duration, Action callback)
    {
        //This just makes sure there is only one fade going on at any given time;
        if (m_currentFadingCoroutine != null)
            StopCoroutine(m_currentFadingCoroutine);
        m_currentFadingCoroutine = StartCoroutine(FadeImage(fadeIn, duration, callback));
    }

    private IEnumerator FadeImage(bool fadeIn, float duration, Action callBack)
    {
        Color fadeImageColor = m_FadingImage.color;
        float startingAlpha = fadeImageColor.a; 
        float timer = 0;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;

            fadeImageColor.a = Mathf.Lerp(startingAlpha, fadeIn ? 0 : 1, timer / duration);
            m_FadingImage.color = fadeImageColor;
            yield return null;
        }
        m_currentFadingCoroutine = null;
        callBack?.Invoke();
    }
    
    //Debug functions for testing
    [ContextMenu("Fade In")]
    private void FadeIn()
    {
        StartFading(true, 1, null);
    }

    [ContextMenu("Fade Out")]
    private void FadeOut()
    {
        StartFading(false, 1, null);
    }

}
