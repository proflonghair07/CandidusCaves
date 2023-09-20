using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearingPlatform : MonoBehaviour
{
    //audio
    private AudioSource source;
    public AudioClip startSound;
    private bool canPlaySound = true;
    //animation
    private Animator animator;
    private string currentState;

    private GameObject dissolveParticles;
    private Rigidbody2D rb;

    public GameObject dissolveParticlesPrefab;
    public BoxCollider2D boxCol;

    private bool canCreateParticles = true;

    
    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (canCreateParticles)
            {
                dissolveParticles = Instantiate(dissolveParticlesPrefab, new Vector2 (transform.position.x, transform.position.y -.2f), transform.rotation);
                canCreateParticles = false;
            }
            
            ChangeAnimationState("Fade");
            if (canPlaySound)
            {
                source.PlayOneShot(startSound, 2f);
                canPlaySound = false;
            }
            

        }

    }


    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    public void ResetPlatform()
    {
        canPlaySound = true;
        canCreateParticles = true;
        boxCol.enabled = true;
        ChangeAnimationState("Idle");
    }

    public void KillParticles()
    {
        Destroy(dissolveParticles);
        
    }

    public void KillCollider()
    {
        boxCol.enabled = false;
    }
}
