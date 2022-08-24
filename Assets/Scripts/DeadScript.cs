using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScript : MonoBehaviour
{

    private float time = 0f;

    public string loadExperience;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 15)
        {
            SceneManager.LoadScene(loadExperience);
        }
    }
}
