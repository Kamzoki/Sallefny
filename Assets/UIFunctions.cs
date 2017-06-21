using UnityEngine;
using System.Collections;

public class UIFunctions : MonoBehaviour
{

	public void Post ()
	{
		ProjectManager.PM.RT = ProjectManager.RequestType.Post;
		ProjectManager.PM.fn_PushInfo ();
	}

	public void Pull ()
	{
		ProjectManager.PM.RT = ProjectManager.RequestType.Pull;
        StartCoroutine(ProjectManager.PM.Request());
	}

    public void OpenNavClient()
    {
        bool fail = false;
        string bundleId = "";
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch (System.Exception e)
        {
            fail = true;
        }

        if (fail)
        { //open app in store
            Application.OpenURL("https://google.com");
        }
        else //open the app
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
    }
}
