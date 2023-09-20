using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaPlant : MonoBehaviour
{
    //animation
    private Animator animator;
    private string currentState;
    public float projectileTime;
    private bool canLaunch = true;
    public GameObject projectile;
    public Transform launchPoint;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
            if (canLaunch)
            {
                canLaunch = false;
                StartCoroutine(routine: LaunchProjectile());
            }
            
        
        
    }

    private IEnumerator LaunchProjectile()
    {
        yield return new WaitForSeconds(projectileTime);
        ChangeAnimationState("Launch");
    }

    public void LaunchTheProjectile()
    {
        Instantiate(projectile, launchPoint.position, launchPoint.rotation);
        ChangeAnimationState("Idle");
        canLaunch = true;
    }

    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
