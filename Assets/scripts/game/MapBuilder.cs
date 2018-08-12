using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapBuilder : MonoBehaviour {
	private Transform hero;
	private int dx = 5;
	private int dz = 5;
	private int now_x = 0;
	private int now_y = 0;
	private int now_z = 0;

	private GameObject[] cloneGround;
	private Queue<GameObject> groundQueue;
	private List<GameObject> groundList;

	private Material[] material; 

	private GameObject cloneScore;
	private Queue<GameObject> scoreQueue;

	GameObject cloneObj;
	int delay = 0;
	//int[] map;

	private MapType[] map;
	private int mapIndex = 0;
	private int mapSize = 0;
	// Use this for initialization
	public Queue<GameObject> GetGroundQueue(){
		return groundQueue;
	}
	public Queue<GameObject> GetScoreQueue(){
		return scoreQueue;
	}
	void CloneObject(){

		cloneObj = Instantiate (cloneGround[map[mapIndex].cubeType]) as GameObject;
		cloneObj.transform.position = new Vector3 (now_x, now_y, now_z);
		//if (map [mapIndex].materialType != 0) {
		cloneObj.GetComponent<Renderer> ().material = material [map [mapIndex].materialType];
		//}
		if (map [mapIndex].dxDz == 0) {		
			cloneObj.transform.Rotate(new Vector3 (0, 90, 0));	
		} else if (map [mapIndex].dxDz == 1) {	
			cloneObj.transform.Rotate(new Vector3 (0, 270, 0));
		} else if (map [mapIndex].dxDz == 2) {	
			cloneObj.transform.Rotate(new Vector3 (0, 180, 0));
		} else if (map [mapIndex].dxDz == 3) {	
			cloneObj.transform.Rotate(new Vector3 (0, 0, 0));
		}
		groundQueue.Enqueue (cloneObj);
		groundList.Add (cloneObj);
		if (map[mapIndex].rewardType != 0) {
			cloneObj = Instantiate (cloneScore) as GameObject;
			cloneObj.transform.position = new Vector3 (now_x + Random.value*4 - 2, (float)(now_y + 2.5), now_z+Random.value*4 - 2);
			scoreQueue.Enqueue (cloneObj);
		}

		GlobalMap.ChangeDxy (map [mapIndex].dxDz,out dx,out dz);
		if (mapIndex < mapSize)
			mapIndex++;
		now_x += dx;
		now_z += dz;
	}
	public void Initial(){
		map = GlobalMap.GetMap ();
		mapSize = GlobalMap.GetMapSize ();
		mapIndex = 0;
		now_x = (int)hero.position.x;
		now_y = (int)hero.position.y - 2;
		now_z = (int)hero.position.z;
		dx = 0;
		dz = 5;
		while (groundQueue.Count > 0) {
			Destroy (groundQueue.Peek ());
			groundQueue.Dequeue ();
		}
		while (scoreQueue.Count > 0) {
			Destroy (scoreQueue.Peek ());
			scoreQueue.Dequeue ();
		}
		for (int i = 0; i < groundList.Count; ++i)
			Destroy (groundList [i]);
		groundList.Clear ();
		GlobalMap.SetWinFlag (0);
	}
	void Awake(){
		groundQueue = new Queue<GameObject>();
		scoreQueue = new Queue<GameObject> ();
		groundList = new List<GameObject> ();
//		Debug.Log (scoreQueue);

		cloneGround = new GameObject[2];
		cloneGround[0] = Resources.Load ("Models/ground_cube") as GameObject;
		cloneGround[1] = Resources.Load ("Models/ground_cylinder") as GameObject;
		cloneScore = Resources.Load ("Models/stone") as GameObject;

		material = new Material[4];
		//material [0] = Resources.Load<Material> ("Materials/prop_asteriod_02_mat");
		//material [0] = Resources.Load <Material>("Materials/1");
		material [0] = Resources.Load<Material>("Materials/acron");
//		Debug.Log (material [0].name);
		material [1] = Resources.Load ("Materials/ground_spark") as Material;
		material [2] = Resources.Load ("Materials/ground_spark") as Material;
		material [3] = Resources.Load ("Materials/ice") as Material;
//		Debug.Log (material [3].name);
		hero = GameObject.Find ("hero").transform;
	}

	void Start () {
		Initial ();
//		Debug.Log ("Initial");
		while (groundQueue.Count < 20 && mapIndex < mapSize) {
			CloneObject ();
		}
	}

	// Update is called once per frame
	void Update () {

		while (groundQueue.Count < 20 && mapIndex < mapSize) {
			CloneObject ();
		}
	}
	void FixedUpdate(){
		int t = 5;
		if (GlobalMap.GetWinFlag() && delay/t < mapSize) {

		//	Debug.Log (delay+" " + GroundMap.winCount);
			if (delay % t != 0) {
				//groundList [delay/t].transform.Translate (Vector3.up * Time.deltaTime * 100, Space.World);
			} else {
				groundList [delay/t].GetComponent<Renderer> ().material = material [3];
			}
			delay++;	
		}
	}

	public bool EndlessUpdateOrNot(){
		if (mapSize - mapIndex < 20)
			return true;
		else
			return false;
	}
	public void EndlessUpdateMap(int select){
		GlobalMap.EndlessUpdateMap (select, ref mapSize, ref map);
	}
}
