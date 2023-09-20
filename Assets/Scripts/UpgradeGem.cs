using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeGem : MonoBehaviour
{
    private GameMaster gm;

    [Header("Upgrade Gem Types")]
    [SerializeField] private bool dblJumpGem;
    [SerializeField] private bool dashGem;
    [SerializeField] private bool glideGem;
    [SerializeField] private bool rhodonite;

    [Header("Particles")]
    public GameObject djumpParticles;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           
            if (dblJumpGem)
            {
                gm.dblJumpBool = true;
                GameObject.Find("GemCanvas").GetComponent<GemCanvas>().DjumpDallasite();
                Instantiate(djumpParticles, transform.position, transform.rotation);
            }
            if (dashGem)
            {
                gm.dashBool = true;
                GameObject.Find("GemCanvas").GetComponent<GemCanvas>().DashualDatolite();
            }
            if (glideGem)
            {
                gm.glideBool = true;
                GameObject.Find("GemCanvas").GetComponent<GemCanvas>().GlidularGabronite();
            }
            if (rhodonite)
            {
                GameObject.Find("GemCanvas").GetComponent<GemCanvas>().ReparareRhodonite();
            }

            Destroy(gameObject);
        }
    }
}
