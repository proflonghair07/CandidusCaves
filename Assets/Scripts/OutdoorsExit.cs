using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutdoorsExit : MonoBehaviour
{
    public AudioSource source;
    public AudioClip gong;

    public GameObject reloadPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            source.PlayOneShot(gong);
            reloadPanel.GetComponent<ReloadPanel>().PlayExitAnimation();
            StartCoroutine(routine: LoadMainLevel());
            GameObject gm = GameObject.Find("Game Master");
            Destroy(gm);
        }

       
    }

    private IEnumerator LoadMainLevel()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("LevelOne");
    }
}
