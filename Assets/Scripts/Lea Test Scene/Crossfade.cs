using System.Collections;
using UnityEngine;

public class Crossfade : MonoBehaviour
{
    public Animator transition;      // Reference to the Animator for crossfade
    public float transitionTime = 1f;

    // Method to start crossfade animation
    public IEnumerator CrossfadeStart()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }
}