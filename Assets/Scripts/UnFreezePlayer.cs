using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnFreezePlayer : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Player.GetComponent<PlayerController>().ReAssignPlayerInput();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
