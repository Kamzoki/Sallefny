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
	private Data reusableData;

	public IEnumerator Request ()
	{

		//Getting the web service
		WWW SallefnyWebService = new WWW ("http://sallefny.com/beta/public/api/product/1/owner");

		yield return SallefnyWebService;

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
			m_InterActiveText.text = "Waiting for Data.";
			yield return new WaitForSeconds (1f);
			m_InterActiveText.text = "Data received.";
			yield return new WaitForSeconds (1f);
			m_InterActiveText.text = " ";
			fn_MapJSON (SallefnyWebService.text);
			break;
		default:
			break;
		}
		//InterActive Text to report what's happening
		m_InterActiveText.text = "Waiting for Data.";
		yield return new WaitForSeconds (1f);
		m_InterActiveText.text = "Data received.";
		yield return new WaitForSeconds (1f);
		m_InterActiveText.text = " ";

	}

	void HideText ()
	{
		m_InterActiveText.text = " ";
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

	void fn_MapJSON (string json)
	{
		//Maping json data to local class data
		Data reusableData = JsonUtility.FromJson<Data> (json);

		//Showing Data on Canvas (No data were sent to me!)
        
	}

	public void fn_PushInfo ()
	{
		//Creating json object
		string json = JsonUtility.ToJson (reusableData.m_ProductID);

		//Creating WWWform
		WWWForm form = new WWWForm ();

		//Adding field according to the web service fields (No data were sent to me!)
		form.AddField ("new field", json);

		WWW newWWW = new WWW (url, form);
		StartCoroutine (Request ());
	}
}

[Serializable]
class Data
{
    public int m_ProductID;
    public string m_OwnerName;
    public string m_OwnerEmail;
    public int m_OwnerNumber;
    public string m_OwnerGender;
    public Sprite m_OwnerPicture;

   /* public Data(int PID, string ON, string OE, int ONU, string OG, Sprite OP)
    {
        this.m_ProductID = PID;
        this.m_OwnerName = ON;
        this.m_OwnerEmail = OE;
        this.m_OwnerNumber = ONU;
        this.m_OwnerGender = OG;
        this.m_OwnerPicture = OP;
    }*/
	
}