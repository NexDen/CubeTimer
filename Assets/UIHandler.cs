using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField] Scrambler scrambler;
    public void sahneDeğiştir(string sahne){
        SceneManager.LoadScene(sahne);
    }


    

}