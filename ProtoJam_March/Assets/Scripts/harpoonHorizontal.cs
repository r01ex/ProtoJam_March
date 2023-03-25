using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harpoonHorizontal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] harpoontype Harpoontype;
    [SerializeField] GameObject square;
    [SerializeField] GameObject triangle;
    [SerializeField] AudioSource shootAudio;
    [SerializeField] AudioSource rewindAudio;
    enum harpoontype
    {
        triangle = 1,
        square = 2
    }
    private void Start()
    {
        playerScript.Instance.onAbilityActive.AddListener(onPlayerAbilityOn);
        playerScript.Instance.onAbilityDeactive.AddListener(onPlayerAbilityOff);
        if(Harpoontype==harpoontype.triangle)
        {
            square.SetActive(false);
        }
        else
        {
            triangle.SetActive(false);
        }
    }

    void onPlayerAbilityOn()
    {
        this.gameObject.GetComponent<Animator>().speed = 0;
        if (shootAudio == null)
            return;
        shootAudio.Pause();
        rewindAudio.Pause();    
    }
    void onPlayerAbilityOff()
    {
        this.gameObject.GetComponent<Animator>().speed = 1;
        if (shootAudio == null)
            return;
        shootAudio.UnPause();
        rewindAudio.UnPause();
    }
        // Update is called once per frame
    void Update()
    {
        
    }
    public void shootAudioPlay()
    {
        if (shootAudio == null)
            return;
        shootAudio.Play();
    }
    public void rewindAudioPlay()
    {
        if (rewindAudio == null)
            return;
        rewindAudio.Play();
    }
}
