using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //animation
    private Animator animator;
    private string currentState;

    public AudioSource source;
    public AudioClip teleport;

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > transform.position.x)
        {
            //face right
            transform.localScale = new Vector3(-1, 1, 1);
            
        }
        else if (player.position.x < transform.position.x)
        {
            //face left
            transform.localScale = new Vector3(1, 1, 1);
            
        }
    }

    public void Teleport()
    {
        ChangeAnimationState("Teleport");
    }

    public void DestroyDad()
    {
        Destroy(gameObject);
    }

    public void PlayTeleportSound()
    {
        source.PlayOneShot(teleport, 1.65f);
    }
    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
