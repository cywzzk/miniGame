using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class rankrefresh : MonoBehaviour {
    Text rank;
    public WWW www;
    // Use this for initialization
    void Start () {
        refresh();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addpoint() {
        string url = "http://www.szygmy.cn/game/lolpoint.php";
        WWWForm form = new WWWForm();
        string UserName = "szy";    //这里是当前用户
        int Point = 10000;      //这里填分数
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

    public void refresh() {
        string url = "http://www.szygmy.cn/game/lolpoint.php";
        rank = GameObject.Find("rank").GetComponent<Text>();
        WWWForm form = new WWWForm();
        form.AddField("UserName", "");
        form.AddField("Points", 1);
        form.AddField("Mode", 1);

        StartCoroutine(GoForm(url, form));
        // w = new WWW(url, form);
        for (int i = 0; i < 10000; i++)
        {
            for (int j = 0; j < 10000; j++)
            {
            }
        }
        //delay
        rank.text = www.text.Replace(';','\n').Replace(',',' ');
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
