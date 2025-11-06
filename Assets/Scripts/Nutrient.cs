using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "ScriptableObject/Food")]
public class Nutrient : ScriptableObject
{
    public int carbohydrate;
    public int protein;
    public int fat; 
}
