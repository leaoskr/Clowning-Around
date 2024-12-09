using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using ns3dRudder;
using UnityEngine;

public class Wind : MonoBehaviour
{   
    [SerializeField] private AudioSource windSound;
    public float windStrength = 5f; // Strength of the wind effect
    public float windStopInterval = 2f; // Interval to change wind direction
    public float windBlowInterval = 5f; // Interval to blow wind
    public bool playerInZone = false;
    public bool playerInZone2 = false;
    public GameObject windEffect;
    private bool isBlowing = false;
    public Movement_rudder player1;
    public Movement_rudder player2;

    private void Start()
    {   
        StartCoroutine(WindEffect());
        windSound  = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInZone && isBlowing)
        {
            player1.windEffect = windStrength;
            isBlowing = false;
        }
        if (playerInZone2 && isBlowing)
        {
            player2.windEffect = windStrength;
            isBlowing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInZone = true;
            player1.inWindZone = true;
            player1.windzoneZ = player1.leanObject.transform.position.z - 1.5f;
            Debug.Log("Player in wind zone");
        }
        if (other.gameObject.CompareTag("Player2"))
        {
            playerInZone2 = true;
            player2.inWindZone = true;
            player2.windzoneZ = player2.leanObject.transform.position.z - 1.5f;
            Debug.Log("Player2 in wind zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInZone = false;
            player1.inWindZone = false;
            player1.windEffect = 0f;
            Debug.Log("Player left wind zone");
        }
        if (other.gameObject.CompareTag("Player2"))
        {
            playerInZone2 = false;
            player2.inWindZone = false;
            player2.windEffect = 0f;
            Debug.Log("Player2 left wind zone");
        }
    }

    private IEnumerator WindEffect()
    {   
        while (true)
        {   
            if (windSound!=null)
            {
                windSound.Play();
            }
            isBlowing = true;
            yield return new WaitForSeconds(windBlowInterval);
            windEffect.SetActive(false);
            if (windSound.isPlaying)
            {
                windSound.Stop();
            }
            isBlowing = false;
            FindObjectOfType<Movement_rudder>().windEffect = 0f;
            yield return new WaitForSeconds(windStopInterval);
            windEffect.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
    }

}
