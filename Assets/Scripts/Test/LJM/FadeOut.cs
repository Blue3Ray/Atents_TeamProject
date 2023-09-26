
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 2.0f; 
    private CanvasGroup canvasGroup;
    private float alpha = 1.0f; 

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeOutCoroutine());
        
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogError("CanvasGroup is missing on this GameObject.");
                return;
            }

            StartCoroutine(FadeOutCoroutine());
        
    }

    private IEnumerator FadeOutCoroutine()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            alpha = Mathf.Lerp(1.0f, 0.0f, timer / fadeDuration);
            canvasGroup.alpha = alpha;

            timer += Time.deltaTime;
            yield return null;
        }

       
        gameObject.SetActive(false);
    }
}

