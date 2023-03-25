using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CameraMovement m_mainCamera;
    public Intro_FadeInOut m_fadeInOut;
    public GameClearUI m_gameClearUI;
    public Text m_ClearTimeText;
    
    [Space(10)]
    public List<Transform> m_listOfStartPos;
    public List<Transform> m_listOfCameraPos;
    
    [Space(10)]
    [SerializeField] private int m_curStage = 0;

    public float m_playedTime { get; private set; }
    private bool m_isGameStoped = false;

    #region singleton
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        m_fadeInOut.gameObject.SetActive(true);
        m_fadeInOut.Do_FadeOut();

        m_playedTime = 0f;
    }

    private void Update()
    {
        if (!m_isGameStoped)
        {
            m_playedTime += Time.deltaTime;
        }
    }

    public void Do_StageClear()
    {
        m_curStage++;

        //마지막 스테이지까지 클리어 했을시 게임 클리어 작업
        if (m_curStage >= 6)
        {
            //Destroy(playerScript.Instance.gameObject);
            playerScript.Instance.gameObject.SetActive(false);
            m_ClearTimeText.text = "Clear Time : " + (int)(m_playedTime / 60) + "m" + (int)(m_playedTime % 60) + "s";
            m_gameClearUI.gameObject.SetActive(true);
            m_gameClearUI.Do_ShowUp();
        }
        else if (m_curStage <= 5)
        {
            playerScript.Instance.gameObject.SetActive(false);
            
            playerScript.Instance.transform.position = m_listOfStartPos[m_curStage].position;
            m_mainCamera.SetCameraDestination(m_listOfCameraPos[m_curStage], m_curStage == 5 ? 7.5f : 5.5f);    //마지막 스테이지 도달 시, 카메라 사이즈 조절
            
            m_listOfStartPos[m_curStage-1].parent.gameObject.SetActive(false);
            m_listOfStartPos[m_curStage].parent.gameObject.SetActive(true);
            
            playerScript.Instance.gameObject.SetActive(true);
        }
    }

    public void Do_GoToMainMenuScene()
    {
        m_fadeInOut.Do_FadeIn();
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(2.0f);
        
        Scene targetScene = SceneManager.GetSceneByName("MainMenuScene");

        if (!(targetScene.isLoaded))
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Single);
            
            while (!(ao.isDone))
            {
                yield return null;
            }
        }
    }
}
