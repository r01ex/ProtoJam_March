using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject lockUI;
    [SerializeField] private GameObject unlockUI;
    [SerializeField] GameObject eUIPrefab;
    GameObject eUI;
    
    [SerializeField] private bool isLocked = true;
    [SerializeField] private bool isPlayerNearby = false;

    private void Start()
    {
        lockUI.SetActive(true);
        unlockUI.SetActive(false);
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
        lockUI.SetActive(false);
        unlockUI.SetActive(true);
    }

    //문 잠금
    public void SetDoorToClose()
    {
        isLocked = true;
        //문이 다시 잠기는 연출 넣을 곳
        lockUI.SetActive(true);
        unlockUI.SetActive(false);
    }
    
    //문 들어가기
    public void DoEnterDoor()
    {
        if (!isLocked)
        {
            // 문 들어가기 관련 스크립트 넣을 곳 (당장은 그냥 Destroy)
            Destroy(playerScript.Instance.gameObject);
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
