using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitMateController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        deadMate = this.transform.Find("DeadMateShot");
        deadMate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onTriggerEnter(Collider Player)
    {
        this.SetActive(false);
        deadMate.position = this.position
        deadMate.SetActive(true);
    }
}
