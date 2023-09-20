using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{

    const string LEFT = "left";
    const string RIGHT = "right";

    [SerializeField] Transform castPos;
    [SerializeField] float baseCastDist;

    string facingDirection;

    Vector3 baseScale;

    Rigidbody2D rb;
    public float moveSpeed = 5;

    //animation
    private Animator animator;
    private string currentState;

    public GameObject explosion;
    public GameObject explosionLeft;
    public Transform explosionPoint;



    void Start()
    {
        baseScale = transform.localScale;
        facingDirection = RIGHT;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

    


    private void FixedUpdate()
    {
        float velocityX = moveSpeed;

        if (facingDirection == LEFT)
        {
            velocityX = -moveSpeed;
        }
        //move the game object
        rb.velocity = new Vector2(velocityX, rb.velocity.y);

        if (IsHittingWall() || IsNearEdge())
        {
            if (facingDirection == LEFT)
            {
                ChangeFacingDirection(RIGHT);
            }
            else if (facingDirection == RIGHT)
            {
                ChangeFacingDirection(LEFT);
            }
        }
    }

    void ChangeFacingDirection(string newDirection)
    {
        Vector3 newScale = baseScale;

        if (newDirection == LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else if (newDirection == RIGHT)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;

        facingDirection = newDirection;
    }

    bool IsHittingWall()
    {
        bool val = false;
        float castDist = baseCastDist;
        // define the cast distance for left and right
        if (facingDirection == LEFT)
        {
            castDist = -baseCastDist;
        }
        else
        {
            castDist = baseCastDist;
        }
        //determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = true;
        }
        else
        {
            val = false;
        }

        return val;
    }

    bool IsNearEdge()
    {
        bool val = false;
        float castDist = baseCastDist;

        //determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.red);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else
        {
            val = true;
        }

        return val;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        //parent to platform
        if (collision.gameObject.tag == "Player")
        {
            if(facingDirection == RIGHT)
            {
                Instantiate(explosion, explosionPoint.position, transform.rotation);
            }
            if (facingDirection == LEFT)
            {
                Instantiate(explosionLeft, explosionPoint.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }

    //animation
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

}
