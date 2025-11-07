using UnityEngine;
using UnityEngine.UI;
public class ss1 : MonoBehaviour
{
    public CustomerData data;
    public GameObject customer;
    public SpriteRenderer spriteRenderer;
    public Image Image;
    void Start()
    {
        gameObject.SetActive(true);
        //if(data.customerNum==)
        Image.sprite = data.customerSprite;
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }
}