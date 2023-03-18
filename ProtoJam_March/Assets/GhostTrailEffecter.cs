using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class GhostTrailEffecter : MonoBehaviour
{
    public float fadeTime;
    public Color color;
    public SpriteRenderer mySpriteRenderer;

    private Transform parents;
    public bool isStart;

    [FormerlySerializedAs("time")] [SerializeField]
    float m_time;

    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        isStart = false;
        m_time = 0f;
    }

    public void Init(Transform _parents, SpriteRenderer _spriteRenderer)
    {
        this.parents = _parents;
        this.transform.position = parents.position;
        this.transform.localScale = parents.localScale;

        this.mySpriteRenderer.sprite = _spriteRenderer.sprite;
        this.mySpriteRenderer.color = this.color;
        if (parents.localScale.x < 0)
        {
            mySpriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        m_time += Time.deltaTime;
        if (isStart)
        {
            Color _color = this.mySpriteRenderer.color;
            float _a = Mathf.Lerp(fadeTime, 0f, m_time);
            this.mySpriteRenderer.color = new Color(_color.r, _color.g, _color.b, _a);
            Debug.Log("Fadefadefade");
        }

        if (m_time >= fadeTime)
        {
            Destroy(this.gameObject);
        }
    }
}