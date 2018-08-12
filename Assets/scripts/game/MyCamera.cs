using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour {

	public Transform target;
	public HeroRun hero;
	int dz;
	int dx;
	//SphereRun tmp;
	// Use this for initialization
	void Start () {
		target = GameObject.Find ("hero").transform;
		hero = (HeroRun)GameObject.Find ("hero").GetComponent<HeroRun> ();
		transform.position = target.position - target.transform.rotation * Vector3.forward * 20 + target.transform.rotation * Vector3.up * 20;
		//transform.LookAt (target, Vector3.up);
		//transform.rotation = Quaternion.AngleAxis(40,new Vector3(1,0,0));
		transform.Rotate (40, 0, 0);
	}

	// Update is called once per frame
	void Update () {

	}

	// Update is called once per frame
	void LateUpdate () {
		//tmp = GetComponent<SphereRun> ();
		if (!GlobalMap.GetWinFlag()) {
			transform.position = target.position - target.transform.rotation * Vector3.forward * 20 + target.transform.rotation * Vector3.up * 20; 
			transform.rotation = target.rotation;
			transform.Rotate (40, 0, 0);
			//transform.position = target.position -  Vector3.forward * 20 +  Vector3.up * 20; 
		} else {
		}
	}
}
