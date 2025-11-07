using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] List<Nutrient> FoodDB;
    [SerializeField] float carbohydrate=0;
    [SerializeField] float protein=0;
    [SerializeField] float fat=0;
    [SerializeField] float score=0;
    [SerializeField] float finalScore=0;
    [SerializeField] TextMeshProUGUI finalScoreText;
    public bool preference = false;
    public void AddFood(int index, float amount)//음식 인덱스와 양을 넣으면 영양소 값 더해짐
    {
        carbohydrate += FoodDB[index].carbohydrate * amount;
        protein+= FoodDB[index].protein * amount;
        fat+= FoodDB[index].fat * amount;
    }
    public void Scoring()
    {
        float total = carbohydrate + protein + fat;
        carbohydrate = carbohydrate / total * 100;//백분위로 %표시
        protein= protein / total * 100;
        fat= fat / total * 100;
        score = 1000 //영양소가 33%에서 벗어날수록 1%당 -50점
            - Mathf.Abs(33 - carbohydrate)*50
            - Mathf.Abs(33 - protein)*50
            - Mathf.Abs(33 - fat)*50;
        if(preference)//선호하면 +500점
        {
            score += 500;
        }
        preference = false;
        carbohydrate = 0;
        protein = 0;
        fat = 0;
        finalScore += score;
        finalScoreText.text = ""+finalScore;
    }
}
