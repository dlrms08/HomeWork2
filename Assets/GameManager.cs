using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SkillInfo
{
    public string skillName;
    public bool enable;
}

/*
Skill NO
0 doubleShot
1 threeShot
2 passShot
3 homingShot
*/

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public SkillInfo[] skillInfos;
    public float playerLv;
    private float exp;
    private Image expSlider;
    public GameObject gameoverUIObj;
    public GameObject skillUIObj;

    private void Awake()
    {
        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        expSlider = GameObject.Find("ExpBar/ExpSlider").GetComponent<Image>();
        playerLv = 1;
        exp = 0;
        AddExp(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddExp(int points)
    {
        exp += points;
        expSlider.fillAmount = exp / playerLv;
        if (exp >= playerLv)
        {
            exp = 0;
            playerLv++;
            Invoke("SkillUIOn", .5f);
        }
    }

    void SkillUIOn()
    {
        Time.timeScale = 0;
        skillUIObj.SetActive(true);
    }

    public void GameOver()
    {
        gameoverUIObj.SetActive(true);
    }

    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }
}
