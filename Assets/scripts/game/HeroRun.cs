using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public struct MyPoint{
	static double esp = 1e-6;
	public int x;
	public int z;
	double x_edge;
	double z_edge;
	public MyPoint(int xx = 0, int zz = 0, double xx_edge = 2.5, double zz_edge = 2.5){
		x = xx;
		z = zz;
		x_edge = xx_edge;
		z_edge = zz_edge;
	}
	public void Set(MyPoint t){
		x = t.x;
		z = t.z;
		x_edge = t.x_edge;
		z_edge = t.z_edge;
	}
	public void Set(int xx, int zz, int xx_edge, int zz_edge){
		x = xx;
		z = zz;
		x_edge = xx_edge;
		z_edge = zz_edge;
	}
	public void Updata(int dx = 0, int dz = 0, double dx_edge = 0, double dz_edge = 0){
		x += dx;
		z += dz;
		x_edge += dx_edge;
		z_edge += dz_edge;
	}
	public bool IsInPoint (double hx, double hz){
		if (hx < x + x_edge + esp && hx > x - x_edge - esp && hz < z + z_edge + esp && hz > z - z_edge - esp)
			return true;
		else
			return false;
	}
}
public class HeroRun : MonoBehaviour {
	const double spee_default = 10;
	const int rotation_splece = 5;

	//	int[] map;
	MapType[] map;
	int mapIndex = 0;
	int mapSize = 0;
	int resurrectionIndex = 0;
	int tmpScore;
	int nowScore = 0;
	int lastScore = 0;
	int continueCount = 3;
	MyPoint resurrectionPoint;

	Queue<GameObject> groundQueue;
	Queue<GameObject> scoreQueue;

	GameObject myCamera;
	UI_gameSystem ui_gameSystem;

	double now_x = 0;
//	double now_y = 0;
	double now_z = 0;

	MyPoint[] ground = new MyPoint[2];
	int max_x,min_x,max_z,min_z;
	int ground_index;
	int dx,dz;
	double speed = spee_default;
	int rotation_count;
	int tmpRo;
	int isalive = 1;

	Transform scoreTransform; 
	public void Initial(){
		map = GlobalMap.GetMap ();
		mapSize = GlobalMap.GetMapSize ();
		mapIndex = 0;
		resurrectionIndex = 0;
		GlobalMap.ChangeDxy (map [mapIndex].dxDz, out dx, out dz);
		GlobalMap.SetWinFlag (0);
		transform.position = new Vector3 (0, 0, 0);
		now_x = transform.position.x;
		//now_y = transform.position.y;
		now_z = transform.position.z;
		ground_index = 0;
		resurrectionPoint = new MyPoint ((int)now_x, (int)now_z,2.5,2.5);
		ground[ground_index] = new MyPoint ((int)now_x, (int)now_z,2.5,2.5);
		ground[1-ground_index] = new MyPoint ((int)now_x+dx, (int)now_z+dz,2.5,2.5);
		rotation_count = 0;
		isalive = 1;
		speed = spee_default;
		max_x = min_x = (int)now_x;
		max_z = min_z = (int)now_z;
		nowScore = 0;
		lastScore = 0;
		continueCount = 3;
		ui_gameSystem.ContinueTextUpdate (continueCount);
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
	}
	// Use this for initialization
	void Awake(){
		myCamera = (GameObject)GameObject.Find ("Camera").gameObject;

		ui_gameSystem = (UI_gameSystem)GameObject.Find ("UI_gameSystem").GetComponent<UI_gameSystem> ();
	}
	void Start () {
		groundQueue = ((MapBuilder)GameObject.Find ("mapBuilder").GetComponent<MapBuilder> ()).GetGroundQueue();

		scoreQueue = ((MapBuilder)GameObject.Find ("mapBuilder").GetComponent<MapBuilder> ()).GetScoreQueue();
		//ChangeDxy ();
		Initial();
	}
	void OnGUI()
	{
	}

	// Update is called once per frame
	void Update () {

		now_x = transform.position.x;
		now_z = transform.position.z;


		if (isalive == 1 &&!ground [ground_index].IsInPoint (now_x, now_z)) {
			if (!ground [1 - ground_index].IsInPoint (now_x, now_z)) {
				speed = 0;
				isalive = 0;
				ui_gameSystem.ActiveContinuePanel (true);
			} else {
				if (mapIndex == mapSize - 1 && !GlobalMap.GetWinFlag ()) {
					speed = 0;
					isalive = 0;
					myCamera.transform.position = new Vector3 ((min_x + max_x) >> 1, 300, (min_z + max_z) >> 1);
					myCamera.transform.rotation = Quaternion.Euler (new Vector3 (90, 0, 0));
					GlobalMap.SetWinFlag (1);
					ui_gameSystem.ActiveEndPanel (true);
					//Debug.Log ("ENDENDEND");
				} else {
					if (map [mapIndex].materialType == 2) {
						resurrectionIndex = mapIndex;
						resurrectionPoint.Set (ground [ground_index]);
						lastScore = nowScore;
					}
					if (now_x > max_x)
						max_x = (int)now_x;
					if (now_x < min_x)
						min_x = (int)now_x;
					if (now_z > max_z)
						max_z = (int)now_z;
					if (now_z < min_z)
						min_z = (int)now_z;
					mapIndex++;
					GlobalMap.ChangeDxy (map [mapIndex].dxDz, out dx, out dz);
					//ChangeDxy ();
					//Debug.Log ("dx:"+dx+" dz:"+dz);
					ground_index = 1 - ground_index;
					ground [1 - ground_index].Set (ground [ground_index]);
					ground [1 - ground_index].Updata (dx, dz);
					//Debug.Log ("dx:"+dx+" dz:"+dz);
					//Debug.Log ("now_x:"+ now_x+" now_z:"+ now_z);
					//Debug.Log ("x0:"+ground [ground_index].x+" z0:"+ground [ground_index].z+" x1:"+ground [1 - ground_index].x+"  z1:"+ground [1 - ground_index].z);
					speed += 0.1;
					nowScore += 10;
					tmpScore = nowScore - 5;
				}
			}
		} else {
			if (dx != 0) {
				tmpScore = nowScore + (int)((transform.position.x - ground [ground_index].x) * 10 / dx);
			} else {
				tmpScore = nowScore + (int)((transform.position.z - ground [ground_index].z) * 10 / dz);
			}
		}
		ui_gameSystem.ScoreTextUpdate (tmpScore);
		if (scoreQueue.Count > 0){
			scoreTransform = scoreQueue.Peek ().transform;
			if (GlobalMap.ReachTest (scoreTransform, this.transform)) {
				speed = spee_default;
				Destroy (scoreQueue.Peek ());
				//Debug.Log ("this:"+this.transform.position.x+" " + this.transform.position.z);
				//Debug.Log ("score:"+scoreTransform.position.x+" " + scoreTransform.position.z);
				//Debug.Log ("dx:"+dx+" dz:"+dz);
				Debug.Log ("reach");
				scoreQueue.Dequeue ();
			}else if (GlobalMap.OverTest(this.transform, scoreTransform, dx, dz)) {
				//Debug.Log ("this:"+this.transform.position.x+" " + this.transform.position.z);
				//Debug.Log ("score:"+this.scoreTransform.position.x+" " + this.scoreTransform.position.z);
				//Debug.Log ("dx:"+dx+" dz:"+dz);
				Debug.Log ("over");
				Destroy (scoreQueue.Peek (),2);
				scoreQueue.Dequeue ();
			}
		}
		
		if (!GlobalMap.GetWinFlag()) {
			transform.Translate (Vector3.forward * (float)speed * Time.deltaTime, Space.Self);
		}
		while (groundQueue.Count > 0 && ground [ground_index].IsInPoint (groundQueue.Peek ().transform.position.x, groundQueue.Peek ().transform.position.z)) {
			groundQueue.Dequeue ();
		}

	}
	void FixedUpdate()
	{
		if (rotation_count < 0) {
			rotation_count++;
			transform.Rotate (0,(float)90/rotation_splece, 0);
		} else if (rotation_count > 0) {
			rotation_count--;
			transform.Rotate (0, (float)-90/rotation_splece, 0);
		}
	}
	public void OnLeft(){
		if (isalive == 1) 
				rotation_count += rotation_splece;
	}
	public void OnRight(){
		if (isalive == 1) 
			rotation_count -= rotation_splece;
	}
	public bool onContinue(){
		int ddx, ddz, ans;
		if (continueCount > 0) {
			continueCount--;
			ui_gameSystem.ContinueTextUpdate (continueCount);
		}
		else
			return false;
		rotation_count = 0;
		mapIndex = resurrectionIndex;
		GlobalMap.ChangeDxy (map [mapIndex].dxDz, out dx, out dz);
		ground [ground_index].Set (resurrectionPoint);
		ground [1 - ground_index].Set (resurrectionPoint);
		ground [1 - ground_index].Updata (dx, dz);
		GlobalMap.ChangeDxy (map [mapIndex + 1].dxDz, out ddx, out ddz);
		//transform.position.Set(ground [ground_index].x,0,ground [ground_index].z);
		transform.position = new Vector3 (ground [ground_index].x, 0, ground [ground_index].z);
		//transform.rotation = Quaternion.AngleAxis((dx-dz)/5*90, new Vector3(0,1,0));
		if (ddz != 0)
			ans = (ddz) / 5 * 90 - 90;
		else
			ans = 0;
		if (ddx != 0)
			ans += (ddx) / 5 * 90;
		transform.rotation = Quaternion.Euler (new Vector3 (0, ans, 0));
		Debug.Log ("dx:" + ddx + " dz:" + ddz);
		//speed = spee_default;
		isalive = 1;
		nowScore = lastScore;
		speed = spee_default;
		return true;
	}
	public void EndlessUpdateMap(int select){
		GlobalMap.EndlessUpdateMap (select, ref mapSize, ref map);
	}
}


