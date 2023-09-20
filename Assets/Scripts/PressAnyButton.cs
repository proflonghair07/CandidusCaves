using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PressAnyButton : MonoBehaviour
{
    //animation
    private Animator animator;
    private string currentState;
    //rewired
    [SerializeField] public int playerID = 0;
    [SerializeField] public Player player;
    public Color yellow;


    public AudioSource source;
    public AudioClip feather;
    public AudioClip buttonPress;

    private bool canPressActionButton = false;

    public GameObject transitionPanel;
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Fire") && canPressActionButton)
        {
            canPressActionButton = false;
            source.PlayOneShot(buttonPress, 2f);
            this.GetComponent<Image>().color = yellow;
            transitionPanel.GetComponent<TransitionPanel>().GoToHouse();
            ChangeAnimationState("PressedEffect");
        }
    }

    public void AllowActionButton()
    {
        canPressActionButton = true;
    }

    public void PlayFeatherSound()
    {
        source.PlayOneShot(feather, 1f);
    }

    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
