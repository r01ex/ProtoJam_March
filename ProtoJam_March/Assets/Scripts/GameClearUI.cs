using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearUI : MonoBehaviour
{
    public float fadeSpeed;
    
    [SerializeField] private RectTransform tr;

    public void Do_ShowUp()
    {
        tr.anchoredPosition = new Vector2(0f, -1050f);
        StartCoroutine(coroutine_ShowUp());
    }
    
    IEnumerator coroutine_ShowUp()
    {
        Vector2 dest = new Vector2(0f, 0f);

        while (true)
        {
            if (tr.anchoredPosition.y >= -1f)
                yield break;

            tr.anchoredPosition = Vector2.Lerp(tr.anchoredPosition, dest, Time.deltaTime * fadeSpeed);
            yield return null;
        }
    }
}
