using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureMovement : MonoBehaviour
{
    public float velocity = 2;
    Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.position += velocity * new Vector3(0,0, Time.deltaTime);
        if(transform.position[2] >= 40) { 
            transform.position = new Vector3(initialPosition.x,initialPosition.y, -40);
        }
       
    }
}