using UnityEngine;
using System.Collections;

public class spacecraft : MonoBehaviour {

	public GameObject OrientationReference;
	public GameObject EngineFWDSound;
	public GameObject EngineREVSound;
	public OVRCameraController CameraController;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {

		float joyX = Input.GetAxis ("Joy0X") * 2;
		float joyY = Input.GetAxis ("Joy0Y") * 2;
		float joyZ = Input.GetAxis ("Joy0Z") * 2;

		if (Input.GetButton("Accelerate")){
			rigidbody.AddRelativeForce(new Vector3(0, 0, 0.50f));
			EngineFWDSound.audio.mute = false;

		}else{
			EngineFWDSound.audio.mute = true;
		}

		if (Input.GetButton("Brake")){
			rigidbody.velocity = rigidbody.velocity * 0.99f;
			EngineREVSound.audio.mute = false;
		}else{
			EngineREVSound.audio.mute = true;
		}

		if (Input.GetButton("ResetOculus")){
			OVRDevice.ResetOrientation(0);
		}

		
		if (Input.GetButton("Escape")){
			Application.Quit();
		}

		if (Input.GetButton("ViewDistance+")){
			CameraController.FarClipPlane += 0.5f;
		}

		if (Input.GetButton("ViewDistance-")){
			CameraController.FarClipPlane -= 0.5f;
		}

		rigidbody.AddRelativeTorque(new Vector3(0, 0, -joyX));
		rigidbody.AddRelativeTorque(new Vector3(-joyY, 0, 0));
		rigidbody.AddRelativeTorque(new Vector3(0, joyZ, 0));

		if (Input.GetKey(KeyCode.Space)){
			rigidbody.AddRelativeForce(new Vector3(0, 0, 0.50f));
		}

		if (Input.GetKey(KeyCode.LeftAlt)){
			OVRDevice.ResetOrientation(0);
		}


		float bufferZone = Config.getF("BufferZone");

		float min = 0 - bufferZone;
		float max = Config.getF("WorldSize") + bufferZone;
		Vector3 position = rigidbody.transform.position;

		if (position.x > max) position.x = min;
		if (position.x < min) position.x = max;

		if (position.y > max) position.y = min;
		if (position.y < min) position.y = max;

		if (position.z > max) position.z = min;
		if (position.z < min) position.z = max;

		rigidbody.position = position;


		Vector3 velocity = rigidbody.velocity;
		Vector3 orientationVelocity = OrientationReference.transform.position - transform.position;
		orientationVelocity = orientationVelocity.normalized * velocity.magnitude;

		velocity = velocity * 0.99f + orientationVelocity * 0.01f;
		rigidbody.velocity = velocity;








		
	}
}
