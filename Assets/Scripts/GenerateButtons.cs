using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenerateButtons : MonoBehaviour
{
    public int buttonCount;
    public float minX, minY, maxX, maxY;
    public int row, column;
    public GameObject buttonPrefab;
    private TitleScreen titleScreen;

    private void Awake() {
        titleScreen = GetComponent<TitleScreen>();
    }
    void Start()
    {
        Setup();
    }
    void Setup() {
        float unitWidth = (maxX - minX) / column;
        float unitHeight = (maxY - minY) / row;
        int index = 0;
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                int level = index;
                float x = minX + j * unitWidth + unitWidth/2;
                float y = maxY - i * unitHeight - unitHeight/2;
                GameObject button = Instantiate(buttonPrefab);
                button.GetComponentInChildren<TextMeshProUGUI>().SetText((index+1).ToString());
                button.GetComponent<Button>().onClick.AddListener(()=> LoadLevel(level));
                button.transform.SetParent(transform);
                button.transform.localPosition = new Vector2(x,y);
                index++;
            }
        }
    }
    void LoadLevel(int index) {
        int p = PlayerPrefs.GetInt("Progress");
        if (p >= index) {
            if (PlayerPrefs.GetInt("Members") > 0) {
                PlayerPrefs.SetInt("level", index);
                SceneManager.LoadScene(1);
            } else {
                titleScreen.NoMember();
            }
        } else {
            
            titleScreen.LevelLocked();
        }
    }
}
