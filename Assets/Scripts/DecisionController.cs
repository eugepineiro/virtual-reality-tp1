using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionController : MonoBehaviour {
    private enum Direction {Left, Right};
    private enum State {
        Start, 
        FirstQuestion, 
        SecondQuestion, 
        ThirdQuestion, 
        FourthQuestion, 
        FifthQuestion, 
        CheckRotation, 
        Win, 
        Lose
    };

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
    private GameObject deadMate;
    private GameObject deadMateWithGlass;
    private GameObject floorRock;

    private int questionNumber = 1;

    // Start is called before the first frame update
    void Start() {
        camera = GameObject.Find("XRRig");

        happyMate = GameObject.Find("HappyMate");
        happyMate.SetActive(false);
        angryMate = GameObject.Find("AngryMate");
        angryMate.SetActive(false);
        deadMate = GameObject.Find("DeadMate");
        deadMate.SetActive(false);
        deadMateWithGlass = GameObject.Find("DeadMateWithGlass");
        deadMateWithGlass.SetActive(false);
        floorRock = GameObject.Find("FloorRock");
        floorRock.SetActive(false);

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
                    case 4:
                        FourthAction(Direction.Right);
                        break;
                    case 5:
                        FifthAction(Direction.Right);
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
                    case 4:
                        FourthAction(Direction.Left);
                        break;
                    case 5:
                        FifthAction(Direction.Left);
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
        if (direction == Direction.Right) { // Refuse to give bread, mate gets angry
            Debug.Log("Derecha");
            angryMate.SetActive(true);
            state = State.ThirdQuestion;
            questionNumber = 3;
            audioSource.clip = clip3;
        } else {
            Debug.Log("Izquierda"); // Give bread, mate gates happy
            happyMate.SetActive(true);
            state = State.SecondQuestion;
            questionNumber = 2;
            audioSource.clip = clip2;
        }
    }

    private void SecondAction(Direction direction) {
        Debug.Log("Second");
        if (direction == Direction.Right) { // Give drink to another mate, he dies
            Debug.Log("Derecha");
            deadMateWithGlass.SetActive(true);
            state = State.FourthQuestion;
            questionNumber = 4;
            // audioSource.clip = clip4;
        } else {
            Debug.Log("Izquierda");
            state = State.Lose; // Drink spider venom, you die
            // die
        }
    }

    private void ThirdAction(Direction direction) {
        Debug.Log("Third");
        if (direction == Direction.Right) {
            Debug.Log("Derecha");  // Dodge rock, grab rock
            floorRock.SetActive(true);
            // activate village music;
            state = State.FifthQuestion;
            questionNumber = 5;
            // audioSource.clip = clip5;
        } else {
            Debug.Log("Izquierda");  // Rock hits you, you die
            state = State.Lose;
        }
    }

    private void FourthAction(Direction direction) {
        Debug.Log("Fourth");
        if (direction == Direction.Right) {
            Debug.Log("Derecha");  // Refuse the deal, mate kills you
            state = State.Lose;
            // die
        } else {
            Debug.Log("Izquierda");  // Take the deal and win
            state = State.Win;
        }
    }

    private void FifthAction(Direction direction) {
        Debug.Log("Fifth");
        if (direction == Direction.Right) {  // Build picaxe, escape
            Debug.Log("Derecha");
            state = State.Win;
        } else {
            Debug.Log("Izquierda");  // Throw rock, lose
            deadMate.SetActive(true);
            state = State.Lose;
        }
    }
}
