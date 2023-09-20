using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPanel : MonoBehaviour
{
    //animation
    private Animator animator;
    private string currentState;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToHouse()
    {
        ChangeAnimationState("FadeToHouse");
    }

    public void LoadTheFuckingHouseSceneGoddammit()
    {
        SceneManager.LoadScene("HouseInterior");
    }

    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
