using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement m_mainCamera;
    
    [Space(10)]
    public List<Transform> m_listOfStartPos;
    public List<Transform> m_listOfCameraPos;
    
    [Space(10)]
    [SerializeField] private int m_curStage = 0;

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

    public void Do_StageClear()
    {
        m_curStage++;

        //마지막 스테이지까지 클리어 했을시 게임 클리어 작업
        if (m_curStage >= 6)
        {
            Destroy(playerScript.Instance.gameObject);
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
}
