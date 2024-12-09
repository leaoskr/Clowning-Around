using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoClip videoClip;   
    private VideoPlayer videoplayer;

    public GameObject blueClown;
    public GameObject redClown;

    public GameObject curtain;
    public Crossfade crossfade;

    void Start()
    {
        curtain.SetActive(false);

        // Show the winner
        if (GameData.winner == 0) // Blue is winner
        {
            blueClown.SetActive(true);
            redClown.SetActive(false);
        }
        else // Red is winner
        {
            blueClown.SetActive(false);
            redClown.SetActive(true);
        }

        videoplayer = gameObject.GetComponent<VideoPlayer>();

        videoplayer.clip = videoClip;
        videoplayer.Play();
        videoplayer.loopPointReached += OnVideoEnd; // Event for when the video finishes
    }

    // This method is called when a video ends
    private void OnVideoEnd(VideoPlayer vp)
    {
        curtain.SetActive(true);
        StartCoroutine(LoadNextScene()); 
    }

    private IEnumerator LoadNextScene()
    {
        //yield return new WaitForSeconds(3f);
        yield return StartCoroutine(crossfade.CrossfadeStart());

        SceneManager.LoadScene(0);
    }

}
