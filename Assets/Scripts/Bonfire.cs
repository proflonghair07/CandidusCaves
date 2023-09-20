using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bonfire : MonoBehaviour
{
    private GameMaster gm;
    //animation
    private Animator animator;
    private string currentState;
    // Start is called before the first frame update
    public GameObject floppy;
    public Transform floppyPoint;
    //audio
    public AudioSource source;
    public AudioClip startFire;
    public bool canPlayFireSound = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm != null)
        {
            if (gameObject == gm.currentBonfire)
            {
                ChangeAnimationState("Active");
            }
            else if (gameObject != gm.currentBonfire)
            {
                ChangeAnimationState("Idle");
            }
        }

    }

    //animation
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            if (gm != null)
            {
                if (canPlayFireSound)
                {
                    source.PlayOneShot(startFire);
                    canPlayFireSound = false;
                }
                
                gm.lastCheckPointPos = transform.position;
                if(gm.currentBonfire != gameObject)
                {
                    Instantiate(floppy, floppyPoint.position, floppyPoint.transform.rotation);
                }
                gm.currentBonfire = gameObject;
                
            }

        }
    }
}
