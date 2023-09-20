using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCulling : MonoBehaviour
{
    public Camera cam;
    public LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        cam.cullingMask = playerLayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
