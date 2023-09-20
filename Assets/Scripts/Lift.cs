using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    //animation
    private Animator animator;
    private string currentState;
    private BoxCollider2D col;
    public bool isRepaired;
    public GameObject liftDad;
    public GameObject startDad;
    public GameObject reloadPanel;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRepaired)
        {
            liftDad.SetActive(true);
            Destroy(startDad);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (isRepaired)
            {
                ChangeAnimationState("TakeOff");
                reloadPanel.GetComponent<ReloadPanel>().PlayFadeBackHomeAnimation();
            }
            
        }
    }

    public void ActivateLift()
    {
        isRepaired = true;
    }

    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
