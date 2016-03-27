using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
	Camera mainCamera;
	bool carrying;
	GameObject carriedObject;
	public float distance;
	public float smooth;
	// Use this for initialization
	void Start() {
		mainCamera = Camera.main;
		print(mainCamera);
	}
	
	// Update is called once per frame
	void Update() {
		if(carrying) {
			carry(carriedObject);
			checkDrop();
			//rotateObject();
		} else {
			pickup();
		}
	}

	void rotateObject() {
		carriedObject.transform.Rotate(5, 10, 15);
	}

	void carry(GameObject o) {
		Vector3 pos = GameObject.Find("Player").transform.position;
		o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + (mainCamera.transform.forward * -1) * 1, Time.deltaTime * smooth);
		o.transform.position = pos + Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, -1f)).direction * distance;
		//o.transform.position += new Vector3(0f, -0.5f, 0f);
		//o.transform.rotation = Quaternion.identity;
	}

	void pickup() {
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, -1f));
		Debug.DrawRay(Camera.main.transform.position, ray.direction * 10);
		if(Input.GetKeyDown(KeyCode.E)) {
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.transform.position, ray.direction, out hit)) {
				print(hit.collider);
				Rigidbody p = hit.collider.gameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if(p != null) {
					carrying = true;
					carriedObject = p.gameObject;
					p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
					p.gameObject.GetComponent<Rigidbody>().useGravity = false;
					Physics.IgnoreCollision(
						GameObject.Find("Player").GetComponent<Collider>(), 
						p.gameObject.GetComponentInChildren<Collider>()
					);
				}
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
		carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
		carriedObject.gameObject.GetComponent<Rigidbody>().velocity = GameObject.Find("Player").GetComponent<Rigidbody>().velocity;
		carriedObject = null;
	}
}
