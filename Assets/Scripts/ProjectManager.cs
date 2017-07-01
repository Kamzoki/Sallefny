using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;
using System.Net;
using System.IO;
using System;
using GoMap;

public class ProjectManager : MonoBehaviour
{
    //Public:
    public static ProjectManager PM;
    public GameObject m_MainProductPreFab;
    public GameObject m_OtherProductPreFab;
    public GOMap m_GoMapRef;
    public GameObject m_InfoCanvas;
    [HideInInspector]
    public Product[] m_AllProducts;

    private List<GameObject> allMarkers = new List<GameObject>();
    private int preInt = -1;
    public enum RequestType
    {
        Post,
        Pull }

    ;

    [HideInInspector]
    public RequestType RT = new RequestType();

    public IEnumerator Request()
    {
        //Getting the web service
        WWW SallefnyWebService = new WWW("http://sallefny.com/beta/public/api/product/unity/get");

        yield return SallefnyWebService;
        fn_MapJSON(SallefnyWebService.text);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == true)
        {
            preInt = -1;
            if (allMarkers != null)
            {
                foreach (GameObject item in allMarkers)
                {
                    GameObject.Destroy(item);
                }
                allMarkers.Clear();
            }
            RT = RequestType.Pull;
            StartCoroutine(Request());
        }
    }
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(PM);
        if (PM == null || PM != this)
        {
            PM = this;
        }
        fn_StaticProductsAssignment();
    }
    private void fn_StaticProductsAssignment()
    {
        m_AllProducts = new Product[3];
        for (int i = 0; i < m_AllProducts.Length; i++)
        {
            m_AllProducts[i] = new Product();
            m_AllProducts[i].user = new User();
        }
        //Example 0
        m_AllProducts[0].name = "Laptop ROG";
        m_AllProducts[0].price = "700$";
        m_AllProducts[0].user.name = "Kamal Aittah";
        m_AllProducts[0].user.phone = "01021456789";
        m_AllProducts[0].user.email = "kamal@example.com";
        //==================================================
        //Example 1
        m_AllProducts[1].name = "Gold necklace";
        m_AllProducts[1].price = "1500$";
        m_AllProducts[1].user.name = "Jasmine Elsayed";
        m_AllProducts[1].user.phone = "01021636789";
        m_AllProducts[1].user.email = "Jasmine@example.com";
        //==================================================
        //Example 2
        m_AllProducts[2].name = "Jeep Car";
        m_AllProducts[2].price = "5000$";
        m_AllProducts[2].user.name = "Ahmad Ezzat";
        m_AllProducts[2].user.phone = "010000001";
        m_AllProducts[2].user.email = "VIP@example.com";
    }
    private void fn_MapJSON(string json)
    {
        //Maping json data to local class data
        Product lol = JsonUtility.FromJson<Product>(json);
        fn_PopulateMap(ref lol);
    }

    private void fn_PopulateMap(ref Product pro)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                GameObject newMainProduct = Instantiate(m_MainProductPreFab) as GameObject;
                m_GoMapRef.dropPin(pro.user.lat, pro.user.lon, newMainProduct);
                allMarkers.Add(newMainProduct);
            }
            else
            {
                GameObject newOtherProduct = Instantiate(m_OtherProductPreFab) as GameObject;
                newOtherProduct.GetComponent<Marker>().m_ID = i - 1;
                fn_DistributeOtherProduct(newOtherProduct);
                allMarkers.Add(newOtherProduct);
            }
        }
    }
    private void fn_DistributeOtherProduct(GameObject go)
    {
        int randomInt = UnityEngine.Random.Range(0, 4);
        if (randomInt != preInt)
        {
            switch (randomInt)
            {
                case 0: go.transform.position += new Vector3(UnityEngine.Random.Range(150, 500), go.transform.position.y, UnityEngine.Random.Range(150, 500));
                    break;
                case 1:
                    go.transform.position += new Vector3(UnityEngine.Random.Range(-150, -500), go.transform.position.y, UnityEngine.Random.Range(-155, -500));
                    break;
                case 2: go.transform.position += new Vector3(UnityEngine.Random.Range(-150, -500), go.transform.position.y, UnityEngine.Random.Range(150, 500));
                    break;
                case 3:
                    go.transform.position += new Vector3(UnityEngine.Random.Range(150, 500), go.transform.position.y, UnityEngine.Random.Range(-150, -500));
                    break;
            }
            preInt = randomInt;
        }
        else
        {
            fn_DistributeOtherProduct(go);
        }
    }
}

[System.Serializable]
public class Product
{
    public int id;
    public string name;
    public string price;
    public string image;
    public string created_at;
    public User user;
}

[System.Serializable]
public class User
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
