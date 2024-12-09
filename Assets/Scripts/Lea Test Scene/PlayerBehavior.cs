using System.Collections;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //public float speed = 1f;
    //public float tiltSpeed = 50f;
    //public float tiltLimit = 45f;
    //private float currentTilt = 0f;

    //public float windStrength = 0f;
    //public Vector3 windDirection;

    //private bool isFrozen = false;
    private bool isReady = false;
    private GameManager gameManager;

    public GameObject startPosition;

    private bool hasTriggeredStart = false;
    private bool hasReachedGoal = false;

    Rigidbody rb;

    // Audio
    //public AudioSource wheelAudio;
    //public AudioSource fallAudio;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();

        // Initialize the wind strength and direction
        //windStrength = Random.Range(0f, 5f);
        //windDirection = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, 0, 0);
    }

    void Update()
    {
        //if (isFrozen || isReady) return;
        if (isReady) return;

        //float tiltHorizontal = Input.GetAxis("Horizontal");

        //currentTilt += tiltHorizontal * tiltSpeed * Time.deltaTime;
        //currentTilt = Mathf.Clamp(currentTilt, -tiltLimit, tiltLimit);

        //// Player freeze for seconds when fall
        //if (currentTilt <= -tiltLimit || currentTilt >= tiltLimit)
        //{
        //    fallAudio.Play();
        //    StartCoroutine(Freeze());
        //}

        //float windTiltEffect = windStrength * windDirection.x;
        //transform.rotation = Quaternion.Euler(0, 0, -currentTilt + windTiltEffect);

        //WindChange();

        //if (Mathf.Abs(tiltHorizontal) > 0.691391)
        //{
        //    wheelAudio.Play();
        //    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Mathf.Abs(tiltHorizontal) * speed * Time.deltaTime);
        //}
    }

    // Update wind strength and direction every 5s
    //IEnumerator WindChange()
    //{
    //    yield return new WaitForSeconds(5);
    //    windStrength = Random.Range(0f, 5f);
    //    windDirection = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, 0, 0);
    //}

    //// Freeze the player for a set duration
    //IEnumerator Freeze()
    //{
    //    isFrozen = true;
    //    Debug.Log("Player is frozen due to excessive tilt.");

    //    // Wait for the freeze duration
    //    yield return new WaitForSeconds(3f);

    //    Debug.Log("Player is unfrozen.");
    //    isFrozen = false;
    //    currentTilt = 0f;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Start") && !hasTriggeredStart)
    //    {
    //        Debug.Log("start trigger");
    //        hasTriggeredStart = true;
    //        gameManager.PlayerReady(this, startPosition.transform.position); // Notify GameManager that player is ready
    //    }

    //    if (other.CompareTag("Goal") && gameManager.GameStarted && !hasReachedGoal)
    //    {
    //        hasReachedGoal = true;
    //        gameManager.PlayerReachedGoal(this); // Notify GameManager that player reached the goal
    //    }
    //}

    //// Set the player's readiness state
    //public void SetReady(bool ready)
    //{
    //    isReady = ready;
    //    if (isReady) { Debug.Log(this.name + "is in ready mode"); }
    //    else { Debug.Log(this.name + "is not in ready mode"); }

    //}

}
