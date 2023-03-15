using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupBox : MonoBehaviour
{
    Vector2 velocityCopy;
    float angularVelCopy;
    float inertiaCopy;
    // Start is called before the first frame update
    void Start()
    {
        playerScript.Instance.onAbilityActive.AddListener(onPlayerAbilityOn);
        playerScript.Instance.onAbilityDeactive.AddListener(onPlayerAbilityOff);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void init()
    {
        if(playerScript.Instance.isAbilityActive)
        {
            onPlayerAbilityOn();
        }
    }
    public void onPlayerAbilityOn()
    {
        velocityCopy = this.gameObject.GetComponent<Rigidbody2D>().velocity;
        angularVelCopy = this.gameObject.GetComponent<Rigidbody2D>().angularVelocity;
        inertiaCopy = this.gameObject.GetComponent<Rigidbody2D>().inertia;
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

    }
    public void onPlayerAbilityOff()
    {
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = velocityCopy;
        this.gameObject.GetComponent<Rigidbody2D>().angularVelocity = angularVelCopy;
        this.gameObject.GetComponent<Rigidbody2D>().inertia = inertiaCopy;
    }
}
