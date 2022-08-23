using UnityEngine;

public class RockShootController : MonoBehaviour
{   
    Rigidbody rockRigidBody; 
    private GameObject player;
    bool shot = false; 

    void Start()
    {
        rockRigidBody = GetComponent<Rigidbody>();    
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.Repeat(Time.time, 4.0F); 

        if ( time < 1.0) { 
            rockRigidBody.position = player.transform.position + new Vector3(0, 0.40f, 0);
            rockRigidBody.velocity = Vector3.zero; 
            shot = false; 
        } else { 
            if ( !shot ) {
                shot = true;
                rockRigidBody.AddForce(Camera.main.transform.forward * 7500.0F); 
            }
        }
    }
}
