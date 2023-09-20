using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadPanel : MonoBehaviour
{
    //animation
    private Animator animator;
    private string currentState;
    public GameObject player;
    public Transform teleportDestination;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //animation
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    public void PlayExitAnimation()
    {
        ChangeAnimationState("Exit");
    }

    public void PlayTeleportAnimation()
    {
        ChangeAnimationState("TeleportPlayer");
    }

    public void TeleportPlayer()
    {
        player.transform.position = teleportDestination.position;
    }

    public void PlayFadeBackHomeAnimation()
    {
        ChangeAnimationState("FadeBackHome");
    }

    public void StartCreditFadeAnimation()
    {
        ChangeAnimationState("FadeToCredits");
    }

   public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadEndHouseScene()
    {
        GameObject gm = GameObject.Find("Game Master");
        Destroy(gm);
        SceneManager.LoadScene("HouseEndScene");
    }
}
