using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCanvas : MonoBehaviour
{
    //animation
    private Animator animator;
    private string currentState;

    public Camera cam;
    public LayerMask playerLayer;
    public LayerMask everything;
    public PlayerController player;

    //audio effect
    public AudioSource source;
    public AudioClip djumpDallasite;
    public AudioClip dashualDatalite;
    public AudioClip glidularGabronite;
    public AudioClip reparareRhodonite;
    public AudioClip tone;

    private AudioSource gmAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gmAudioSource = GameObject.Find("Game Master").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideLayer()
    {
        player.FreezePlayer();
        player._rb.gravityScale = 0;
        player.enabled = false;
        gmAudioSource.mute = true;
        source.PlayOneShot(tone);
        player._rb.constraints = RigidbodyConstraints2D.FreezeAll;
        cam.cullingMask = playerLayer;
    }

    public void ReturnToIdle()
    {
        gmAudioSource.mute = false;
        player.enabled = true;
        player._rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        cam.cullingMask = everything;
        player._rb.gravityScale = 1;
        player.UnFreezePlayer();
    }

    public void DjumpDallasite()
    {
        source.PlayOneShot(djumpDallasite, 1.5f);
        ChangeAnimationState("DjumpDallasite");
    }

    public void DashualDatolite()
    {
        source.PlayOneShot(dashualDatalite);
        ChangeAnimationState("DashualDatolite");
    }

    public void GlidularGabronite()
    {
        source.PlayOneShot(glidularGabronite);
        ChangeAnimationState("GlidularGabronite");
    }

    public void ReparareRhodonite()
    {
        source.PlayOneShot(reparareRhodonite);
        ChangeAnimationState("ReparareRhodonite");
    }

    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
