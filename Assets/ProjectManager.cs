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
		WWW SallefnyWebService = new WWW ("http://sallefny.com/beta/public/api/items/1");
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
            Debug.Log("Deta Recived: " + SallefnyWebService.text);
            fn_ParseJsonString(SallefnyWebService.text);
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
        RT = RequestType.Pull;
		StartCoroutine (Request ());
	}

    void fn_ParseJsonString(string rawJson)
    {
        string jsonItem = "";
        char jsonItemBuffer;
        int jsonItemBoundry = 0;
        int jsonArrayIndex;
        int jsonItemIndex = 0;
        bool startParsing = false;

        if (rawJson != null || rawJson.Length > 0)
        {
            for ( jsonArrayIndex = 0; jsonArrayIndex < rawJson.Length; jsonArrayIndex = jsonItemIndex + 1)
            {
                jsonItemBuffer = rawJson[jsonArrayIndex];
                if (jsonItemBuffer == '{')
                {
                    jsonItemBoundry++;
                    startParsing = true;
                }
                if (startParsing == true)
                {
                    jsonItem += jsonItemBuffer;
                    for ( jsonItemIndex = jsonArrayIndex+1; jsonItemIndex < rawJson.Length; jsonItemIndex++)
                    {
                        jsonItemBuffer = rawJson[jsonItemIndex];
                        jsonItem += jsonItemBuffer;
                        if (jsonItemBuffer == '}')
                        {
                            jsonItemBoundry--;
                        }
                        if (jsonItemBoundry == 0)
                        {
                            //fn_MapJSON(jsonItem);
                            Debug.Log(jsonItem);
                            jsonItem = "";
                            startParsing = false;
                            break;
                        }
                    }
                }
            }
        }
    }
	static void fn_MapJSON (string json)
	{
        //Maping json data to local class data
        Product [] lol = JsonUtility.FromJson<Product[]>(json);
        for (int i = 0; i < lol.Length; i++)
        {
            Debug.Log("Product " + i + ":- ");
            Debug.Log(lol[i].id);
            Debug.Log(lol[i].name);
            Debug.Log(lol[i].price);
            Debug.Log(lol[i].image);
            Debug.Log(lol[i].created_at);
            Debug.Log("user:- ");
            Debug.Log(lol[i].user.id);
            Debug.Log(lol[i].user.name);
            Debug.Log(lol[i].user.phone);
            Debug.Log(lol[i].user.email);
            Debug.Log(lol[i].user.birthdate);
            Debug.Log(lol[i].user.gender);
            Debug.Log(lol[i].user.lat);
            Debug.Log(lol[i].user.lon);
            Debug.Log(lol[i].user.created_at);
            Debug.Log(lol[i].user.updated_at);
            Debug.Log(lol[i].category.id);
            Debug.Log(lol[i].category.name);
        }
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
    public Category category;
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

[System.Serializable]
class Category
{
    public int id;
    public string name;
}