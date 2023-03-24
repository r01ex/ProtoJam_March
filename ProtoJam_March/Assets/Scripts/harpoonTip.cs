using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harpoonTip : MonoBehaviour
{
    [SerializeField] harpoontype Harpoontype;
    bool isAbilityOn = false;
    [SerializeField] float xmagnitude;
    [SerializeField] float ymagnitude;
    [SerializeField] float stunDuration;
    enum harpoontype
    {
        triangle = 1,
        square = 2
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && playerScript.Instance.spikehitRecent == false)
        {
            if (Harpoontype == harpoontype.triangle)
            {
                //�˹�
                playerScript.Instance.spikehitRecent = true;
                collision.rigidbody.velocity = new Vector2(-Mathf.Sign(collision.relativeVelocity.x) * xmagnitude, ymagnitude);
                Invoke("resetPlayer", stunDuration);

            }
            else
            {
                if (isAbilityOn)
                {
                    //nothing
                }
                else
                {
                    //�˹�
                    playerScript.Instance.spikehitRecent = true;
                    collision.rigidbody.velocity = new Vector2(-Mathf.Sign(collision.relativeVelocity.x) * xmagnitude, ymagnitude);
                    Invoke("resetPlayer", stunDuration);
                }
            }
        }
    }
    void resetPlayer()
    {
        playerScript.Instance.spikehitRecent = false;
    }
    private void Start()
    {
        playerScript.Instance.onAbilityActive.AddListener(onPlayerAbilityOn);
        playerScript.Instance.onAbilityDeactive.AddListener(onPlayerAbilityOff);
    }

    void onPlayerAbilityOn()
    {
        isAbilityOn = true;
    }
    void onPlayerAbilityOff()
    {
        isAbilityOn = false;
    }
}
