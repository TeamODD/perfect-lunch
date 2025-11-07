using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Customer")]
public class CustomerData : ScriptableObject
{
    public int[] customerNum = new int[4];
    public Sprite customerSprite;
}

