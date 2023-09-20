using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutdorFade : MonoBehaviour
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

    public void TouchedDoor()
    {
        ChangeAnimationState("Exit");
    }

    public void LoadOutdoors()
    {
        SceneManager.LoadScene("Outdoors");
    }

    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
