using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_mover : MonoBehaviour
{   

    public float velocity = 10; 
    public float angularPeriod = 8;
    public float scaleFrequency = 0.5F;

    void Start()
    {
    
    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * new Vector3(0,0, Time.deltaTime);


        float angle = Time.time * (360/ angularPeriod);
        transform.rotation = Quaternion.Euler(0, angle, 0); 

        float scale = 30 + 10 * Mathf.Cos(2* Mathf.PI * scaleFrequency * Time.time);
        transform.localScale = new Vector3(scale, 2, scale);

    }
}
