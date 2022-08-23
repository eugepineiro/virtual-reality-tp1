using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DecisionController : MonoBehaviour {
    private enum Direction {Left, Right};
    private enum State {
        Start, 
        Intro, 
        FirstQuestion, 
        SecondQuestion, 
        ThirdQuestion, 
        FourthQuestion, 
        FifthQuestion, 
        CheckRotation, 
        Win, 
        Lose,
        Shooting
    };

    [SerializeField] private string deadScene;
    private GameObject xrrigCamera;
    private AudioSource audioSource;
    private AudioSource townAudioSource;
    private AudioSource festivalAudioSource;

    private AudioClip audioInit;
    private AudioClip audioIntro;
    private AudioClip audioDer;
    private AudioClip audioDerDer;
    private AudioClip audioDerDerDer;
    private AudioClip audioDerDerIzq;
    private AudioClip audioDerIzq;
    private AudioClip audioIzq;
    private AudioClip audioIzqDer;
    private AudioClip audioIzqIzq;
    private AudioClip audioShot;
    private State state;
    private Vector3 prevRotation;
    private GameObject happyMate;
    private GameObject happyMateGlass;
    private GameObject angryMate;
    private GameObject angryMateRock;
    private GameObject deadMate;
    private GameObject deadMateWithGlass;
    private GameObject floorRock;
    private GameObject rockShooter;
    private GameObject[] shotMates;
    private int shotMatesAmount;
    private int questionNumber = 1;
    private float time = 0f;
    private Action<Direction>[] questionActions;

    // Start is called before the first frame update
    void Start() {
        fetchGameObjects();
        fetchAudioClips();
        state = State.Start;
        questionActions = new Action<Direction>[5] {FirstAction, SecondAction, ThirdAction, FourthAction, FifthAction};
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        if (time >= 3 && state == State.Start) {
            state = State.Intro;
            audioSource.clip = audioIntro;
            audioSource.Play();
        }
        if (time >= 16 && state == State.Intro) {
            state = State.FirstQuestion;
            audioSource.clip = audioInit;
            audioSource.Play();
        }

        if (isStateInQuestion() && !audioSource.isPlaying) {
            prevRotation = transform.rotation.eulerAngles;
            state = State.CheckRotation;
        }

        if (state == State.CheckRotation) {
            var currRotation = transform.rotation.eulerAngles;
            if (currRotation.y - prevRotation.y >= 45) {
                questionActions[questionNumber-1](Direction.Right);
                audioSource.Play();
            }
            else if (currRotation.y - prevRotation.y <= -45) {
                questionActions[questionNumber-1](Direction.Left);
                audioSource.Play();
            }
        }

        if (state == State.Shooting) {
            bool foundActiveMate = false;
            for (int i = 0; i < shotMatesAmount; i++) {
                if (shotMates[i].transform.Find("SitMate").gameObject.activeInHierarchy) {
                    foundActiveMate = true;
                    break;
                }
            }
            if (!foundActiveMate) {
                rockShooter.SetActive(false);
                state = State.Win;
                audioSource.clip = audioDerDerDer;
                audioSource.Play();
            }
        }

        if (state == State.Win && !audioSource.isPlaying) {
            Debug.Log("WIN");
            xrrigCamera.transform.position = new Vector3(20, xrrigCamera.transform.position.y, xrrigCamera.transform.position.z);
            townAudioSource.Play();
            festivalAudioSource.volume = 0.6f;
        }
        if (state == State.Lose && !audioSource.isPlaying) {
            SceneManager.LoadScene(deadScene);
            Debug.Log("LOSE");
        }
    }

    private void FirstAction(Direction direction) {
        Debug.Log("First");
        if (direction == Direction.Right) { // Refuse to give bread, mate gets angry
            Debug.Log("Derecha");
            angryMate.SetActive(true);
            state = State.ThirdQuestion;
            questionNumber = 3;
            audioSource.clip = audioDer;
        } else {
            Debug.Log("Izquierda"); // Give bread, mate gates happy
            happyMate.SetActive(true);
            state = State.SecondQuestion;
            questionNumber = 2;
            audioSource.clip = audioIzq;
        }
    }

    private void SecondAction(Direction direction) {
        Debug.Log("Second");
        happyMateGlass.SetActive(false);
        if (direction == Direction.Right) { // Give drink to another mate, he dies
            Debug.Log("Derecha");
            deadMateWithGlass.SetActive(true);
            state = State.FourthQuestion;
            questionNumber = 4;
            audioSource.clip = audioIzqDer;
        } else {
            Debug.Log("Izquierda");
            state = State.Lose; // Drink spider venom, you die
            audioSource.clip = audioIzqIzq;
            // die
        }
    }

    private void ThirdAction(Direction direction) {
        Debug.Log("Third");
        if (direction == Direction.Right) {
            Debug.Log("Derecha");  // Dodge rock, grab rock
            floorRock.SetActive(true);
            angryMateRock.SetActive(false);
            state = State.FifthQuestion;
            questionNumber = 5;
            audioSource.clip = audioDerDer;
            festivalAudioSource.volume = 0.1f;
            festivalAudioSource.Play();
        } else {
            Debug.Log("Izquierda");  // Rock hits you, you die
            angryMateRock.SetActive(false);
            state = State.Lose;
            audioSource.clip = audioDerIzq;
        }
    }

    private void FourthAction(Direction direction) {
        Debug.Log("Fourth");
        if (direction == Direction.Right) {
            Debug.Log("Derecha");  // Refuse the deal, mate kills you
            state = State.Lose;
            audioSource.mute = true;
            // audioSource.clip = audioIzqDerDer;
            // die
        } else {
            Debug.Log("Izquierda");  // Take the deal and win
            rockShooter.SetActive(true);
            audioSource.clip = audioShot;
            state = State.Shooting;
        }
    }

    private void FifthAction(Direction direction) {
        Debug.Log("Fifth");
        if (direction == Direction.Right) {  // Build picaxe, escape
            Debug.Log("Derecha");
            state = State.Win;
            audioSource.clip = audioDerDerDer;
        } else {
            Debug.Log("Izquierda");  // Throw rock, lose
            deadMate.SetActive(true);
            floorRock.SetActive(false);
            angryMate.SetActive(false);
            state = State.Lose;
            audioSource.clip = audioDerDerIzq;
        }
    }

    private bool isStateInQuestion() {
        return state == State.FirstQuestion || state == State.SecondQuestion || state == State.ThirdQuestion || state == State.FourthQuestion || state == State.FifthQuestion;
    }

    private void fetchGameObjects() {
        xrrigCamera = GameObject.Find("XRRig");
        shotMatesAmount = 2;
        shotMates = new GameObject[shotMatesAmount];
        for (int i = 0; i < shotMatesAmount; i++) {
            shotMates[i] = GameObject.Find(string.Format("ShotMate{0}", i+1));
        }
        happyMate = GameObject.Find("HappyMate");
        happyMateGlass = happyMate.transform.Find("Glass").gameObject;
        happyMate.SetActive(false);
        angryMate = GameObject.Find("AngryMate");
        angryMateRock = angryMate.transform.Find("AngryMateRock").gameObject;
        angryMate.SetActive(false);
        deadMate = GameObject.Find("DeadMate");
        deadMate.SetActive(false);
        deadMateWithGlass = GameObject.Find("DeadMateWithGlass");
        deadMateWithGlass.SetActive(false);
        floorRock = GameObject.Find("FloorRock");
        floorRock.SetActive(false);
        rockShooter = GameObject.Find("RockShooter");
        rockShooter.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        townAudioSource = GameObject.Find("Town").GetComponent<AudioSource>();
        festivalAudioSource = GameObject.Find("BonFire Festival").GetComponent<AudioSource>();
    }

    private void fetchAudioClips() {
        audioInit = Resources.Load<AudioClip>("Audio_inicial");
        audioIntro = Resources.Load<AudioClip>("Audio_intro");
        audioDer = Resources.Load<AudioClip>("Audio_der");
        audioDerDer = Resources.Load<AudioClip>("Audio_der_der");
        audioDerDerDer = Resources.Load<AudioClip>("Audio_der_der_der");
        audioDerDerIzq = Resources.Load<AudioClip>("Audio_der_der_izq");
        audioDerIzq = Resources.Load<AudioClip>("Audio_der_izq");
        audioIzq = Resources.Load<AudioClip>("Audio_izq");
        audioIzqDer = Resources.Load<AudioClip>("Audio_izq_der");
        audioIzqIzq = Resources.Load<AudioClip>("Audio_izq_izq");
        audioShot = Resources.Load<AudioClip>("Audio_shot");
    }
}
