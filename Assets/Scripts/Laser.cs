using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class Laser : MonoBehaviour
{
    
    //rewired
    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;

    private LineRenderer _lineRenderer;
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        player = ReInput.players.GetPlayer(playerID);
        

    }
    // Update is called once per frame
    void Update()
    {
        
        _lineRenderer.SetPosition(0, transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        if (hit.collider)
        {
            _lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, transform.position.z));
        }
        else
        {
            _lineRenderer.SetPosition(1, transform.up * 2000);
        }

        //destroy laser on button up
        if (player.GetButtonUp("Fire"))
        {
            Destroy(gameObject);
        }
        
    }

    



}
