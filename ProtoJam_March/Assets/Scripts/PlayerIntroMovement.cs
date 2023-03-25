using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntroMovement : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject ghostTrailPrefab;
    List<GameObject> traillist = new List<GameObject>();

    private Rigidbody2D rbody;
    private float timer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        StartCoroutine(trailSpawner());
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        timer += Time.deltaTime;
        if (timer >= 8f)
        {
            this.transform.position = startPos.position;
            timer = 0f;
        }
    }
    private void Run()
    {
        rbody.velocity = new Vector2(4f, 0f);
    }
    
    IEnumerator trailSpawner()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.15f);
            GameObject g = Instantiate(ghostTrailPrefab, this.transform);
            g.transform.SetParent(null);
            traillist.Add(g);
            //g.transform.localScale = this.transform.localScale;
            g.GetComponent<SpriteRenderer>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
