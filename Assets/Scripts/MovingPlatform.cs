using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MovingPlatform : MonoBehaviour
{
    //rewired
    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;
    //instantiate explosion
    public GameObject platformExplosion;
    public GameObject platformExplosionParticles;
    public GameObject platformExplosionParticlesTwo;
    public GameObject movingPlatform;
    public Transform platformHolder;
    //movement
    public float forwardSpeed;
    public float verticalSpeed;
    private bool withPlayer = false;
    private bool wasActivated = false;
    public bool shouldDie = false;
    //respawn at original position
    public Vector2 startPos;
    //audio stuff
    AudioSource source;
    public AudioClip platforExplosion;
    public float platformExplosionVolume;

    public AudioSource propulsionSource;

    //animation
    private Animator animator;
    private string currentState;

    public bool inUpZone = false;
    public bool inDownZone = false;
    public float zoneSpeed;
  


    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
        startPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldDie == true)
        {
           // shouldDie = false;
           Instantiate(platformExplosion, new Vector2 (transform.position.x, transform.position.y + .5f), transform.rotation);
           Instantiate(platformExplosionParticles, transform.position, transform.rotation);
           Instantiate(platformExplosionParticlesTwo, transform.position, transform.rotation);

            // source.PlayOneShot(platforExplosion, platformExplosionVolume);
            if (withPlayer == true)
            {
                GameObject.Find("Player").GetComponent<PlayerController>().UpwardsBoost();
                GameObject.Find("Player").GetComponent<PlayerController>().RemoveParent();
                wasActivated = false;
            }
           // transform.position = startPos;
            //shouldDie = false;
            wasActivated = false;
            Destroy(gameObject);
           

        }

    }

    private void FixedUpdate()
    {
        if (wasActivated == true)
        {
            this.GetComponent<Target>().enabled = true;
            propulsionSource.enabled = true;
            transform.Translate(Vector3.right * forwardSpeed * Time.deltaTime);
            ChangeAnimationState("Active");
        }

        if (player.GetAxisRaw("Move Vertical") > 0f && withPlayer == true)
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
        }

        if (player.GetAxisRaw("Move Vertical") < 0f && withPlayer == true)
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime * -1);
        }

        if (inUpZone == true)
        {
            transform.Translate(Vector3.up * zoneSpeed * Time.deltaTime);
        }

        if (inDownZone == true)
        {
            transform.Translate(Vector3.up * zoneSpeed * Time.deltaTime * -1);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            wasActivated = true;
            withPlayer = true;
        }

        if (collision.gameObject.tag == "Ground")
        {


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            wasActivated = true;
            withPlayer = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            withPlayer = false;
        }

        if (collision.gameObject.tag == "PlatformUp")
        {
            

        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            withPlayer = false;
        }
    }

    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}

