using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrailController : MonoBehaviour
{
    [SerializeField] GameObject ghostTrailPrefab;
    Coroutine spawner;
    List<GameObject> traillist = new List<GameObject>();
    private void Start()
    {
        playerScript.Instance.onAbilityActive.AddListener(onPlayerAbilityOn);
        playerScript.Instance.onAbilityDeactive.AddListener(onPlayerAbilityOff);
    }

    private void Update()
    {

    }
    void onPlayerAbilityOn()
    {
        spawner = StartCoroutine(trailSpawner());
    }
    void onPlayerAbilityOff()
    {
        StopCoroutine(spawner);
        foreach(GameObject g in traillist)
        {
            Destroy(g);
        }
    }
    IEnumerator trailSpawner()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject g = Instantiate(ghostTrailPrefab, this.transform);
            g.transform.SetParent(null);
            traillist.Add(g);
            //g.transform.localScale = this.transform.localScale;
            g.GetComponent<SpriteRenderer>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
