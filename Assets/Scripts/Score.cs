using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] List<Nutrient> FoodDB;
    [SerializeField] float carbohydrate;
    [SerializeField] float protein;
    [SerializeField] float fat;
    [SerializeField] float score;
    [SerializeField] float finalScore;
    public void AddFood(int index, float amount)//음식 인덱스와 양을 넣으면 영양소 값 더해짐
    {
        carbohydrate += FoodDB[index].carbohydrate * amount;
        protein+= FoodDB[index].protein * amount;
        fat+= FoodDB[index].fat * amount;
    }
    private void Scoring()
    {

    }
}
