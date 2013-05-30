using UnityEngine;
using System.Collections;

public abstract class Movement : MonoBehaviour 
{
	public float maxSpeed;
    public float gravity;
	public float rotationSpeed;
	public float horsePower;
	public double weight;
	public bool isAlive;
	public bool Detracked;
	public bool isGrounded;
	public bool isStopped 
	{
		get 
		{
			return ((substractSpeed - maxSpeed) +
				(backwardSubstractSpeed - maxSpeedBackwards) == 0);
		} 
	}
	
	public Transform[] trackWheelsLeft;
	public Transform[] trackWheelsRight;
	
	public GameObject trackLeft;
	public GameObject trackRight;
	
	public ParticleSystem smokeExhaustLeft;
	public ParticleSystem smokeExhaustRight;
	public ParticleSystem dirtForwardLeft;
	public ParticleSystem dirtForwardRight;
	public ParticleSystem dirtBackwardLeft;
	public ParticleSystem dirtBackwardRight;
    
	public float maxSpeedSave;
	public float maxSpeedBackwards;
	public Vector3 moveDirection = Vector3.zero;
	public bool rotating;
	public float substractSpeed;
	public float backwardSubstractSpeed;
	public bool directionForward = true;
	
	public ParticleSystem smokeExhaustLeftSave;
	public ParticleSystem smokeExhaustRightSave;
	
	public virtual void Start ()
	{
		smokeExhaustLeftSave = smokeExhaustLeft;
		smokeExhaustRightSave = smokeExhaustRight;
		
		smokeExhaustLeft.Play();
		smokeExhaustRight.Play();
		
		maxSpeedBackwards = maxSpeed / 4;
		maxSpeedSave = maxSpeed;
		
		substractSpeed = maxSpeed;
		backwardSubstractSpeed = maxSpeedBackwards;
		StopAllDirt();
		CheckMobility();
	}
	
	private void CheckMobility() 
	{
        InvokeRepeating("Mobility", 1, 0.1F);
    }
	
	private void Mobility()
	{
		if(!isStopped || rotating)
		{
			if(!audio.isPlaying)
			{
				audio.Play();
			}
		}
		else
		{
			audio.Stop();
		}
	}
	
	public void Update () {
		if(isAlive && !Detracked && isGrounded)
		{
			if(!OutOfOrder())
			{
			    MoveForwardBackward();
				Rotate ();
			}
		}
		else
		{
			StopAllDirt();
		}
	}
	
	public bool OutOfOrder()
	{
		//FAKE PHYSICS version 0.1
		/* Scenarious where the tank should never have been to ( FAKE GRAVITY
		if(directionForward && gameObject.transform.localRotation.eulerAngles.x > 35
			&& gameObject.transform.localRotation.eulerAngles.x < 320 )
		{
			MoveBackward();
			return true;
		}
		
		if(!directionForward && gameObject.transform.localRotation.eulerAngles.x < 25
			&& gameObject.transform.localRotation.eulerAngles.x > 0 )
		{
			MoveForward();
			return true;
		}
		
		if(gameObject.transform.localRotation.eulerAngles.z > 20 
			&& gameObject.transform.localRotation.eulerAngles.z < 180)
		{
			moveDirection = new Vector3(10, -10, 0);
			return true;
		}
		
		if(gameObject.transform.localRotation.eulerAngles.z < 340 
			&& gameObject.transform.localRotation.eulerAngles.z > 180)
		{
			moveDirection = new Vector3(-10, -10, 0);
			return true;
		}*/
			return false;
	}
	
	public void StopAllDirt()
	{
		dirtForwardLeft.Stop();
		dirtForwardRight.Stop();
		dirtBackwardLeft.Stop();
		dirtBackwardRight.Stop();
	}
	
	public void MoveForwardBackward()
	{
		MoveForward();
		MoveBackward();
		
		if(directionForward)
		{
			moveDirection = new Vector3(0, 0, 1);
		}
		else
		{
			moveDirection = new Vector3(0, 0, -1);
		}
		
		if(substractSpeed > maxSpeed)
		{
			substractSpeed = maxSpeed;
		}
		if(substractSpeed < 0)
		{
			substractSpeed = 0;
		}
		
		if(backwardSubstractSpeed < 0)
		{
			backwardSubstractSpeed = 0;
		}
		if(backwardSubstractSpeed > maxSpeedBackwards)
		{
			backwardSubstractSpeed = maxSpeedBackwards;
		}
		
		moveDirection = transform.TransformDirection(moveDirection);
		
		if(directionForward)
		{
			moveDirection *= (maxSpeed - substractSpeed);
		}
		else
		{
			moveDirection *= (maxSpeedBackwards - backwardSubstractSpeed);
		}
		
		float emmiterSpeed = (maxSpeed - substractSpeed) + (maxSpeedBackwards - backwardSubstractSpeed);
		ThrowDirt(directionForward, emmiterSpeed, false, false);
		SpinTracks(directionForward, false, false);
		SpinWheelsWithTracks(directionForward, false, false);
		
		rigidbody.velocity = moveDirection;
	}
	
	public void Rotate()
	{
		rotating = false;
		TurnLeft();
		TurnRight();
		
		if(maxSpeed - substractSpeed == 0 && maxSpeedBackwards - backwardSubstractSpeed == 0)
		{
			directionForward = true;
		}
		
		if(rotating)
		{
			maxSpeed -= (float)((weight/5000)*Time.deltaTime);
			substractSpeed -= (float)((weight/5000)*Time.deltaTime);
		}
		else
		{
			if(maxSpeed < maxSpeedSave)
			{
				substractSpeed += Time.deltaTime * horsePower / 10;
				maxSpeed += Time.deltaTime * horsePower / 10;
			}
		}
		if(maxSpeed <= 0)
		{
			maxSpeed = 0;
		}
	}
	
	public virtual void MoveForward()
	{
		directionForward = true;
		substractSpeed -= (float)( weight / 3000 ) * Time.deltaTime;
	}
	
	public virtual void MoveBackward()
	{
		if(substractSpeed < maxSpeed)
		{
				substractSpeed += (float)( weight / 2000 ) * Time.deltaTime;
		}
		else
		{
			directionForward = false;
			backwardSubstractSpeed -= (float)( weight / 3000 ) * Time.deltaTime;
		}
	}
	
	public virtual void TurnLeft()
	{
		if((directionForward || maxSpeed-substractSpeed == 0) && maxSpeedBackwards - backwardSubstractSpeed == 0)
		{
				transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
		}
		else
		{
			transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed, Space.World);
		}
		
		rotating = true;
		
		float emmiterSpeed = (maxSpeed - substractSpeed) + (maxSpeedBackwards - backwardSubstractSpeed);
		SpinTracks(directionForward, false, true);
		SpinWheelsWithTracks(directionForward, false, true);
//				EmmitSmoke(directionForward, false, true, emmiterSpeed);
		ThrowDirt(directionForward, emmiterSpeed, false, true);
		
	}
	
	public virtual void TurnRight()
	{
		if((directionForward|| maxSpeed-substractSpeed == 0) && maxSpeedBackwards-backwardSubstractSpeed == 0)
		{
			transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed, Space.World);
		}
		else
		{
			transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
		}
		rotating = true;
		
		float emmiterSpeed = (maxSpeed - substractSpeed) + (maxSpeedBackwards - backwardSubstractSpeed);
		SpinTracks(directionForward, true, false);
		SpinWheelsWithTracks(directionForward, true, false);
//				EmmitSmoke(directionForward, true, false, emmiterSpeed);
		ThrowDirt(directionForward, emmiterSpeed, true, false);
	}
	
	private void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name == "Terrain")
		{
			isGrounded = false;
		}
    }
	
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name == "Terrain")
		{
			isGrounded = true;
		}
    }	
	
	public void SpinWheels(Vector3 direction, float speed, Transform[] wheels)
	{
		bool skipZeroIndex = true; // to remove the parent
		foreach (Transform wheel in wheels) 
		{
			if(!skipZeroIndex)
			{
				wheel.Rotate(direction, speed);
			}
			skipZeroIndex = false;
		}
	}
	/* STACK OVERFOW
	public void MoveParticles(ParticleSystem system, Vector3 direction)
	{
		ParticleSystem.Particle[] p = new ParticleSystem.Particle[system.particleCount+1];
		int l = system.GetParticles(p);
		 
		int i = 0;
		while (i < l) 
		{
			p[i].velocity = direction;
		}
		 system.SetParticles(p, l);
	}
	
	public void EmmitSmoke(bool direction, bool left, bool right, float speed)
	{
		if(speed > 0)
		{
			//Forward and Backward
			if(!left && !right)
			{
				if(direction)
				{
					speed /= maxSpeed;
					
					MoveParticles(smokeExhaustLeft, new Vector3(0, 30,-50 * speed));
					MoveParticles(smokeExhaustRight, new Vector3(0, 30,-50 * speed));
					
				}
				else
				{
					speed /= maxSpeedBackwards;
					MoveParticles(smokeExhaustLeft, new Vector3(0, 30, 20 * speed));
					MoveParticles(smokeExhaustRight, new Vector3(0, 30, 20 * speed));
				}
			}
			speed /= rotationSpeed;
			if(left)
			{
				MoveParticles(smokeExhaustLeft, new Vector3(20 * speed, 30, 0));
				MoveParticles(smokeExhaustRight, new Vector3(20 * speed, 30, 0));
			}
			if(right)
			{
				MoveParticles(smokeExhaustLeft, new Vector3(-20 * speed, 30, 0));
				MoveParticles(smokeExhaustRight, new Vector3(-20 * speed, 30, 0));
			}
		}
		else
		{
			ResetSmokeEmmiters();
		}
	}
	*/
	
	public void ResetSmokeEmmiters()
	{
		smokeExhaustLeft = smokeExhaustLeftSave;
		smokeExhaustRight = smokeExhaustRightSave;
	}
	
	public void ThrowDirt(bool direction, float speed, bool left, bool right)
	{
		if(speed > 1)
		{
			if(direction)
			{
				//Debug.Log ("forward");
				dirtBackwardLeft.Stop();
				dirtBackwardRight.Stop();
				
				speed /= maxSpeed;
				
				if(!dirtForwardLeft.isPlaying)
				{
					dirtForwardLeft.Play();
				}
				if(!dirtForwardRight.isPlaying)
				{
					dirtForwardRight.Play();
				}
				dirtForwardLeft.startSpeed = 5 * speed;
				dirtForwardRight.startSpeed = 5 * speed;
			}
			else
			{
				//Debug.Log ("backward");
				dirtForwardLeft.Stop();
				dirtForwardRight.Stop();
					
				speed /= maxSpeedBackwards;
				
				if(!dirtBackwardLeft.isPlaying)
				{
					dirtBackwardLeft.Play();
				}
				if(!dirtBackwardRight.isPlaying)
				{
					dirtBackwardRight.Play();
				}
				dirtBackwardLeft.startSpeed = 5 * speed;
				dirtBackwardRight.startSpeed = 5 * speed;
			}
		}
		//It gets to these scenarious but does nothing.......
		else if(left)
		{
			//Debug.Log("LEFT");
			dirtForwardLeft.Stop();
			dirtBackwardRight.Stop();
			
			if(!dirtBackwardLeft.isPlaying)
			{
				dirtBackwardLeft.Play();
			}
			if(!dirtForwardRight.isPlaying)
			{
				dirtForwardRight.Play();
			}
			dirtBackwardLeft.startSpeed = (float)(5 * (float)(12/40));
			dirtForwardRight.startSpeed = (float)(5 * (float)(12/40));
		}
		else if(right)
		{
			//Debug.Log("RIGHT");
			dirtBackwardLeft.Stop();
			dirtForwardRight.Stop();
				
			if(!dirtForwardLeft.isPlaying)
			{
				dirtForwardLeft.Play();
			}
			if(!dirtBackwardRight.isPlaying)
			{
				dirtBackwardRight.Play();
			}
			dirtForwardLeft.startSpeed = (float)(5 * (float)(12/40));
			dirtBackwardRight.startSpeed = (float)(5 * (float)(12/40));
		}
		// End of comment
		else
		{
			//Debug.Log ("Nothing");
			if(dirtBackwardLeft.isPlaying)
			{
				dirtBackwardLeft.Stop();
			}
			if(dirtBackwardRight.isPlaying)
			{
				dirtBackwardRight.Stop();
			}
			if(dirtForwardLeft.isPlaying)
			{
				dirtForwardLeft.Stop();
			}
			if(dirtForwardRight.isPlaying)
			{
				dirtForwardRight.Stop();
			}
		}
	}
	
	public void SpinWheelsWithTracks(bool direction, bool left, bool right)
	{
		float spinningSpeed = rotationSpeed / 40;
		float currentForwardSpeed = maxSpeed - substractSpeed;
		float currentBackwardSpeed = maxSpeedBackwards - backwardSubstractSpeed;
		//NotMoving
		if(currentForwardSpeed == 0 && currentBackwardSpeed == 0)
		{
			if (left)
			{
				SpinWheels(Vector3.left, (float) spinningSpeed  * 2 , trackWheelsLeft);
				SpinWheels(Vector3.right, (float) spinningSpeed  * 2, trackWheelsRight);
			}
			else if(right)
			{
				SpinWheels(Vector3.right, (float) spinningSpeed  * 2, trackWheelsLeft);
				SpinWheels(Vector3.left, (float) spinningSpeed * 2, trackWheelsRight);
			}
		}
		else
		{
			if(!left && ! right)
			{
				//Forward
				if(directionForward)
				{
					SpinWheels(Vector3.right, ((currentForwardSpeed) * spinningSpeed / 2), trackWheelsLeft);
					SpinWheels(Vector3.right, ((currentForwardSpeed) * spinningSpeed / 2), trackWheelsRight);
				}
				//Backward
				else
				{
					SpinWheels(Vector3.left, ((currentBackwardSpeed) * spinningSpeed / 2), trackWheelsLeft);
					SpinWheels(Vector3.left, ((currentBackwardSpeed) * spinningSpeed / 2), trackWheelsRight);
				}
			}
			else
			{
				// LeftForward
				if(left && direction)
				{
					SpinWheels(Vector3.right, ((currentForwardSpeed) * spinningSpeed / 8), trackWheelsLeft);
					SpinWheels(Vector3.right, ((currentForwardSpeed) * spinningSpeed / 4), trackWheelsRight);
				}
				// LeftBackward
				else if(left && !direction)
				{
					SpinWheels(Vector3.right, ((currentBackwardSpeed) * spinningSpeed / 4), trackWheelsLeft);  // Too fast NO MATTER WHAT
					SpinWheels(Vector3.right, ((currentBackwardSpeed) * spinningSpeed / 8), trackWheelsRight);
				}
				// RightForward
				if(right && direction)
				{
					SpinWheels(Vector3.left, ((currentForwardSpeed) * spinningSpeed / 8), trackWheelsRight);
					SpinWheels(Vector3.left, ((currentForwardSpeed) * spinningSpeed / 4), trackWheelsLeft);
				}
				// RightBackward
				else if(right && !direction)
				{
					SpinWheels(Vector3.left, ((currentBackwardSpeed) * spinningSpeed / 4), trackWheelsRight);
					SpinWheels(Vector3.left, ((currentBackwardSpeed) * spinningSpeed / 8), trackWheelsLeft); // Too slow NO MATTER WHAT
				}
			}
		}
	}
	
	public void SpinTracks(bool direction, bool left, bool right)
	{
		float leftR = 0, rightR = 0; //left right rotation speed bonus
		if(left)
		{
			leftR = 0.01f;
			rightR= - 0.01f;
		}
		if(right)
		{
			leftR = - 0.01f;
			rightR= 0.01f;
		}
		float offset = 0.01f * (maxSpeedBackwards - backwardSubstractSpeed);
		if(direction) // Forward
		{
			offset = - 0.01f  * (maxSpeed - substractSpeed)/7;
		}
		else 
		{
			offset = + 0.01f; // Backward
		}
		if(maxSpeed - substractSpeed == 0 && maxSpeedBackwards - backwardSubstractSpeed == 0)
		{
			offset = 0; // NotMoving
		}
		
		Vector2 currentOffsetR = trackRight.renderer.material.mainTextureOffset;
		Vector2 currentOffsetL = trackLeft.renderer.material.mainTextureOffset;
		
		currentOffsetR.x += offset + rightR;
		currentOffsetL.x += offset + leftR;
		
		trackRight.renderer.material.mainTextureOffset = new Vector2(currentOffsetR.x, 0);
		trackLeft.renderer.material.mainTextureOffset = new Vector2(currentOffsetL.x, 0);
	}
}
