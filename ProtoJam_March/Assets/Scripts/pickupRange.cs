using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupRange : MonoBehaviour
{
    bool playerIsInRange = false;
    [SerializeField] GameObject eUIPrefab;
    GameObject eUI;
    [SerializeField] AudioSource boxpickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (eUI != null)
        {
            eUI.transform.position = this.transform.position - new Vector3(0, 0.9f, 0);
        }
        if (playerIsInRange)
        {
            if (Input.GetMouseButtonDown(1))
            {
                GameObject p = this.transform.parent.gameObject;
                playerScript.Instance.isholdingBox = true;
                Debug.Log("picking up " + p);
                //sprite change
                playerScript.Instance.boxOverheadUI.SetActive(true);
                boxpickup.Play();
                Destroy(p);
                playerScript.Instance.gameObject.GetComponent<Animator>().SetBool("ispickingup", true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIsInRange = false;
            Destroy(eUI);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && playerScript.Instance.isholdingBox==false)
        {
            playerIsInRange = true;
            if (eUI == null)
            {
                eUI = Instantiate(eUIPrefab, this.transform.position - new Vector3(0, 0.9f, 0), Quaternion.identity);
            }
        }
    }
}
