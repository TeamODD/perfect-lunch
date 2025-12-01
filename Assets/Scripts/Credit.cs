using UnityEngine;
using DG.Tweening;

public class Credit : MonoBehaviour
{
    public GameObject credit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickCredit()
    {
        credit.SetActive(true);
        credit.transform.localScale=new Vector3 (0f, 0f, 0f);
        credit.transform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.OutBack);
    }
    public void ExitCredit()
    {
        credit.SetActive (false);
    }
}
