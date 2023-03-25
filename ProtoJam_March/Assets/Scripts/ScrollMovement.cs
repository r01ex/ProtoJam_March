using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMovement : MonoBehaviour
{
    public bool m_isMoving = false;
    public float m_scrollSpeed;
    private float m_scrollingTimer = 0f;
    private bool m_isPlayerAbilityOn = false;
    
    private void Start()
    {
        playerScript.Instance.onAbilityActive.AddListener(onPlayerAbilityOn);
        playerScript.Instance.onAbilityDeactive.AddListener(onPlayerAbilityOff);
    }

    private void onPlayerAbilityOn()
    {
        m_isPlayerAbilityOn = true;
    }
    
    private void onPlayerAbilityOff()
    {
        m_isPlayerAbilityOn = false;
    }
    
    private void Update()
    {
        if (m_isPlayerAbilityOn)
            return;
        
        m_scrollingTimer += Time.deltaTime;
        if (m_scrollingTimer >= 30f)
        {
            m_isMoving = false;
        }
        
        if (m_isMoving)
        {
            this.transform.Translate(m_scrollSpeed, 0f, 0f);
        }
    }

    public void ScrollReset()
    {
        m_isMoving = true;
        m_scrollingTimer = 0f;
    }
}
