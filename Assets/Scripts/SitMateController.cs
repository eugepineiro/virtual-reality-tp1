using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitMateController : MonoBehaviour
{
    private GameObject deadMate;
    private GameObject sitMate;

    // Start is called before the first frame update
    void Start()
    {
        deadMate = this.transform.Find("DeadMateShot").gameObject;
        deadMate.SetActive(false);
        sitMate = this.transform.Find("SitMate").gameObject;
        sitMate.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "RockShooter") {
            sitMate.SetActive(false);
            deadMate.SetActive(true);
        }
    }
}
