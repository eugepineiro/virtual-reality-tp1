using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionController : MonoBehaviour {
    private AudioSource audioSource;
    private string state;

    private float time = 0f;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        state = "start";
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        if (time >= 3 && state == "start") {
            Debug.Log("Play audio");
            state = "firstQuestion";
            audioSource.Play();
        }



        // if (transform.rotation.y >= 45) {
        //     Debug.Log("")
        // }
    }
}
