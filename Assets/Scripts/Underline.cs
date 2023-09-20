using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underline : MonoBehaviour
{
    public AudioSource source;
    public AudioClip slamSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySlamSound()
    {
        source.PlayOneShot(slamSound, .75f);
    }
}
