using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Intro_FadeInOut m_fadeInOut;
    public string m_StageSceneStr;

    private void Start()
    {
        m_fadeInOut.Do_FadeOut();
    }

    public void Do_StartGame()
    {
        m_fadeInOut.Do_FadeIn();
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(2.0f);
        
        Scene targetScene = SceneManager.GetSceneByName(m_StageSceneStr);

        if (!(targetScene.isLoaded))
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(m_StageSceneStr, LoadSceneMode.Single);
            
            while (!(ao.isDone))
            {
                yield return null;
            }
        }
    }

    public void Do_OpenOption()
    {
    }

    public void Do_ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}