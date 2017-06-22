using UnityEngine;
using System.Collections;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;
using System.Net;
using System.IO;
using System;

public class ProjectManager : MonoBehaviour
{
	//Public:
	public static ProjectManager PM;
	public Text m_InterActiveText;
	//Private:
	public enum RequestType
	{
		Post,
		Pull}

	;

	[HideInInspector]
	public RequestType RT = new RequestType ();
	private string url;
	private Product reusableData;

	public IEnumerator Request ()
	{

		//Getting the web service
		WWW SallefnyWebService = new WWW ("http://sallefny.com/beta/public/api/products/11/view");
        //WWW SallefnyWebService = new WWW("http://13.255.253.57:8000/api/products/10/view");

		yield return SallefnyWebService;

        Debug.Log("Entered");
		switch (RT) {
		case RequestType.Post:
			//InterActive Text to report what's happening
			m_InterActiveText.text = "Waiting for Data.";
			yield return new WaitForSeconds (1f);
			m_InterActiveText.text = "Data sent.";
			yield return new WaitForSeconds (1f);
			m_InterActiveText.text = " ";
			break;
		case RequestType.Pull:
                //InterActive Text to report what's happening
            Debug.Log("Waiting for Data.");
			yield return new WaitForSeconds (1f);
			Debug.Log("Data received.");
			yield return new WaitForSeconds (1f);
                Debug.Log(SallefnyWebService.text);
                fn_MapJSON(SallefnyWebService.text);
			break;
		default:
			break;
		}

	}

	void HideText ()
	{
		m_InterActiveText.text = " ";
	}

    private void OnApplicationFocus(bool focus)
    {
        if (focus == true)
        {
            RT = RequestType.Pull;
            StartCoroutine(Request());
        }
    }
    // Use this for initialization
    void Start ()
	{
        DontDestroyOnLoad(PM);
        if (PM == null || PM != this)
        {
            PM = this;
        }

		m_InterActiveText.text = "Welcome";
        Invoke("HideText", 2f);
	}

	public void fn_RefreshInfo ()
	{
		StartCoroutine (Request ());
	}

	static void fn_MapJSON (string json)
	{
        //Maping json data to local class data
        Product lol = JsonUtility.FromJson<Product>(json);
        Debug.Log(lol.id);
        Debug.Log(lol.name);
        Debug.Log(lol.price);
        Debug.Log(lol.image);
        Debug.Log(lol.created_at);
        Debug.Log("user:- ");
        Debug.Log(lol.user.id);
        Debug.Log(lol.user.name);
        Debug.Log(lol.user.phone);
        Debug.Log(lol.user.email);
        Debug.Log(lol.user.birthdate);
        Debug.Log(lol.user.gender);
        Debug.Log(lol.user.lat);
        Debug.Log(lol.user.lon);
        Debug.Log(lol.user.created_at);
        Debug.Log(lol.user.updated_at);
    }

	public void fn_PushInfo ()
	{
		//Creating json object
		string json = JsonUtility.ToJson (reusableData.id);

		//Creating WWWform
		WWWForm form = new WWWForm ();

		//Adding field according to the web service fields (No data were sent to me!)
		form.AddField ("new field", json);

		WWW newWWW = new WWW (url, form);
		StartCoroutine (Request ());
	}
}

[System.Serializable]
class Product
{
    public int id;
    public string name;
    public int price;
    public string image;
    public string created_at;
    public User user;
}

[System.Serializable]
class User
{
    public int id;
    public string name;
    public string email;
    public string phone;
    public string birthdate;
    public string gender;
    public string photo;
    public float lat;
    public float lon;
    public string created_at;
    public string updated_at;
}