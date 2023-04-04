using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> toBeRemoved;
    [SerializeField] public Text PbText;
    public Text clock;

    public void HideScreen(){
        foreach(GameObject g in toBeRemoved){
            g.gameObject.SetActive(false);
        }
    }

    public void ShowScreen(){
        foreach(GameObject g in toBeRemoved){
            g.gameObject.SetActive(true);
        }
    }

    void Update(){
        clock.text = System.DateTime.Now.ToString("HH:mm");
    }

}