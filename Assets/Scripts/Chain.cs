using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Chain : MonoBehaviour
{
    //rewired
    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;

    public Rigidbody2D hook;
    public GameObject[] prefabRopeSegs;
    public int numLinks = 5;
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    void GenerateRope()
    {
        Rigidbody2D prevBod = hook;
        for(int i = 0; i < numLinks; i++)
        {
            int index = Random.Range(0, prefabRopeSegs.Length);
            GameObject newSeg = Instantiate(prefabRopeSegs[index]);
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;
            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;

            prevBod = newSeg.GetComponent<Rigidbody2D>();
        }
    }

    public void Update()
    {
        if (player.GetButtonUp("Fire"))
        {
            Destroy(gameObject);
        }
    }
}
