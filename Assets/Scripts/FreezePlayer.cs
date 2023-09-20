using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class FreezePlayer : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player.GetComponent<PlayerController>().player = null;
    }

    // Update is called once per frame
    void Update()
    {
        Player.GetComponent<PlayerController>().player = null;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        Player.GetComponent<PlayerController>().ChangeAnimationState("Idle");
    }
}
