using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class Pause : MonoBehaviour
{

    //rewired
    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;

    public GameObject pausePanel;
    public bool paused;
    public GameObject playerController;

    public AudioSource source;
    public AudioClip pause;
    public AudioClip unpause;



    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
        pausePanel.SetActive(false);
        paused = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused == false && player.GetButtonDown("Pause"))
        {
            playerController.GetComponent<PlayerController>().enabled = false;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            source.PlayOneShot(pause, 1f);
            paused = true;
        }
        else if (paused == true && player.GetButtonDown("Pause"))
        {
            playerController.GetComponent<PlayerController>().enabled = true;
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            source.PlayOneShot(unpause, 1f);
            paused = false;
        }


    }

}
