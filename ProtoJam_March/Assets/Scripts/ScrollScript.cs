using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    [SerializeField] Animator scrollmove;
    [SerializeField] Animator scrolltip;
    private void Start()
    {
        playerScript.Instance.onAbilityActive.AddListener(onPlayerAbilityOn);
        playerScript.Instance.onAbilityDeactive.AddListener(onPlayerAbilityOff);
    }

    void onPlayerAbilityOn()
    {
        scrollmove.speed = 0;
        scrolltip.speed = 0;
    }
    void onPlayerAbilityOff()
    {
        scrollmove.speed = 1;
        scrolltip.speed = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //SmokyOnion -> R01ex :: 이부분의 주석이 유니코드 에러가 난건지 뷁쉙풹으로 되어있습니다.
        }
    }
}
