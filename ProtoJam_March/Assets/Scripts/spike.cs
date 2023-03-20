using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spike : MonoBehaviour
{
    [SerializeField] float xmagnitude;
    [SerializeField] float ymagnitude;
    [SerializeField] float stunDuration;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag== "Player")
        {
            Rigidbody2D playerR2d = playerScript.Instance.gameObject.GetComponent<Rigidbody2D>();
            playerScript.Instance.spikehitRecent = true;
            playerR2d.velocity = new Vector2(-Mathf.Sign(collision.relativeVelocity.x) * xmagnitude, ymagnitude);
            //animation play
            Invoke("resetPlayer", stunDuration);
        }
    }
    void resetPlayer()
    {
        //reset animation to idle
        playerScript.Instance.spikehitRecent = false;
    }
}
