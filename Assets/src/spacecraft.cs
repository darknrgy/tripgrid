using UnityEngine;
using System.Collections;

public class spacecraft : MonoBehaviour {

	public GameObject OrientationReferenceFWD;
	public GameObject OrientationReferenceLEFT;
	public GameObject OrientationReferenceRIGHT;

	public GameObject EngineFWDSound;
	public GameObject EngineREVSound;
	public GameObject MissileTemplate;
	public OVRCameraRig CameraRig;



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
			OVRManager.display.RecenterPose();
		}

		
		if (Input.GetButton("Escape")){
			Application.Quit();
		}

		if (Input.GetButton("ViewDistance+")){
			//CameraRig.FarClipPlane += 0.5f;
		}

		if (Input.GetButton("ViewDistance-")){
			//CameraRig.FarClipPlane -= 0.5f;
		}

		if (Input.GetButton("FireWeapon")){
			FireWeapon();
		}

		rigidbody.AddRelativeTorque(new Vector3(0, 0, -joyX));
		rigidbody.AddRelativeTorque(new Vector3(-joyY, 0, 0));
		rigidbody.AddRelativeTorque(new Vector3(0, joyZ, 0));

		if (Input.GetKey(KeyCode.Space)){
			rigidbody.AddRelativeForce(new Vector3(0, 0, 0.50f));
		}

		if (Input.GetKey(KeyCode.LeftAlt)){
			OVRManager.display.RecenterPose();

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
		Vector3 orientationReference = OrientationReferenceFWD.transform.position - transform.position;
		orientationReference = orientationReference.normalized * velocity.magnitude;

		velocity = velocity * (1 - Config.getF("Control")) + orientationReference * Config.getF("Control");
		rigidbody.velocity = velocity;

		
	}

	bool parity = true;

	void FireWeapon(){

		if (!CoolDown()) return;

		GameObject side = parity ? OrientationReferenceLEFT : OrientationReferenceRIGHT;
		parity = !parity;

		Vector3 orientationReference = OrientationReferenceFWD.transform.position - transform.position;
		Vector3 velocity = transform.rigidbody.velocity + orientationReference * 10;

		GameObject child = (GameObject) Instantiate(MissileTemplate, side.transform.position, transform.rotation);
		child.rigidbody.velocity = velocity;
		Destroy(child, 3.0f);

	}

	private float CoolDownTimestamp = 0;

	bool CoolDown(){
		if (Time.time - CoolDownTimestamp < 0.1) return false;
		CoolDownTimestamp = Time.time;
		return true;
	}


}
