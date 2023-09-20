using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public bool onLift = false;
    private bool isActive = true;
    //NEON PLATFORMER
    //avoid issues if gamemaster is disabled
    [Header("Disable Checkpoints")]
    public bool checkPointsActive = true;
    [Header("Upgrade Bools")]
    public bool canDoubleJump;
    public bool dashingEnabled;
    public bool glidingEnabled;

    [Header("Dashing Variables")]
    private Vector2 _dashingDir;
    public bool _isDashing;
    private bool _canDash;
    //upgrades

    [Header("Dashing")]
    [SerializeField] private float _horizontalDashingVelocity;
    [SerializeField] private float _dashingVelocity = 14f;
    [SerializeField] private float _dashingTime = 0.5f;

    [Header("Double Jump")]
    private int jumpCount = 1;
    public GameObject dblJumpParticles;
    public GameObject dblJumpParticlesDown;
    public GameObject dblJumpExplosion;
    public GameObject dblJumpExplosionDown;
    public Transform dblJmpParticlePos;

    [Header("Gliding")]
    private bool isGliding = false;
    public float glidingSpeed;
    public GameObject glider;
    //moving platform stuff
    public float upwardsBoost = 20f;
    public float platformExplosionBoost = 5;
    public float platformBoost = 5;

    //reload panel
    public GameObject reloadPanel;
    //spawn door turn sprite
    public GameObject doorTurn;

    //Laser
    public GameObject laser;
    public Transform laserLaunchPos;

    //rewired
    [SerializeField] public int playerID = 0;
    [SerializeField] public Player player;

    //flip
    public bool facingRight;

    //Gamemaster
    private GameMaster gm;


    //animation
    private Animator animator;
    private string currentState;

    //coyote time testin'
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    //jump buffer
    private float jumpBufferTime = 0.24f;
    private float jumpBufferCounter;

    //movement new guy
    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float maxMoveSpeedOffPlatform;
    [SerializeField] private float maxMoveSpeedOnMovingPlatform;
    [SerializeField] private float _linearDrag;
    private float _horizontalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);

    [Header("Jump Variables")]
    [SerializeField] private float movingJumpForce = 12f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float standingJumpForce = 12f;
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    private bool _canJump => player.GetButtonDown("Jump") && _onGround;


    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Ground Collision Variables")]
    [SerializeField] private float _groundRaycastLength;
    public bool _onGround;

    [Header("Components")]
    public Rigidbody2D _rb;


    public BoxCollider2D playerCollider;

    public GameObject deathParticles;

    public CinemachineVirtualCamera cineCam;

    private bool canDie = true;

    //audio
    public AudioSource source;
    public AudioClip death;
    public AudioClip jump;
    public AudioClip[] footSteps;
    private AudioClip footStep;
    public AudioClip landingSound;
    public AudioClip dash;

    void Start()
    {
        _maxMoveSpeed = maxMoveSpeedOffPlatform;
        //flip on same bug
        facingRight = true;

        player = ReInput.players.GetPlayer(playerID);
        _rb = GetComponent<Rigidbody2D>();
        //unfreeze player
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator = GetComponent<Animator>();

        //load to last checkpoint
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

        if (checkPointsActive && gm.lastCheckPointPos != new Vector2(0,0))
        {
            transform.position = gm.lastCheckPointPos;
        }

    }



    void Update()
    {

        //gamemaster upgrade bool situation
        if(gm.GetComponent<GameMaster>().dblJumpBool == true)
        {
            canDoubleJump = true;
        }
        if (gm.GetComponent<GameMaster>().dashBool == true)
        {
            dashingEnabled = true;
        }
        if (gm.GetComponent<GameMaster>().glideBool == true)
        {
            glidingEnabled = true;
        }



        //movement
        if (isActive == true)
        {
            //dashing

            if (player.GetButtonDown("Fire") && _canDash == true && dashingEnabled == true)
            {
                source.PlayOneShot(dash);
                this.GetComponent<EchoEffect>().StartGhosting();
                _isDashing = true;
                _canDash = false;
                _dashingDir = new Vector2(x: player.GetAxisRaw("Move Horizontal"), y: player.GetAxisRaw("Move Vertical"));
                if (_dashingDir == Vector2.zero)
                {
                    if (facingRight)
                    {
                        _dashingDir = new Vector2(transform.localScale.x, y: 0);
                    }
                    if (!facingRight)
                    {
                        _dashingDir = new Vector2(transform.localScale.x * -1, y: 0);
                    }

                }
                //add stopping dash
                StartCoroutine(routine: StopDashing());
            }

            if (_isDashing)
            {
                if(player.GetAxisRaw("Move Vertical") != 0)
                {
                    _rb.velocity = _dashingDir.normalized * _dashingVelocity;
                }
                if (player.GetAxisRaw("Move Vertical") == 0)
                {
                    _rb.velocity = _dashingDir.normalized * _horizontalDashingVelocity;
                }

                return;
            }
            if (_onGround)
            {
                isGliding = false;
                _canDash = true;
                jumpCount = 1;
            }

            if (player.GetButtonDown("Jump") && !_onGround && jumpCount > 0 && canDoubleJump == true) Jump();

            //coyote time testin'
            if (_onGround)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
            if (player.GetButtonUp("Jump"))
            {
                coyoteTimeCounter = 0f;

                
            }
            else
            {
                jumpBufferCounter = jumpBufferTime;
            }

            //jump buffer attempt
            if (player.GetButtonDown("Jump"))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }


            _horizontalDirection = GetInput().x;
            if (player.GetButtonDown("Jump") && coyoteTimeCounter > 0f) Jump();
            //flip
            if (player.GetAxisRaw("Move Horizontal") > 0f && facingRight == false)
            {
                Flip();
            }
            if (player.GetAxisRaw("Move Horizontal") < 0f && facingRight == true)
            {
                Flip();
            }

            else if (_onGround && player.GetAxisRaw("Move Horizontal") != 0)
            {
                ChangeAnimationState("Run");
                _jumpForce = movingJumpForce;
            }


            else if (_onGround && player.GetAxisRaw("Move Horizontal") == 0)
            {
                ChangeAnimationState("Idle");
                _jumpForce = standingJumpForce;
            }


            else if (_onGround == false && _rb.velocity.y > 0f)
            {
                ChangeAnimationState("Jump");
            }
            else if (_onGround == false && _rb.velocity.y < 0f)
            {  
                ChangeAnimationState("Fall");
                if(glidingEnabled == true)
                {
                    if (player.GetButton("Fire"))
                    {
                        isGliding = true;
                    }
                    if (player.GetButtonUp("Fire"))
                    {
                        isGliding = false;
                    }
                    if (isGliding)
                    {
                        if (Mathf.Abs(_rb.velocity.y) > glidingSpeed)
                            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Sign(_rb.velocity.y) * glidingSpeed);
                    }
                }
                
            }
            if (player.GetButtonUp("Fire"))
            {
                isGliding = false;
            }
            //activate parachute
            if (isGliding)
            {
                glider.SetActive(true);
            }
            if (!isGliding)
            {
                glider.SetActive(false);
            }
            //Laser
            if (player.GetButtonDown("Fire"))
            {
                // Instantiate(laser, laserLaunchPos.position, laserLaunchPos.transform.rotation);
            }
        }
     


    }



    private void FixedUpdate()
    {
        MoveCharacter();

        if (_onGround)
        {
            ApplyLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
            FallMultiplier();
        }


    }

    public void UpwardsBoost()
    {
        //source.PlayOneShot(upwardsBoostSound, upwardsBoostVolume);
        _rb.AddForce(transform.up * upwardsBoost);
        if (facingRight == true)
        {
            _rb.AddForce(transform.right * platformExplosionBoost * Time.deltaTime);
        }
        if (facingRight == false)
        {
            _rb.AddForce(transform.right * platformExplosionBoost * -1 * Time.deltaTime);
        }
        // Set vibration in all Joysticks assigned to the Player
        int motorIndex = 0; // the first motor
        float motorLevel = 1.0f; // full motor speed
        float duration = .3f; // 2 seconds
        player.SetVibration(motorIndex, motorLevel, duration);
    }

    public void RemoveParent()
    {
        this.transform.parent = null;
    }

    //dashing

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingTime);
        _isDashing = false;
    }

    private Vector2 GetInput()
    {
        return new Vector2(player.GetAxisRaw("Move Horizontal"), _rb.velocity.y);
    }

    private void MoveCharacter()
    {
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);
        if(_isDashing == false && this.GetComponent<EchoEffect>().isGhosting == false)
        {
            if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
                _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
        }
       
    }


    private void ApplyLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _linearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        _rb.drag = _airLinearDrag;
    }

    private void Jump()
    {
        source.PlayOneShot(jump, .35f);
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        jumpBufferCounter = 0;
        if(coyoteTimeCounter <= 0f)
        {
            jumpCount--;
        }
        if (!_onGround  && jumpCount != 1)
        {
            
            if (player.GetAxisRaw("Move Horizontal") != 0)
            {
               // Instantiate(dblJumpParticles, dblJmpParticlePos.position, transform.rotation);
                Instantiate(dblJumpExplosion, dblJmpParticlePos.position, transform.rotation);
            }
            if (player.GetAxisRaw("Move Horizontal") == 0)
            {
                if (facingRight)
                {
                   // Instantiate(dblJumpParticlesDown, new Vector2(dblJmpParticlePos.position.x -.25f, dblJmpParticlePos.position.y - .5f), transform.rotation);
                    Instantiate(dblJumpExplosionDown, new Vector2(dblJmpParticlePos.position.x - .5f, dblJmpParticlePos.position.y), transform.rotation);
                }
                if (!facingRight)
                {
                    //Instantiate(dblJumpParticlesDown, new Vector2(dblJmpParticlePos.position.x + .25f, dblJmpParticlePos.position.y - .5f), transform.rotation);
                    Instantiate(dblJumpExplosionDown, new Vector2(dblJmpParticlePos.position.x + .5f, dblJmpParticlePos.position.y), transform.rotation);
                }

            }

        }
        
    }

    private void FallMultiplier()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _fallMultiplier;
        }
        else if (_rb.velocity.y > 0 && !player.GetButton("Jump"))
        {
            _rb.gravityScale = _lowJumpFallMultiplier;
        }
        else
        {
            _rb.gravityScale = 1f;
        }
    }



    //Flipping
    void Flip()
    {

        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;

    }
    //freeze and unfreeze player
    public void FreezePlayer()
    {
        player = null;
    }

    public void UnFreezePlayer()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    //unfreeze player after dialogue
    public void ReAssignPlayerInput()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spikes")
        {
            this.GetComponent<UnFreezePlayer>().enabled = false;
            isActive = false;
            _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            KillPlayer();
            reloadPanel.GetComponent<ReloadPanel>().PlayExitAnimation();
        }

        if (collision.gameObject.tag == "Door")
        {
            Instantiate(doorTurn, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        //parent to platform
        if (collision.gameObject.tag == "Moving Platform")
        {
            this.transform.parent = collision.transform;
            _maxMoveSpeed = maxMoveSpeedOnMovingPlatform;
        }

        if (collision.gameObject.tag == "Lift")
        {
            if(GameObject.Find("Lift").GetComponent<Lift>().isRepaired == true)
            {
                _rb.gravityScale = 2000;
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _rb.velocity = new Vector2(0, transform.position.y);
                this.transform.parent = collision.transform;
                FreezePlayer();
                cineCam.m_Follow = null;
                isActive = false;
                ChangeAnimationState("Idle");
            }
           

        }

        if(collision.gameObject.tag == "Exit")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Moving Platform")
        {
            if (facingRight == true)
            {
                _rb.AddForce(transform.right * platformBoost * Time.deltaTime);
            }
            if (facingRight == false && _onGround == false)
            {
                _rb.AddForce(transform.right * platformBoost * -.5f * Time.deltaTime);
            }
            _maxMoveSpeed = maxMoveSpeedOffPlatform;
            this.transform.parent = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Moving Platform")
        {
            source.PlayOneShot(landingSound, .75f);
        }
    }

    private void KillPlayer()
    {
        if (canDie)
        {
            Destroy(glider);
            source.PlayOneShot(death, 3);
            Time.timeScale = 0.07f;
            cineCam.m_Follow = null;
            StartCoroutine(routine: ReturnToNormalTime());
            canDie = false;
            isGliding = false;
            this.GetComponent<EchoEffect>().StartGhosting();
            Instantiate(deathParticles, transform.position, transform.rotation);
            //rumble
            // Set vibration in all Joysticks assigned to the Player
            int motorIndex = 0; // the first motor
            float motorLevel = 3f; // full motor speed
            float duration = .1f; // 2 seconds
            player.SetVibration(motorIndex, motorLevel, duration);
            FreezePlayer();
            ChangeAnimationState("Death");
        }
      
        
    }

    private IEnumerator ReturnToNormalTime()
    {
        yield return new WaitForSeconds(.027f);
        Time.timeScale = 1f;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayFootStepSound()
    {
        footStep = footSteps[Random.Range(0, footSteps.Length)];
        source.PlayOneShot(footStep);
    }




    //animation
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }




}

