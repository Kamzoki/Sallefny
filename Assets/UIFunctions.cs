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
}
