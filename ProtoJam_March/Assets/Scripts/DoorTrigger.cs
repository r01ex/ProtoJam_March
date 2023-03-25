using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject eUIPrefab;
    GameObject eUI;
    
    [SerializeField] private bool isLocked = true;
    [SerializeField] private bool isPlayerNearby = false;

    [SerializeField] AudioSource openAudio;
    [SerializeField] AudioSource closeAudio;

    private void Start()
    {
        if(!isLocked)
            this.GetComponent<Animator>().SetBool("isOpen", true);
    }

    private void Update()
    {
        if (isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DoEnterDoor();
            }
        }
    }

    //문 잠금해제
    public void SetDoorToOpen()
    {
        isLocked = false;
        // 문이 열리는 연출 넣을 곳
        openAudio.Play();
        this.GetComponent<Animator>().SetBool("isOpen", true);
    }

    //문 잠금
    public void SetDoorToClose()
    {
        isLocked = true;
        //문이 다시 잠기는 연출 넣을 곳
        closeAudio.Play();
        this.GetComponent<Animator>().SetBool("isOpen", false);
    }
    
    //문 들어가기
    public void DoEnterDoor()
    {
        if (!isLocked)
        {
            // 스테이지 클리어 작업
            GameManager.instance.Do_StageClear();
            
            // 문으로 들어가거나 다음 스테이지 이동하는 연출 넣을 곳
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isPlayerNearby = true;
            if (eUI == null)
            {
                eUI = Instantiate(eUIPrefab, this.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isPlayerNearby = false;
            if (eUI != null)
            {
                Destroy(eUI);
            }
        }
    }
}
