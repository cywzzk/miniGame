using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UI_gameSystem : MonoBehaviour {
	HeroRun heroRun;
	MapBuilder mapBuilder;
	Text endPanelNextButtonText;
	GameObject playPanel;
	GameObject continuePanel;
	GameObject endPanel;
	GameObject playCanvas;

	Text scoreText;
	Text continueText;
	Text continueButtonText;
	Text continueScoreText;
    Text rewardScoreText;

    WWW www;
	int nowPoint;
	// Use this for initialization
	void Awake(){
		heroRun = (HeroRun)GameObject.Find ("hero").GetComponent<HeroRun> ();
		mapBuilder = (MapBuilder)GameObject.Find ("mapBuilder").GetComponent<MapBuilder> ();
		endPanelNextButtonText= (Text)GameObject.Find("nextButton").transform.FindChild("Text").GetComponent<Text>();

		playCanvas = GameObject.Find ("Inventories").transform.FindChild ("panelCanvas").gameObject;

		playPanel = playCanvas.transform.FindChild("playPanel").gameObject;
		continuePanel = playCanvas.transform.FindChild("continuePanel").gameObject;
		endPanel = playCanvas.transform.FindChild("endPanel").gameObject;

		scoreText = (Text)GameObject.Find ("scoreText").GetComponent<Text>();

		continueText = (Text)GameObject.Find ("continueCountText").GetComponent<Text>();

		continueButtonText = (Text)GameObject.Find ("continueButton").transform.FindChild("Text").GetComponent<Text>();

		continueScoreText = (Text)GameObject.Find ("continueScoreText").GetComponent<Text>();

        rewardScoreText = (Text)GameObject.Find("rewardText").GetComponent<Text>();

    }
	void Start () {

		Initial ();
	}
	public void Initial(){
		if (CYWGlobal.gameSelect == CYWGlobal.gameAll) {
			endPanelNextButtonText.text = "敬请期待";
		} else {
			endPanelNextButtonText.text = "下一关";
		}
		continuePanel.SetActive (false);
		endPanel.SetActive (false);
		playPanel.SetActive (true);
        nowPoint = 0;
    }
	// Update is called once per frame
	void Update () {
		if (CYWGlobal.gameMode == 2 && mapBuilder.EndlessUpdateOrNot ()) {
			int select = Random.Range (1, 4);
			//Debug.Log (select);
			mapBuilder.EndlessUpdateMap(select);
			heroRun.EndlessUpdateMap (select);
		}
	}
	public void onBack(){
		SceneManager.LoadScene ("Select");
	}
	public void onPlayPanelLeft(){
		heroRun.OnLeft ();
	}
	public void onPlayPanelRight(){
		heroRun.OnRight ();
	}
	public void onContinuePanelReplay(){
		heroRun.Initial ();
		mapBuilder.Initial ();
		Initial ();
	}
	public void onContinuePanelContinue(){
		if (heroRun.onContinue ())
			ActiveContinuePanel (false);
	}
	public void onContinuePanelPull(){
		addpoint ();
	}

	public void onEndPanelNext(){
		if (CYWGlobal.gameSelect < CYWGlobal.gameAll) {
			CYWGlobal.gameSelect++;
			SceneManager.LoadScene ("load");
		}
	}
	public void onEndPanelReplay(){
		heroRun.Initial ();
		mapBuilder.Initial ();
		Initial ();
	}
	public void onEndPanelPull(){
		addpoint ();
	}
	public void onEndPanelBack(){
		ActiveEndPanel (false);
	}

	public void ActivePlayPanel(bool f){
		playPanel.SetActive (f);
	}
	public void ActiveContinuePanel(bool f){
		continuePanel.SetActive (f);
	}
	public void ActiveEndPanel(bool f){
		endPanel.SetActive (f);
	}
	public void ScoreTextUpdate(int f){
		scoreText.text = f.ToString ();
		nowPoint = f;
		continueScoreText.text = "当前得分: " +f.ToString ();
	}
	public void ContinueTextUpdate(int f){
		continueText.text = "剩余复活次数" + f.ToString ();
		continueButtonText.text = "继续" + f.ToString (); 
	}
	public void addpoint() {
		string url = "http://www.szygmy.cn/game/lolpoint.php";
		WWWForm form = new WWWForm();
		string UserName = CYWGlobal.userName;    //这里是当前用户
		int Point = nowPoint;      //这里填分数
		form.AddField("UserName", UserName);
		form.AddField("Points", Point);
		form.AddField("Mode", 2);
		StartCoroutine(GoForm(url, form));
		// w = new WWW(url, form);
		for (int i = 0; i < 10000; i++)
		{
			for (int j = 0; j < 10000; j++)
			{
			}
		}
		//delay

		Debug.Log(www.text);
	}

	IEnumerator GoForm(string url, WWWForm form)
	{
		www = new WWW(url, form);
		yield return www;

		if (www.error != null)
			print(www.error);
		else
			Debug.Log("提交成功！");

	}

	IEnumerator GoForm(string url)
	{
		www = new WWW(url);
		yield return www;

		if (www.error != null)
			print(www.error);
		else
			Debug.Log("提交成功！");

	}
}
