using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFunctions : MonoBehaviour
{
    public GameObject m_InfoWindow;

    //Info Components
    public Text m_ProductName;
    public Text m_ProductPrice;
    public Text m_OwnerName;
    public Text m_OwnerPhone;
    public Text m_OwnerEmail;

	public void Pull ()
	{
		ProjectManager.PM.RT = ProjectManager.RequestType.Pull;
        StartCoroutine(ProjectManager.PM.Request());
	}

    public void CloseInfoWindow()
    {
        m_InfoWindow.SetActive(false);
    }
}
