using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadingManager.LoadInstance.LoadScene("Menu");
    }

   
}
