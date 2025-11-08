using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] UIManager uimanager;
    [SerializeField] List<Nutrient> FoodDB;
    [SerializeField] float carbohydrate=0;
    [SerializeField] float protein=0;
    [SerializeField] float fat=0;
    [SerializeField] float score=0;
    [SerializeField] int finalScore=0;
    [SerializeField] List<TextMeshProUGUI> scoreText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] List<string> preferenceList;//손님이 어떤 재료를 선호하는지 리스트화    
    [SerializeField] GameObject ScoreBoard;
    [SerializeField] GameObject boardButton;
    int preferencePlus=0;//선호도로 얼마나 보너스를 받는지
    public int index;//현재 조리하는 음식의 인덱스(UI매니저에서 할당)
    public int nutrientScore = 0;
    public int preferScore = 0;   
    public void AddFood(float amount)//음식 인덱스와 양을 넣으면 영양소 값 더해짐
    {
        carbohydrate += FoodDB[index].carbohydrate * amount;
        protein += FoodDB[index].protein * amount;
        fat += FoodDB[index].fat * amount;
        if (FoodDB[index].type == preferenceList[uimanager.cindex] && preferencePlus < 500)//손님이 선호하고 선호도 추가점수가 500미만이면
        {
            preferencePlus += 100;//선호 보너스 +100
        }
    }
    public void Scoring()
    {
        float total = carbohydrate + protein + fat;
        carbohydrate = carbohydrate / total * 100;//백분위로 %표시
        protein= protein / total * 100;
        fat= fat / total * 100;
        score = 1000 //탄수 55% 단백질 20% 지방 25%에서 벗어날수록 1%당 -50점
            - Mathf.Abs(55 - carbohydrate) * 50
            - Mathf.Abs(20 - protein) * 50
            - Mathf.Abs(25 - fat) * 50;
        nutrientScore += (int)score;
        preferScore += preferencePlus;
        score += preferencePlus;
        carbohydrate = 0;
        protein = 0;
        fat = 0;
        preferencePlus = 0;
        finalScore += (int)score;
        finalScoreText.text = ""+finalScore;

        uimanager.CustomerOutCo();
    }
    public void GameEnd()
    {
        StartCoroutine("GameEndCo");
    }

    IEnumerator GameEndCo()
    {
        int cnt = uimanager.cnt + 1;
        uimanager.CountText.gameObject.SetActive(true);
        uimanager.CountText.text = "Finish!";
        yield return new WaitForSeconds(2);
        ScoreBoard.SetActive(true);
        scoreText[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        scoreText[1].gameObject.SetActive(true);
        scoreText[1].text = cnt + "명";
        yield return new WaitForSeconds(1);
        scoreText[2].gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        scoreText[3].gameObject.SetActive(true);
        scoreText[3].text = nutrientScore + "/"+ cnt*1000;
        yield return new WaitForSeconds(1);
        scoreText[4].gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        scoreText[5].gameObject.SetActive(true);
        scoreText[5].text = preferScore + "/" + cnt * 500;
        yield return new WaitForSeconds(1);
        scoreText[6].gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        scoreText[7].gameObject.SetActive(true);
        scoreText[7].text = ""+(nutrientScore+preferScore);
        yield return new WaitForSeconds(1);
        boardButton.SetActive(true);
    }
}
