using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UI_LoginSystem : MonoBehaviour {
	public InputField Username;
	public InputField Password;
	public WWW www;
	public Text textUsername , textPassword , welcome;
	public Button regIn, logIn;
	GameObject playPanel,loginPanel;
	GameObject playCanvas;
	void Awake(){

		playCanvas = GameObject.Find ("Canvas").gameObject;

		playPanel = playCanvas.transform.FindChild("playPanel").gameObject;

		loginPanel = playCanvas.transform.FindChild("loginPanel").gameObject;

		Username = GameObject.Find("UserName").GetComponent<InputField>();
		Password = GameObject.Find("PassWord").GetComponent<InputField>();
		welcome = GameObject.Find("Welcome").GetComponent<Text>();


    }

	// Use this for initialization
	void Start () {
		if (CYWGlobal.userName == "") {
			playPanel.SetActive (false);
			loginPanel.SetActive (true);
		} else {
            welcome.text = "Welcome! " + CYWGlobal.userName;
            playPanel.SetActive (true);
			loginPanel.SetActive (false);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPlay(){
		SceneManager.LoadScene("Select");
	}
	public void OnSetting(){
		//SceneManager.LoadScene("3");
	}
	public void OnMore(){
		SceneManager.LoadScene("rank");
	}
	public void OnAbout(){
		SceneManager.LoadScene("about");
	}
	public void OnClose(){
        if (playPanel.activeSelf)
        {
            welcome.text = "Welcome!";
            playPanel.SetActive(false);
            loginPanel.SetActive(true);
            CYWGlobal.userName = "";
        }
        else
        {
            Application.Quit();
        }
	}



	public void login(int mode) {
		string url = "http://www.szygmy.cn/game/lol.php";


		WWWForm form = new WWWForm();
		form.AddField("UserName", Username.text);
		form.AddField("PassWord", Password.text);
		switch (mode) {
		case 1:
			Debug.Log("name:" + Username.text + " password:" + Password.text);
			form.AddField("LoginMode", 1);
			//WWW w;//= new WWW(url, form);

			StartCoroutine(GoForm(url,form));
			// w = new WWW(url, form);
			for (int i = 0; i < 10000; i++) {
				for (int j = 0; j < 10000; j++) {
				}
			}
			//delay
			if (www.text[0] == 'O') {
				welcome.text = "Welcome! " + Username.text;
				playPanel.SetActive (true);
				loginPanel.SetActive (false);
				CYWGlobal.userName =  Username.text;
			}

			Debug.Log(www.text);
			break;
		case 2:
			Debug.Log("name:" + Username.text + " password:" + Password.text);
			form.AddField("LoginMode", 2);
			StartCoroutine(GoForm(url, form));
			for (int i = 0; i < 10000; i++)
			{
				for (int j = 0; j < 10000; j++)
				{
				}
			}
			if (www.text[0] == 'O')
			{
				welcome.text = "Welcome! " + Username.text;
				playPanel.SetActive (true);
				loginPanel.SetActive (false);
				CYWGlobal.userName =  Username.text;
			}
			Debug.Log(www.text);
			break;
		default:
			break;
		}
	}

	IEnumerator GoForm(string url , WWWForm form)
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
	public void OnGuest(){
		Username.text = "Guest";
		welcome.text = "Welcome! " + Username.text;
		playPanel.SetActive (true);
		loginPanel.SetActive (false);
		CYWGlobal.userName =  Username.text;
	}

}
