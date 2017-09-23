using UnityEngine;
using System.Collections;

public class SteelPush : MonoBehaviour {

	public GameObject controller;

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		if(Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, -0.1f));
			float dist = 100f;
			float radius = 0.05f;

			RaycastHit[] hits;
			hits = Physics.SphereCastAll(ray, radius, dist);
			for(int i = 0;i < hits.Length;i++) {
				Rigidbody r = hits[i].collider.gameObject.transform.root.GetComponentInChildren<Rigidbody>();
				if(r != null) {
					if(Input.GetMouseButton(0)) {
						r.AddForceAtPosition(Camera.main.transform.forward * 50, hits[i].point);
					}
					if(Input.GetMouseButton(1)) {
						r.AddForceAtPosition(Camera.main.transform.forward * -50, hits[i].point);
					}
				}	
			}
		}
	}
}
