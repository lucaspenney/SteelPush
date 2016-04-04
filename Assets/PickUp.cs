using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
	Camera mainCamera;
	bool carrying;
	GameObject carriedObject;
	public float distance;
	public float smooth;
	public Vector3 rotationDegs;
	// Use this for initialization
	void Start() {
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update() {
		if(carrying) {
			carry(carriedObject);
			checkDrop();
			scroll();
			stopRotate();
		} else {
			pickup();
		}
	}

	void stopRotate() {
		if(Input.GetMouseButton(1)) {
			Vector3 vel = new Vector3(0, 0, 0);
			carriedObject.GetComponent<Rigidbody>().angularVelocity = vel;
		}
	}

	void carry(GameObject o) {
		Vector3 pos = mainCamera.transform.position;
		Vector3 targetPos = pos + Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, -0.1f)).direction * distance;
		Vector3 posDiff = targetPos - o.GetComponentInChildren<Collider>().bounds.center;

		Vector3 newVel = posDiff * 10f;
		newVel += GameObject.Find("Player").GetComponent<Rigidbody>().velocity;
		o.GetComponent<Rigidbody>().velocity = newVel;
	}

	void pickup() {
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, -0.1f));
		Debug.DrawRay(Camera.main.transform.position, ray.direction * 10);
		if(Input.GetKeyDown(KeyCode.E)) {
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.transform.position, ray.direction, out hit)) {
				Rigidbody p = hit.collider.gameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if(p != null && !p.gameObject.GetComponent<Rigidbody>().isKinematic) {
					Vector3 myPos = GameObject.Find("Player").transform.position;
					distance = (myPos - p.gameObject.transform.position).magnitude;
					rotationDegs = p.gameObject.transform.rotation.eulerAngles;
					carrying = true;
					carriedObject = p.gameObject;
					//p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
					p.gameObject.GetComponent<Rigidbody>().useGravity = false;
					Physics.IgnoreCollision(
						GameObject.Find("Player").GetComponent<Collider>(), 
						p.gameObject.GetComponentInChildren<Collider>()
					);
				}
			}
		}
	}

	void scroll() {
		Rigidbody r = carriedObject.GetComponent<Rigidbody>();
		float val = 0;
		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			val = -0.1f;
		} else if(Input.GetAxis("Mouse ScrollWheel") > 0) {
			val = 0.1f;
		}
		if(val != 0) {
			if(Input.GetKey(KeyCode.LeftShift)) {
				r.angularVelocity = new Vector3(r.angularVelocity.x + val, r.angularVelocity.y, r.angularVelocity.z);
			} else if(Input.GetKey(KeyCode.LeftControl)) {
				r.angularVelocity = new Vector3(r.angularVelocity.x, r.angularVelocity.y + val, r.angularVelocity.z + val);
			} else if(Input.GetKey(KeyCode.LeftAlt)) {
				r.angularVelocity = new Vector3(r.angularVelocity.x, r.angularVelocity.y, r.angularVelocity.z + val);
			} else {
				distance += val;
			}
		}
	}

	void checkDrop() {
		if(Input.GetKeyDown(KeyCode.E)) {
			dropObject();
		}
	}

	void dropObject() {
		Physics.IgnoreCollision(
			GameObject.Find("Player").GetComponent<Collider>(), 
			carriedObject.GetComponentInChildren<Collider>(),
			false
		);
		carrying = false;
		//carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
		//carriedObject.gameObject.GetComponent<Rigidbody>().velocity = GameObject.Find("Player").GetComponent<Rigidbody>().velocity;
		carriedObject = null;
	}
}
