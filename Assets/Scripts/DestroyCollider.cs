using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollider : MonoBehaviour
{
    public GameObject movingPlatform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Brick")
        {
            movingPlatform.GetComponent<MovingPlatform>().shouldDie = true;
        }

        if (collision.gameObject.tag == "PlatformUp")
        {
            movingPlatform.GetComponent<MovingPlatform>().inUpZone = true;
        }

        if (collision.gameObject.tag == "PlatformDown")
        {
            movingPlatform.GetComponent<MovingPlatform>().inDownZone = true;
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlatformUp")
        {
            movingPlatform.GetComponent<MovingPlatform>().inUpZone = false;
        }

        if (collision.gameObject.tag == "PlatformDown")
        {
            movingPlatform.GetComponent<MovingPlatform>().inDownZone = false;
        }
    }
}
