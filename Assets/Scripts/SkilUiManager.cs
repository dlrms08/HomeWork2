using UnityEngine;
using UnityEngine.UI;

public class SkilUiManager : MonoBehaviour
{
    public Button[] buttons;

    private void OnEnable()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }


        for(int i = 0; i < 2; i++)
        {
            int rnd = Random.Range(0, buttons.Length);
            int maxIterations = 0;
            while (!buttons[rnd].interactable && maxIterations < 100)
            {
                rnd = Random.Range(0, buttons.Length);
                maxIterations++;
                //Debug.Log(maxIterations);
            }
            maxIterations = 0;

            buttons[rnd].gameObject.SetActive(true);
        }
    }
}
