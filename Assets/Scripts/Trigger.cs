using System.Collections;
using System.Collections.Generic;
using ns3dRudder;
using UnityEngine;
using UnityEngine.UIElements;

public class Trigger : MonoBehaviour
{
    public Movement_rudder player1;
    public Movement_rudder player2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Start") && !player1.hasTriggeredStart) 
        {
            Debug.Log("start trigger");
            player1.hasTriggeredStart = true;
            player1.gameManager.PlayerReady(1, player1.goPosition1.transform.position); // Notify GameManager that player is ready
        }
        else if (other.CompareTag("Start2") && !player2.hasTriggeredStart)
        {
            Debug.Log("start trigger");
            player2.hasTriggeredStart = true;
            player2.gameManager.PlayerReady(2, player2.goPosition2.transform.position); // Notify GameManager that player is ready
        }

        if (other.CompareTag("Goal") && player1.gameManager.GameStarted && !player1.hasReachedGoal)
        {
            player1.hasReachedGoal = true;
            player1.gameManager.PlayerReachedGoal(1); // Notify GameManager that player reached the goal
        }
        else if (other.CompareTag("Goal2") && player2.gameManager.GameStarted && !player2.hasReachedGoal)
        {
            player2.hasReachedGoal = true;
            player2.gameManager.PlayerReachedGoal(2); // Notify GameManager that player reached the goal
        }

    }

    // Set the player's readiness state
    public void SetReady(bool ready)
    {
        FindObjectOfType<Movement_rudder>().isReady = ready;
        if (FindObjectOfType<Movement_rudder>().isReady) { Debug.Log(this.name + "is in ready mode"); }
        else { Debug.Log(this.name + "is not in ready mode"); }

    }
}
