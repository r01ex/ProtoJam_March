using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CameraMovement m_mainCamera;
    public DOTweenAnimation m_BGDarkAnim;
    
    [Space(10)]
    public ScrollMovement m_scrollMovement;
    public Intro_FadeInOut m_fadeInOut;
    
    [Space(10)]
    public GameClearUI m_gameClearUI;
    public GameClearUI m_gameOverUI;
    public Text m_ClearTimeText;
    public Text m_gameOverTimeText;
    public ParticleSystem m_DeathEffect;
    
    [Space(10)]
    public List<Transform> m_listOfStartPos;
    public List<Transform> m_listOfCameraPos;
    public List<Transform> m_listOfScrollStartPos;
    
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
            m_ClearTimeText.text = "Clear   Time   :   " + ((int)(m_playedTime / 60)).ToString("D2") + "분 " + ((int)(m_playedTime % 60)).ToString("D2") + "초";
            m_gameClearUI.gameObject.SetActive(true);
            m_gameClearUI.Do_ShowUp();
        }
        else if (m_curStage <= 5)
        {
            playerScript.Instance.gameObject.SetActive(false);
            
            playerScript.Instance.transform.position = m_listOfStartPos[m_curStage].position;
            m_mainCamera.SetCameraDestination(m_listOfCameraPos[m_curStage], m_curStage == 5 ? 7.5f : 5.5f);    //마지막 스테이지 도달 시, 카메라 사이즈 조절
            m_scrollMovement.transform.position = m_listOfScrollStartPos[m_curStage].position;
            if (m_curStage == 5)
            {
                m_scrollMovement.transform.localScale += new Vector3(0.35f, 0.35f, 0.35f);
            }
            m_scrollMovement.ScrollReset();
            
            m_listOfStartPos[m_curStage-1].parent.gameObject.SetActive(false);
            m_listOfStartPos[m_curStage].parent.gameObject.SetActive(true);
            
            playerScript.Instance.gameObject.SetActive(true);
            if (playerScript.Instance.isAbilityActive == true)
            {
                playerScript.Instance.ActivateAbility();
            }
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

    public void Do_GameOver()
    {
        playerScript.Instance.gameObject.SetActive(false);
        m_DeathEffect.transform.position = playerScript.Instance.transform.position;
        m_DeathEffect.Play();
        m_DeathEffect.GetComponent<AudioSource>().Play();
        Invoke("Do_ShowUpGameOverUI", 2f);
    }

    private void Do_ShowUpGameOverUI()
    {
        m_gameOverUI.gameObject.SetActive(true);
        m_gameOverTimeText.text = "Play   Time   :   " + ((int)(m_playedTime / 60)).ToString("D2") + "분 " + ((int)(m_playedTime % 60)).ToString("D2") + "초";
        m_gameOverUI.Do_ShowUp();
    }

    public void Do_AbilityEffectOn()
    {
        m_BGDarkAnim.DOPlay();
        m_mainCamera.Do_ShakeCamera();
    }
    public void Do_AbilityEffectOff()
    {
        m_BGDarkAnim.DORewind();
    }
}
