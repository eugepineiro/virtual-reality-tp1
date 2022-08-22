using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockShootController : MonoBehaviour
{   
    Rigidbody rockRigidBody; 
    bool shot = false; 

    void Start()
    {
        rockRigidBody = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.Repeat(Time.time, 5.0F); 

        if ( time < 2.0) { 
            rockRigidBody.position = new Vector3(-9,2,0); 
            rockRigidBody.velocity = Vector3.zero; 
            shot = false; 
        } else { 
            if ( !shot ) {
                shot = true;
                rockRigidBody.AddForce(Camera.main.transform.forward * 7000.0F); 
            }
        }
    }
}
