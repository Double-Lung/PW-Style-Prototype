using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private void Awake() {
        if (PlayerPrefs.HasKey("Progress")) {
            return;
        }
        InitializePrefs();
    }

    void InitializePrefs() {
        PlayerPrefs.SetInt("Progress", 0);
        PlayerPrefs.SetInt("Members", 0);
        PlayerPrefs.SetInt("Speed", 1);
    }
    void Start()
    {
        
    }
}
