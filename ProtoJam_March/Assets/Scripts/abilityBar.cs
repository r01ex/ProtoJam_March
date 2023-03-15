using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class abilityBar : MonoBehaviour
{
    float maxtime;
    // Start is called before the first frame update
    void Start()
    {
        maxtime = playerScript.Instance.remainingAbilityTime;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Slider>().value = playerScript.Instance.remainingAbilityTime / maxtime;
    }
}
