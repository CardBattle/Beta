using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private void Awake()
    {
      foreach(var character in GameObject.FindGameObjectsWithTag("Player"))
            character.GetComponent<Character>().Init();
        
        
        
       GameObject.Find("CardManager").GetComponent<CardManager>().Init();
       
    }
}
