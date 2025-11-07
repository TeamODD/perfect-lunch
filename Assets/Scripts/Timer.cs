using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI text_Timer;
    public float limitTime;//초 단위로 입력받음
    public bool gameStart;

    private void Update()
    {
        if(gameStart)
        {
            limitTime -= Time.deltaTime;
            if(Mathf.Floor(limitTime / 60)<10)
            {
                if(Mathf.Round(limitTime) % 60<10)
                {
                    text_Timer.text = "0" + Mathf.Floor(limitTime / 60) + " : 0" + Mathf.Round(limitTime) % 60;
                }
                else
                {
                    text_Timer.text = "0" + Mathf.Floor(limitTime / 60) + " : " + Mathf.Round(limitTime) % 60;
                }
                    
            }
            else
            {
                if (Mathf.Round(limitTime) % 60 < 10)
                {
                    text_Timer.text = "" + Mathf.Floor(limitTime / 60) + " : 0" + Mathf.Round(limitTime) % 60;
                }
                else
                {
                    text_Timer.text = "" + Mathf.Floor(limitTime / 60) + " : " + Mathf.Round(limitTime) % 60;
                }
            }
                
        }
    }
}
