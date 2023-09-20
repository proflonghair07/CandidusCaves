using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    public float ghostingTime;
    public bool isGhosting = false;

    private float timeBtwSpawns;
    public float startTimeBtwSpawns;
    private PlayerController player;

    public GameObject echo;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(isGhosting)
        {

            if (timeBtwSpawns <= 0)
            {
                Instantiate(echo, new Vector2(transform.position.x, transform.position.y - .15f), transform.rotation);
                timeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
        
    }

    public void StartGhosting()
    {
        isGhosting = true;
        StartCoroutine(routine: StopGhosting());
    }

    private IEnumerator StopGhosting()
    {
        yield return new WaitForSeconds(ghostingTime);
        isGhosting = false;
    }
}
