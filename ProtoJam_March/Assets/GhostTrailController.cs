using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrailController : MonoBehaviour
{
    public GameObject ghostTrailPrefab;
    public float trailSpawnCycleTime;
    public SpriteRenderer spriteRenderer;
    private float curtime;
    private bool isGhostTrailing = false;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isGhostTrailing = !isGhostTrailing;
        }
        
        if (isGhostTrailing)
        {
            curtime += Time.deltaTime;
            if (curtime >= trailSpawnCycleTime)
            {
                GameObject tr = Instantiate(ghostTrailPrefab, this.transform.position, this.transform.rotation);
                GhostTrailEffecter effecter = tr.GetComponent<GhostTrailEffecter>();
                effecter.Init(this.transform, this.spriteRenderer);
                effecter.isStart = true;
            }
        }
    }

    public void GhostTrailStart()
    {
        isGhostTrailing = true;
    }

    public void GhostTrailStop()
    {
        isGhostTrailing = false;
    }
}
