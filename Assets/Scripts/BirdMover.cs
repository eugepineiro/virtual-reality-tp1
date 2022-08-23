using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMover : MonoBehaviour
{
    Vector3 initialPosition;
    public float speed = 0.5F;
    public float width = 10;
    public float height = 10;
    float time = 0;

    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * speed; 

        float x = Mathf.Sin(time) * height + initialPosition.x; 
        float y = initialPosition.y;
        float z = Mathf.Cos(time) * width + initialPosition.z; 

        transform.position = new Vector3(x,y, z);
        
        transform.RotateAround(this.transform.position, Vector3.up, 40 * Time.deltaTime);
    }
}
