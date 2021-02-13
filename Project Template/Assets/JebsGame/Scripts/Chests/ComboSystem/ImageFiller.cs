using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFiller : MonoBehaviour
{
    public Image image;

    public float animationLength = 0.25f;

    public float currentFill = 0.0f;

    private void Start()
    {
        image.fillAmount = currentFill;
    }

    public void SetInstantFillAmount(float fillAmount)
    {
        image.fillAmount = fillAmount;
    }

    public void SetFillAmount(float fillAmount)
    {
        StopAllCoroutines();
        StartCoroutine(LerpFill(fillAmount,animationLength));
    }

    IEnumerator LerpFill(float target, float duration)
    {
        float time = 0;
        float startFill = currentFill;

        while (time < duration)
        {
            currentFill = Mathf.SmoothStep(startFill, target, time / duration);
            image.fillAmount = currentFill;
            time += Time.deltaTime;
            yield return null;
        }

        image.fillAmount = target;
    }

}
