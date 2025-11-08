using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject Meats;
    [SerializeField] GameObject Grains;
    [SerializeField] GameObject Fruits;
    [SerializeField] GameObject Dairy;
    [SerializeField] GameObject Table;
    [SerializeField] Score score;
    [SerializeField] List<GameObject> customers;
    [SerializeField] List<int> randomList;//손님 인덱스를 랜덤으로 저장한 리스트
    [SerializeField] int cnt=0;//손님 수
    public int cindex = 0;//cnt랑 다르게 0~3까지 하고 다시 0부터 시작
    private void Start()
    {
        Shuffle();
        CustomerIn();
    }
    public void MovePlusX(GameObject obj)
    {
         obj.transform.DOLocalMoveX(-220, 0.4f);
    }
    public void MovePlusX2(GameObject obj)
    {        
         obj.transform.DOLocalMoveX(1620, 0.4f);
    }
    public void MoveMinusX(GameObject obj)
    {      
        obj.transform.DOLocalMoveX(-1610, 0.4f);
    }
    public void MoveMinusX2(GameObject obj)
    {
        obj.transform.DOLocalMoveX(220, 0.4f);
    }
    public void SelectFood(int index)//재료 선택하면 실행하는 함수
    { 
        score.index = index;
        Table.gameObject.SetActive(true);
    }
    public void Shuffle()//마지막 인덱스가 처음으로 나오지 않게 손님순서 셔플
    {
        int last = randomList[randomList.Count - 1];
        while(true)
        {
            for (int i = randomList.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                int temp = randomList[i];
                randomList[i] = randomList[randomIndex];
                randomList[randomIndex] = temp;
            }
            if (randomList[0]!=last)
            {
                break;
            }
        }       
    }
    public void CustomerIn()
    {
        customers[randomList[cindex]].transform.DOLocalMoveY(72, 1f).SetEase(Ease.OutBack);
    }
    public void CustomerOut()
    {
        customers[randomList[cindex]].transform.DOLocalMoveY(-667, 1f).SetEase(Ease.OutBack);
        cindex++;
        cnt++;
    }
}
