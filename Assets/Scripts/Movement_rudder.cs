using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns3dRudder
{
	public class Movement_rudder : ILocomotion
	{
		private Vector4 axes;
        private Transform trans;
		public AudioSource sweatingSound;

		public float tiltSpeed = 5f;            // Speed at which the unicycle tilts left/right
		public float maxForwardSpeed = 4f;      // Maximum speed of the unicycle when moving forward
		public float acceleration = 1f;         // Rate of acceleration
		public float deceleration = 1.5f;       // Rate of deceleration when no input
		public float tiltAmount = 30f;          // Maximum tilt angle of the unicycle (left/right)
		public float fallThreshold = 25f;       // Tilt angle at which the unicycle will fall
		public float forwardTiltThreshold = 90f; // Tilt angle at which the unicycle will fall        // Speed at which the accumulator decays (for smoother control)
		public bool isFalling = false;          // Tracks if the unicycle is falling
		public float respawnDelay = 2f;         // Delay before respawning after falling
		public float AccumulationLimit = 50;
		public float windEffect = 0f;
		public Transform leanObject;
		public GameObject sweatParticles;
		public AudioSource Falling;

		private float horizontalInput;
		private float verticalInput;
		[SerializeField]
		private float currentTilt = 0f;
		private float forwardTilt = 0f;
		private float currentForwardSpeed = 0f; // The current forward speed, will change based on inertia
		private Vector3 startPosition;    
		private Vector3 RespawnPosition;
		[SerializeField]
		public bool inWindZone = false;
		[SerializeField]
		public float windzoneZ;
		private Rigidbody rb;
        // Use this for initialization

        public bool isReady = false;
        public GameManager gameManager;

        public GameObject goPosition1;
		public GameObject goPosition2;

        public bool hasTriggeredStart = false;
        public bool hasReachedGoal = false;
		private BoxCollider BoxCollider;
		public GameObject hintTwo;
        void Start()
		{
			hintTwo.SetActive(false);
            axes = Vector4.zero;
            trans = transform;
			startPosition = new Vector3(leanObject.transform.position.x, leanObject.transform.position.y, 0f);
			rb = leanObject.GetComponent<Rigidbody>();
			sweatParticles.SetActive(false);
            gameManager = FindObjectOfType<GameManager>();
			BoxCollider = leanObject.GetComponent<BoxCollider>();
        }

		// Vector4 X = Left/Right, Y = Up/Down, Z = Forward/Backward, W = Rotation
		public override void UpdateAxes(Controller3dRudder controller3dRudder, Vector4 axesWithFactor)
		{
            // save axesWithFactor like you want
            axes = axesWithFactor;
		}

		// Update is called once per frame
		void Update()
		{	
			if (isFalling) return;
			if (isReady)
			{
                leanObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
				return;
            }
            // // mutliply saved axesWithFactor with the Time.deltaTime to apply translation or rotation            
            // trans.Translate(axes * Time.deltaTime);
            // trans.Rotate(0, axes.w * Time.deltaTime, 0);
			horizontalInput = axes.x;
			verticalInput = axes.z;
			if (verticalInput > 0.2f)
			{
				currentForwardSpeed = Mathf.Min(currentForwardSpeed + acceleration * Time.deltaTime, maxForwardSpeed);
			}
			else if(verticalInput > 0)
			{
				currentForwardSpeed = Mathf.Max(currentForwardSpeed - deceleration * Time.deltaTime, 0f);
			}
			else
			{
				currentForwardSpeed = 0;
			}

			if (verticalInput > 0)
			{
				forwardTilt = Mathf.Lerp(forwardTilt, verticalInput * tiltAmount, Time.deltaTime * tiltSpeed);
			}
			else
			{
				forwardTilt = Mathf.Lerp(forwardTilt, verticalInput * tiltAmount, Time.deltaTime * tiltSpeed);
			}


			// Tilting 
			if (inWindZone)
			{
				currentTilt = Mathf.Lerp(currentTilt, horizontalInput * tiltAmount + windEffect, Time.deltaTime * tiltSpeed);
			}
			else
			{
				currentTilt = Mathf.Lerp(currentTilt, horizontalInput * tiltAmount, Time.deltaTime * tiltSpeed);
			}
			// Debug.Log(currentTilt);

			leanObject.rotation = Quaternion.Euler(forwardTilt, 0, -currentTilt); 

			FallingCheck();
			Sweating();

			Vector3 newPosition = leanObject.position + Vector3.forward * currentForwardSpeed * Time.deltaTime;
			newPosition.x = startPosition.x; // Keep x position constant
			newPosition.y = startPosition.y; // Keep y position constant
			leanObject.position = newPosition; 
		}
		void TriggerFall(int fallType)
		{   
			BoxCollider.enabled = false;
			sweatParticles.SetActive(false);
			sweatingSound.Stop();
			Falling.Play();
            isFalling = true;
			if (inWindZone)
			{
				RespawnPosition = new Vector3(startPosition.x, startPosition.y, windzoneZ);
			}
			else
			{
				RespawnPosition = new Vector3(startPosition.x, startPosition.y, leanObject.transform.position.z);
			}
			// sound effect
			// unable the camera movement 
			// Fall
			rb.constraints = RigidbodyConstraints.None;
			if (fallType == 1)
			{
				rb.AddTorque(Vector3.forward * 90f, ForceMode.Impulse);
				rb.AddForce(Vector3.down * 50f, ForceMode.Impulse);
			} 
			else if (fallType == 2)
			{
				rb.AddTorque(Vector3.forward * -90f, ForceMode.Impulse);
				rb.AddForce(Vector3.down * 50f, ForceMode.Impulse);
        	}   
			else if (fallType == 3) 
			{
				rb.AddTorque(Vector3.right * 90f, ForceMode.Impulse);
				rb.AddForce(Vector3.down * 50f, ForceMode.Impulse);
			}
			else if (fallType == 4)
			{
				rb.AddTorque(Vector3.right * -90f, ForceMode.Impulse);
				rb.AddForce(Vector3.down * 50f, ForceMode.Impulse);
			}

            if (hintTwo != null)
            {
                hintTwo.SetActive(true);
            }

			Invoke("Respawn", respawnDelay);
		}

		void Respawn()
		{   
			// sound effect
			// Reset the rotation
			leanObject.transform.rotation = Quaternion.Euler(0, 0, 0);
			leanObject.transform.position = RespawnPosition;

			rb.velocity = Vector3.zero;
        	rb.angularVelocity = Vector3.zero;
        	rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

			// Reset movement-related variables
			currentForwardSpeed = 0f;
			
			isFalling = false;

			if (hintTwo != null) 
			{
                hintTwo.SetActive(false);
            }

            BoxCollider.enabled = true;

        }


		void FallingCheck()
		{   
			if (currentTilt < -fallThreshold)
			{	
				TriggerFall(1);
				return;
			}
			else if (currentTilt > fallThreshold)
			{
				TriggerFall(2);
				return;
			}
			if (forwardTilt > forwardTiltThreshold)
			{
				TriggerFall(3);
				return;
			}
			else if (forwardTilt < -forwardTiltThreshold)
			{
				TriggerFall(4);
				return;
			}

		}

		void Sweating()
		{
			if (Mathf.Abs(currentTilt) > fallThreshold-7 && Mathf.Abs(currentTilt) < fallThreshold || Mathf.Abs(forwardTilt) > forwardTiltThreshold-7 && Mathf.Abs(forwardTilt) < forwardTiltThreshold)
			{   
				sweatParticles.SetActive(true);
				if (!sweatingSound.isPlaying)
				{
					sweatingSound.Play();
				}
			}
			else
			{
				sweatParticles.SetActive(false);
				if (sweatingSound.isPlaying)
				{
					sweatingSound.Stop();
				}
			}
		}

		//private void OnTriggerEnter(Collider other)
		//{
		//	if (other.CompareTag("Start") && !hasTriggeredStart)
		//	{
		//		Debug.Log("start trigger");
		//		hasTriggeredStart = true;
		//		gameManager.PlayerReady(this, goPosition.transform.position); // Notify GameManager that player is ready
		//	}

		//	if (other.CompareTag("Goal") && gameManager.GameStarted && !hasReachedGoal)
		//	{
		//		hasReachedGoal = true;
		//		gameManager.PlayerReachedGoal(this); // Notify GameManager that player reached the goal
		//	}
		//}

		// Set the player's readiness state
		//public void SetReady(bool ready)
		//{
		//	isReady = ready;
		//	if (isReady) { Debug.Log(this.name + "is in ready mode"); }
		//	else { Debug.Log(this.name + "is not in ready mode"); }

		//}


	}

}