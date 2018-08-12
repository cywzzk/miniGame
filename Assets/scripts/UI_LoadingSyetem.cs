using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class UI_LoadingSyetem : MonoBehaviour {
	private AsyncOperation loadAsyn;
	private int toProcess;
	private int nowProcess;
	public Image loadBar;
	public Text loadText;
	// Use this for initialization
	void Awake(){
		toProcess = 0;
		nowProcess = 0;
		loadBar = transform.FindChild ("loadProcess").GetComponent<Image>();
		loadText = (Text)transform.FindChild ("Text").GetComponent<Text>();
	}
	void Start () {
		
		Debug.Log (CYWGlobal.gameSelect);

		GlobalMap.UpdateMap ();

		StartCoroutine (MyLoadScene());

	}

	private IEnumerator MyLoadScene(){
		//loadAsyn = SceneManager.LoadSceneAsync (CYWGlobal.loadScene);
		loadAsyn = SceneManager.LoadSceneAsync (CYWGlobal.loadScene);
		loadAsyn.allowSceneActivation = false;
		while (true) {
			yield return loadAsyn;
		}
	}
	// Update is called once per frame
	void Update () {
		if (loadAsyn.progress < 0.9f)
			toProcess = (int)(100 * loadAsyn.progress);
		else
			toProcess = 100;
		if (nowProcess < toProcess) {
			nowProcess++;
			loadBar.fillAmount = nowProcess / 100f;
			loadText.text = nowProcess.ToString () + "%";
		} else if (toProcess == 100)
			loadAsyn.allowSceneActivation = true;
	}
}
