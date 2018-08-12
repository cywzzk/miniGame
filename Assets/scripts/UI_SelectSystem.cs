using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UI_SelectSystem : MonoBehaviour {
	private GameObject[] buttonList;
	private Text[] textList;
	private GameObject	buttonPre;
	private GameObject	buttonNext;
	private int nowPage;
	private int allPage;
	private int allGame;
	// Use this for initialization
	void Awake(){
		GameObject tmp = GameObject.Find ("Canvas").transform.FindChild ("gameModePanel").gameObject;
		buttonList = new GameObject[6];
		textList = new Text[6];
		for (int i = 1; i <= 6; ++i) {
			buttonList [i - 1] = tmp.transform.FindChild ("game" + i.ToString ()).gameObject;
			textList [i - 1] = buttonList [i - 1].transform.FindChild ("Text").GetComponent<Text> ();
		}
		buttonPre = tmp.transform.FindChild ("previous").gameObject;
		buttonNext = tmp.transform.FindChild ("next").gameObject;
	}
	void ButtonVisualOrNot(){
		for (int i = 0; i < 6; ++i) {
			if (allGame - 6 * nowPage - 1 >= i) {
				buttonList [i].gameObject.SetActive (true);
				textList [i].text = (6 * nowPage + i + 1).ToString ();
			}else buttonList [i].gameObject.SetActive (false);
		}
		if (nowPage != 0) {
			buttonPre.gameObject.SetActive (true);
		} else {
			buttonPre.gameObject.SetActive (false);
		}
		if (nowPage != allPage) {
			buttonNext.gameObject.SetActive (true);
		} else {
			buttonNext.gameObject.SetActive (false);
		}
	}
	void Start () {
		allGame = CYWGlobal.gameAll;
		allPage = (int)((allGame - 1) / 6);
		nowPage = 0;
		ButtonVisualOrNot ();
	}
	public void OnGame1(){
		CYWGlobal.gameSelect = 6 * nowPage + 1;
		CYWGlobal.gameMode = 1;
		SceneManager.LoadScene ("load");
	}
	public void OnGame2(){
		CYWGlobal.gameSelect = 6 * nowPage + 2;
		CYWGlobal.gameMode = 1;
		SceneManager.LoadScene ("load");
	}
	public void OnGame3(){
		CYWGlobal.gameSelect = 6 * nowPage + 3;
		CYWGlobal.gameMode = 1;
		SceneManager.LoadScene ("load");
	}
	public void OnGame4(){
		CYWGlobal.gameSelect = 6 * nowPage + 4;
		CYWGlobal.gameMode = 1;
		SceneManager.LoadScene ("load");
	}
	public void OnGame5(){
		CYWGlobal.gameSelect = 6 * nowPage + 5;
		CYWGlobal.gameMode = 1;
		SceneManager.LoadScene ("load");
	}
	public void OnGame6(){
		CYWGlobal.gameSelect = 6 * nowPage + 6;
		CYWGlobal.gameMode = 1;
		SceneManager.LoadScene ("load");
	}
	public void OnButtonPre(){
		if (nowPage != 0)
			nowPage--;
		ButtonVisualOrNot ();
	}
	public void OnButtonNext(){
		if (nowPage != allPage)
			nowPage++;
		ButtonVisualOrNot ();
	}
	public void OnEndless(){
		CYWGlobal.gameSelect = 1;
		CYWGlobal.gameMode = 2;
		SceneManager.LoadScene ("load");
	}
	public void OnBack(){
		SceneManager.LoadScene ("login");
	}
	// Update is called once per frame
	void Update () {
	
	}
}
