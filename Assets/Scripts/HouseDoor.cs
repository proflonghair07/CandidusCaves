using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{

    private AudioSource source;
    public AudioClip doorOpen;
    private bool canBeActivated;
    public GameObject outdoorsPanel;
    public BoxCollider2D boxCol;
    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        canBeActivated = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && canBeActivated == true)
        {
            outdoorsPanel.GetComponent<OutdorFade>().TouchedDoor();
            canBeActivated = false;
            source.PlayOneShot(doorOpen, 1f);
        }
    }

    public void EnableCollider()
    {
        boxCol.enabled = true;
    }
}
