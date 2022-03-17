using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public bool[] skills;
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
        skills = new bool[4];
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
        if (playerLv > 3)
            return;

        exp += points;
        expSlider.fillAmount = exp / playerLv;
        if (exp >= playerLv)
        {
            exp = 0;
            playerLv++;
            StartCoroutine(SkillUIOnCo());
        }
    }



    IEnumerator SkillUIOnCo()
    {
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0;
        skillUIObj.SetActive(true);
    }

    public void SkillEnable(int num)
    {
        skills[num] = true;
        StartCoroutine(SkillEnableCo());
    }

    IEnumerator SkillEnableCo()
    {
        Time.timeScale = 1;
        AddExp(0);
        yield return new WaitForSeconds(0.3f);
        skillUIObj.SetActive(false);
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
