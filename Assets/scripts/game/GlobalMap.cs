using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class MapType{
	public int dxDz;//0:0,1  1: 0,-1   2:1,0   3:-1,0
	public int cubeType;//0:cube  1: cylinder
	public int materialType; //0：normal  1: start/end  2: resurrection
	public int rewardType; // 0:None   1:slow
	public MapType(){
	}
	public MapType(int dxdz, int cube, int mat, int rew){
		dxDz = dxdz;
		cubeType = cube;
		materialType = mat;
		rewardType = rew;
	}
}
public static class GlobalMap{
	private static int mapSize;
	private static MapType[] mapType = new MapType[0];

	private static int winFlag = 0;

	private static float esp = 1e-6f;
	private static float edge = 2.5f;
	private static float dis_splice = 1f;

	public static MapType[] GetMap(){
		return mapType;
	}
	public static int GetMapSize(){
		return mapSize;
	}
	public static bool GetWinFlag(){
		return (winFlag == 1);
	}
	public static void SetWinFlag(int f){
		winFlag = f;
	}
	public static bool ReachTest(Transform t1, Transform t2){
		float x = t1.position.x - t2.position.x;
		float z = t1.position.z - t2.position.z;
		float dis = t1.lossyScale.x * t1.lossyScale.z + t2.lossyScale.x * t2.lossyScale.z;
		if (x * x + z * z < dis / dis_splice) {
			return true;
		}
		else
			return false;
	}
	public static bool OverTest(Transform t1, Transform t2, int dx, int dz){
		if (dz == 0 && dx > 0) {
			if (t1.position.z - t2.position.z > -1 * edge - esp && t1.position.z - t2.position.z < edge + esp) {
				if (t1.position.x > t2.position.x + dx/2)
					return true;
				else
					return false;
			} else
				return false;
		} else if (dz == 0 && dx < 0) {
			if (t1.position.z - t2.position.z > -1 * edge - esp && t1.position.z - t2.position.z < edge + esp) {
				if (t1.position.x < t2.position.x + dx/2)
					return true;
				else
					return false;
			} else
				return false;
		} else if (dx == 0 && dz > 0) {
			if (t1.position.x - t2.position.x > -1 * edge - esp && t1.position.x - t2.position.x < edge + esp) {
				if (t1.position.z > t2.position.z + dz/2)
					return true;
				else
					return false;
			} else
				return false;
		} else if (dx == 0 && dz < 0) {
			if (t1.position.x - t2.position.x > -1 * edge - esp && t1.position.x - t2.position.x < edge + esp) {
				if (t1.position.z < t2.position.z + dz/2)
					return true;
				else
					return false;
			} else
				return false;
		}
		return false;
	}
	public static void ChangeDxy(int map_value, out int dx, out int dz){
		dx = 0;
		dz = 0;
		switch (map_value) {
		case 0:
			dx = 0; 
			dz = 5;
			break;
		case 1:
			dx = 0;
			dz = -5;
			break;
		case 2:
			dx = 5;
			dz = 0;
			break;
		case 3:
			dx = -5;
			dz = 0;
			break;
		default: 
			break;
		}
	}
	public static void Initial(){
		winFlag = 0;

	}
	public static void EndlessUpdateMap(int select, ref int mapSize, ref MapType[] map){
		string content;
		string[] split;
		char[] split_word = {'\n'};
		int i;
		MapType[] newMap;
		int newSize;
		content = Resources.Load<TextAsset> ("Map/wujin" + select.ToString ()).text;
		Debug.Log (content);
		split = content.Split (split_word);
		newSize = mapSize + split.Length;
		//Debug.Log (mapSize);

		newMap = new MapType[newSize];
		for (i = 0; i < mapSize; ++i) {
			newMap [i] = new MapType (map [i].dxDz, map [i].cubeType, map [i].materialType, map [i].rewardType);
		}
		for (i = 0; i < split.Length; ++i) {
			//Debug.Log (split[i]);
			newMap [i + mapSize] = new MapType (split [i] [0] - '0', split [i] [1] - '0', split [i] [2] - '0', split [i] [3] - '0');
		}
		map = newMap;
		mapSize = newSize;
	}
	public static void UpdateMap(){
		string content;
		string[] split;
		char[] split_word = {'\n'};
		int i;
		if (CYWGlobal.gameMode == 1) {
			content = Resources.Load<TextAsset> ("Map/map" + CYWGlobal.gameSelect.ToString ()).text;
			Debug.Log (content);
			split = content.Split (split_word);
			mapSize = split.Length;
			//Debug.Log (mapSize);

			mapType = new MapType[mapSize];
			for (i = 0; i < mapSize; ++i) {
				//Debug.Log (split[i]);
				mapType [i] = new MapType (split [i] [0] - '0', split [i] [1] - '0', split [i] [2] - '0', split [i] [3] - '0');
			}
		} else {
			int tmp = Random.Range (1, 4);
			content = Resources.Load<TextAsset> ("Map/wujin" + tmp.ToString ()).text;
			Debug.Log (content);
			split = content.Split (split_word);
			mapSize = split.Length;
			//Debug.Log (mapSize);

			mapType = new MapType[mapSize];
			for (i = 0; i < mapSize; ++i) {
				//Debug.Log (split[i]);
				mapType [i] = new MapType (split [i] [0] - '0', split [i] [1] - '0', split [i] [2] - '0', split [i] [3] - '0');
			}
		}
	}

}
	