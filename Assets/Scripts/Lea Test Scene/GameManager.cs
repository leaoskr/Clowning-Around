using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ns3dRudder;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    private int playersReady = 0; // Track how many players are ready
    public bool GameStarted { get; private set; } = false;
    public GameObject gamingcamera;
    public GameObject p1;
    public GameObject p2;

    [SerializeField] private Movement_rudder player1;
    [SerializeField] private Movement_rudder player2;
    private Movement_rudder[] players;

    // Audio
    public AudioSource introAudio;
    public AudioSource tutorialAudio;
    public AudioSource readyAudio;
    public AudioSource countDownThreeAudio;
    public AudioSource countDownTwoAudio;
    public AudioSource countDownOneAudio;
    public AudioSource goAudio;
    public AudioSource winAudio;
    public AudioSource backgroundAudio;
    
    // UI
    public GameObject logoUI;
    public GameObject readyUI;
    public GameObject countDownThree;
    public GameObject countDownTwo;
    public GameObject countDownOne;
    public GameObject goUI;
    public GameObject blueWinUI;
    public GameObject redWinUI;

    public GameObject hintOne;
    public GameObject hintTwo;

    // Animator
    public GameObject curtain;
    public GameObject vcamera;
    private Animator curtainAnimator;
    private Animator vcameraAnimator;

    // Timer
    private float startTime;

    // Curtain
    public GameObject curtainObject;


    private void Start()
    {
        tutorialAudio.Play();
        players = new Movement_rudder[] { player1, player2 };
        Debug.Log(players[0]);

        curtainAnimator = curtain.GetComponent<Animator>();
        vcameraAnimator = vcamera.GetComponent<Animator>();

        // UI Set Up
        logoUI.SetActive(true);
        readyUI.SetActive(false);
        countDownThree.SetActive(false);
        countDownTwo.SetActive(false);
        countDownOne.SetActive(false);
        goUI.SetActive(false);
        blueWinUI.SetActive(false);
        redWinUI.SetActive(false);
        hintOne.SetActive(true);
        hintTwo.SetActive(false);

    }

    private void Update()
    {
        // Detect "Enter" key press to start curtain animation
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Set curtain "open" to true
            logoUI.SetActive(false);
            curtainAnimator.SetBool("open", true);
            Debug.Log("curtain open");
            StartCoroutine(WaitForCurtainToOpen());

        }
    }

    // Coroutine to wait for curtain animation to complete
    private IEnumerator WaitForCurtainToOpen()
    {
        yield return new WaitForSeconds(1f);

        // After curtain animation ends, start the vcamera animation
        vcameraAnimator.SetBool("Start", true);
        Debug.Log("camera move start");
        yield return new WaitForSeconds(2.5f);
        gamingcamera.SetActive(true);
        vcamera.SetActive(false);
        // Play intro audio
        introAudio.Play();

        //Disable Curtain
        curtainObject.SetActive(false);
        hintOne.SetActive(false);
    }

    public void PlayerReady(int index, Vector3 startPosition)
    {   

        if (index == 1)
        {
            player1.transform.position = startPosition;
            player1.isReady = true;
            player1.sweatingSound.Stop();
            Debug.Log(player1.name + " is ready!");
            readyAudio.Play();
            playersReady++;
        }
        else if(index == 2)
        {
            player2.transform.position = startPosition;
            player2.isReady = true;
            player2.sweatingSound.Stop();
            Debug.Log(player2.name + " is ready!");
            readyAudio.Play();
            playersReady++;
        }


        if (playersReady == 2)
        {
            backgroundAudio.Play();
            tutorialAudio.Pause();
            StartCoroutine(StartGameCountdown());
        }
    }

    private IEnumerator StartGameCountdown()
    {
        readyUI.SetActive(true);

        yield return new WaitForSeconds(1);
        readyUI.SetActive(false);
        Debug.Log("3");
        countDownThree.SetActive(true);
        countDownThreeAudio.Play();

        yield return new WaitForSeconds(1);
        countDownThree.SetActive(false);
        Debug.Log("2");
        countDownTwo.SetActive(true);
        countDownTwoAudio.Play();

        yield return new WaitForSeconds(1);
        countDownTwo.SetActive(false);
        Debug.Log("1");
        countDownOne.SetActive(true);
        countDownOneAudio.Play();

        yield return new WaitForSeconds(1);
        countDownOne.SetActive(false);
        Debug.Log("Go!");
        goUI.SetActive(true);
        goAudio.Play();
        GameStarted = true; // Allow goal collision detection

        yield return new WaitForSeconds(1);
        goUI.SetActive(false);

        // Start the timer
        startTime = Time.time;
        Destroy(hintTwo);

        player1.isReady = false;
        player2.isReady = false;
    }

    public void PlayerReachedGoal(int index)
    {   

        p1.SetActive(false);
        p2.SetActive(false);
        if (index ==1)
        {   
            
            Debug.Log(player1.name + "Wins!");

            GameData.finishTime = Time.time - startTime;

            if (player1 == players[0]) // Player 1
            {
                blueWinUI.SetActive(true);
                GameData.winner = 0;
            }
            else if (player1 == players[1]) // Player 2
            {
                redWinUI.SetActive(true);
                GameData.winner = 1;
            }

            winAudio.Play();
            GameStarted = false; // Reset game state

            StartCoroutine(LoadNextScene());
        }
        if (index == 2)
        {
            Debug.Log(player2.name + "Wins!");

            GameData.finishTime = Time.time - startTime;

            if (player2 == players[0]) // Player 1
            {
                blueWinUI.SetActive(true);
                GameData.winner = 0;
            }
            else if (player2 == players[1]) // Player 2
            {
                redWinUI.SetActive(true);
                GameData.winner = 1;
            }

            winAudio.Play();
            GameStarted = false; // Reset game state

            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(1);
    }
}
