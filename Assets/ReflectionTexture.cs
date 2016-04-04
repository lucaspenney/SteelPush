using UnityEngine;
using System.Collections;

public class ReflectionTexture : MonoBehaviour {

	public Camera reflectionCamera;
	private Quaternion startAngles;

	// Use this for initialization
	void Start() {
		startAngles = reflectionCamera.transform.rotation;
	}
	
	// Update is called once per frame
	void Update() {
		Vector3 diff = reflectionCamera.transform.position - Camera.main.transform.position;
		//diff.x = 0;
		Vector3 lookPos = reflectionCamera.transform.position;
		diff.x *= -1;
		diff.y *= -1;
		lookPos += diff;

		Debug.DrawLine(reflectionCamera.transform.position, lookPos);
		reflectionCamera.transform.LookAt(lookPos);

		reflectionCamera.transform.Rotate(new Vector3(0f, 0f, 0f));

	}
}
