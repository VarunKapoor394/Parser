using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class JSONcontroller : MonoBehaviour
{


    [Serializable]
    public struct JSONdataclass
    {
        public string Name;
        public string PictureUrl;
        public string DisplayName;
        public string Language;
        public int InterestID;
        public Sprite img;
    }

    JSONdataclass[] items;
    public GameObject s;
  
    public void Start()
    {
        if (File.Exists(Application.persistentDataPath + "Saved.json"))
        {
          GameObject g = GameObject.FindGameObjectWithTag("Fetch");
            g.SetActive(false);
            s.SetActive(true);
        }

       
    }

    
    public void Fetch()
    {
       StartCoroutine("GetData");
    }

    IEnumerator GetData()
    {
        string url = "https://testinterest.s3.amazonaws.com/interest.json";


        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.Send();
        if (request.isDone)
        {   
            Debug.Log("successful");
            
            string path = Application.persistentDataPath + "Saved.json";
            File.WriteAllText(path, request.downloadHandler.text);
         }
        else
        {
            Debug.Log("error");
        }



    }

    IEnumerator DlImages()
    {
        
        
            GameObject DataContainer = transform.GetChild(0).gameObject;
            GameObject dc;
            for (int i = 0; i < 8; i++)
            {
                string paths = Application.persistentDataPath + "Saved.json";
                string jsonString = File.ReadAllText(paths);
                JSONdataclass[] imgdata = JsonHelper.GetArray<JSONdataclass>(jsonString);

                WWW w = new WWW(imgdata[i].PictureUrl);


                yield return w;
                if (w.isDone)
                {


                    Debug.Log("done");

                    Texture2D tx = w.texture;
                    imgdata[i].img = Sprite.Create(tx, new Rect(0f, 0f, tx.width, tx.height), Vector2.zero, 10f);


                }

                else
                {
                    Debug.Log("error");
                }

             
              
                dc = Instantiate(DataContainer, transform);
                dc.transform.GetChild(0).GetComponent<Text>().text = imgdata[i].Name;
                dc.transform.GetChild(1).GetComponent<Image>().sprite = imgdata[i].img;
                dc.transform.GetChild(2).GetComponent<Text>().text = imgdata[i].DisplayName;
                dc.transform.GetChild(3).GetComponent<Text>().text = imgdata[i].Language;
                dc.transform.GetChild(4).GetComponent<Text>().text = imgdata[i].InterestID.ToString();
              
            }
        
    }
    public void Show()
    {
        StartCoroutine(DlImages());   
    }
    public void CloseApp()
    {
        Application.Quit();
        Debug.Log("Quit");
    }






}
    




   
