using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionController : MonoBehaviour {
    private enum Direction {Left, Right};
    private enum State {Start, FirstQuestion, SecondQuestion, ThirdQuestion, CheckRotation, Win, Lose};

    private GameObject camera;
    private AudioSource audioSource;
    private AudioClip clip2;
    private AudioClip clip3;
    // private AudioClip clip4 = Resources.Load<AudioClip>("../Audio/Test4.mp3");
    // private AudioClip clip5 = Resources.Load<AudioClip>("../Audio/Test5.mp3");
    // private AudioClip clip6 = Resources.Load<AudioClip>("../Audio/Test6.mp3");
    // private AudioClip clip7 = Resources.Load<AudioClip>("../Audio/Test7.mp3");


    private State state;

    private float time = 0f;
    private Vector3 prevRotation;

    private GameObject happyMate;
    private GameObject angryMate;

    private int questionNumber = 1;

    // Start is called before the first frame update
    void Start() {
        camera = GameObject.Find("XRRig");

        happyMate = GameObject.Find("HappyMate");
        happyMate.SetActive(false);
        angryMate = GameObject.Find("AngryMate");
        angryMate.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        clip2 = Resources.Load<AudioClip>("Test1");
        clip3 = Resources.Load<AudioClip>("Test2");

        state = State.Start;
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        if (time >= 3 && state == State.Start) {
            state = State.FirstQuestion;
            audioSource.Play();
        }

        if (!audioSource.isPlaying && state != State.Start && state != State.CheckRotation && state != State.Win && state != State.Lose) {
            prevRotation = transform.rotation.eulerAngles;
            state = State.CheckRotation;
        }

        if (state == State.CheckRotation) {
            var currRotation = transform.rotation.eulerAngles;
            if (currRotation.y - prevRotation.y >= 45) {
                switch (questionNumber) {
                    case 1:
                        FirstAction(Direction.Right);
                        break;
                    case 2:
                        SecondAction(Direction.Right);
                        break;
                    case 3:
                        ThirdAction(Direction.Right);
                        break;
                }
                Debug.Log("Play");
                audioSource.Play();
            }
            else if (currRotation.y - prevRotation.y <= -45) {
                switch (questionNumber) {
                    case 1:
                        FirstAction(Direction.Left);
                        break;
                    case 2:
                        SecondAction(Direction.Left);
                        break;
                    case 3:
                        ThirdAction(Direction.Left);
                        break;
                }
                audioSource.Play();
            }
        }

        if (state == State.Win) {
            Debug.Log("WIINNN");
            camera.transform.position = new Vector3(20, camera.transform.position.y, camera.transform.position.z);
        }
        if (state == State.Lose) {
            Debug.Log("LOOOSEERR");
        }
    }

    private void FirstAction(Direction direction) {
        Debug.Log("First");
        if (direction == Direction.Right) {
            Debug.Log("Derecha");
            angryMate.SetActive(true);
            state = State.ThirdQuestion;
            questionNumber = 3;
            audioSource.clip = clip2;
        } else {
            Debug.Log("Izquierda");
            happyMate.SetActive(true);
            state = State.SecondQuestion;
            questionNumber = 2;
            audioSource.clip = clip3;
        }
    }

    private void SecondAction(Direction direction) {
        Debug.Log("Second");
        if (direction == Direction.Right) {
            Debug.Log("Derecha");
            state = State.Win;
        } else {
            Debug.Log("Izquierda");
            state = State.Lose;
        }
    }

    private void ThirdAction(Direction direction) {
        Debug.Log("Third");
        if (direction == Direction.Right) {
            Debug.Log("Derecha");
            state = State.Win;
        } else {
            Debug.Log("Izquierda");
            state = State.Lose;
        }
    }
}
