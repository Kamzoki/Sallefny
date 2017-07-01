using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {

    [HideInInspector]
    public int m_ID;

    private RaycastHit info;
    private Ray cameraRay;
    private UIFunctions infoCanvasComponents;

    // Use this for initialization
    void Start () {
       infoCanvasComponents = ProjectManager.PM.m_InfoCanvas.GetComponent<UIFunctions>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount == 1)
        {
            Touch finger = Input.GetTouch(0);
            cameraRay = Camera.main.ScreenPointToRay(finger.position);
            if (finger.phase == TouchPhase.Ended)
            {
                if (Physics.Raycast (cameraRay, out info))
                {
                    if (info.collider.tag == "Marker")
                    {
                        if (m_ID < ProjectManager.PM.m_AllProducts.Length && m_ID >= 0)
                        {
                            infoCanvasComponents.m_ProductName.text = ProjectManager.PM.m_AllProducts[m_ID].name;
                            infoCanvasComponents.m_ProductPrice.text = ProjectManager.PM.m_AllProducts[m_ID].price;
                            infoCanvasComponents.m_OwnerName.text = ProjectManager.PM.m_AllProducts[m_ID].user.name;
                            infoCanvasComponents.m_OwnerPhone.text = ProjectManager.PM.m_AllProducts[m_ID].user.phone;
                            infoCanvasComponents.m_OwnerEmail.text = ProjectManager.PM.m_AllProducts[m_ID].user.email;
                            ProjectManager.PM.m_InfoCanvas.SetActive(true);
                        }
                    }
                }
            }
        }
	}
}
