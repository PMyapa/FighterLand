using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class godata : MonoBehaviour
{
    public float score;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
