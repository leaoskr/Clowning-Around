using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinLose : MonoBehaviour
{
    private int winner = GameData.winner; // 0 = blue; 1= red
    private float finishTime = GameData.finishTime; // Time it took winner to finish the game

    public GameObject blueWin;
    public GameObject redWin;
    public GameObject blueLose;
    public GameObject redLose;

    public GameObject blueSpotlight;
    public GameObject redSpotlight;

    public GameObject blueConfetti;
    public GameObject redConfetti;

    public GameObject blueWinText;
    public GameObject redWinText;
    public GameObject blueLoseText;
    public GameObject redLoseText;

    public TextMeshProUGUI blueSpeedText;
    public TextMeshProUGUI redSpeedText;

    public Crossfade crossfade;

    // Start is called before the first frame update
    void Start()
    {
        /*//testing
        finishTime = 20f;
        winner = 1;*/

        if (winner == 0) // Blue is winner
        {
            blueWinText.SetActive(true);
            redWinText.SetActive(false);
            blueLoseText.SetActive(false);
            redLoseText.SetActive(true);
            blueSpeedText.text = finishTime.ToString("n1") + "s";
            redSpeedText.text = "";

            blueWin.SetActive(true);
            redWin.SetActive(false);
            blueLose.SetActive(false);
            redLose.SetActive(true);

            blueSpotlight.SetActive(true);
            redSpotlight.SetActive(false);

            blueConfetti.SetActive(true);
            redConfetti.SetActive(false);
        }
        else // Red is winner
        {
            redWinText.SetActive(true);
            blueWinText.SetActive(false);
            redLoseText.SetActive(false);
            blueLoseText.SetActive(true);
            redSpeedText.text = finishTime.ToString("n1") + "s";
            blueSpeedText.text = "";

            redWin.SetActive(true);
            blueWin.SetActive(false);
            redLose.SetActive(false);
            blueLose.SetActive(true);

            redSpotlight.SetActive(true);
            blueSpotlight.SetActive(false);

            redConfetti.SetActive(true);
            blueConfetti.SetActive(false);
        }

        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(6f);
        yield return StartCoroutine(crossfade.CrossfadeStart());
        SceneManager.LoadScene(2);
    }
}
