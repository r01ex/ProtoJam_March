using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_FadeInOut : MonoBehaviour
{
    public float fadeSpeed;
    
    [SerializeField] private RectTransform tr;

    public void Do_FadeIn()
    {
        tr.anchoredPosition = new Vector2(-2150f, 0f);
        StartCoroutine(coroutine_FadeIn());
    }
    
    public void Do_FadeOut()
    {
        tr.anchoredPosition = new Vector2(0f, 0f);
        StartCoroutine(coroutine_FadeOut());
    }

    IEnumerator coroutine_FadeIn()
    {
        Vector2 dest = new Vector2(0f, 0f);

        while (true)
        {
            if (tr.anchoredPosition.x >= -1f)
                yield break;

            tr.anchoredPosition = Vector2.Lerp(tr.anchoredPosition, dest, Time.deltaTime * fadeSpeed);
            yield return null;
        }
    }
    
    IEnumerator coroutine_FadeOut()
    {
        Vector2 dest = new Vector2(2150f, 0f);

        while (true)
        {
            if (tr.anchoredPosition.x >= 2149f)
                yield break;

            tr.anchoredPosition = Vector2.Lerp(tr.anchoredPosition, dest, Time.deltaTime * fadeSpeed);
            yield return null;
        }
    }
}
