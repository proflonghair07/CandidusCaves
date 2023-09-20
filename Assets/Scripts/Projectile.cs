using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float forwardSpeed = 5;
    public GameObject projectileExplosion;
    public GameObject projectileParticles;
    public GameObject explosionPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * forwardSpeed * Time.deltaTime * -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            Debug.Log("touched ground projectile!");
            Instantiate(projectileExplosion, explosionPoint.transform.position, explosionPoint.transform.rotation);
            Instantiate(projectileParticles, explosionPoint.transform.position, explosionPoint.transform.rotation);
            Destroy(gameObject);
        }

      

    }
}
