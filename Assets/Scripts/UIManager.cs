using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject Meats;
    [SerializeField] GameObject Grains;
    [SerializeField] GameObject Fruits;
    [SerializeField] GameObject Dairy;
    [SerializeField] GameObject Table;
    [SerializeField] GameObject Tutorial;
    [SerializeField] GameObject Dialogue;
    [SerializeField] GameObject Cook;
    [SerializeField] GameObject CookFood;//삭제하기 위해 임시저장
    [SerializeField] Score score;
    [SerializeField] Timer timer;
    [SerializeField] List<GameObject> customers;
    [SerializeField] List<GameObject> foodPrefabs;
    [SerializeField] List<int> randomList;//손님 인덱스를 랜덤으로 저장한 리스트
    public int cnt=0;//손님 수
    public TextMeshProUGUI CountText;
    [SerializeField] TextMeshProUGUI DialogueText;
    public bool iscook = false;//지금 재료손질 중인지
    public int cindex = 0;//cnt랑 다르게 0~3까지 하고 다시 0부터 시작
    private void Start()
    {
        Shuffle();
        CustomerIn();
    }
    public void MovePlusX(GameObject obj)
    {
        if(!iscook)
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
        if (!iscook)
            obj.transform.DOLocalMoveX(220, 0.4f);
    }
    public void SelectFood(int index)//재료 선택하면 실행하는 함수
    { 
        if(!iscook)
        {
            score.index = index;
            Table.gameObject.SetActive(true);
            iscook = true;//현재 재료손질 중이라는 뜻
            //CookFood =Instantiate(foodPrefabs[index],Cook.transform);
            CookFood = Instantiate(foodPrefabs[index]);
            CookFood.transform.position = Cook.transform.position;
            CookFood.name = "CookFood";
            SpriteRenderer sr = CookFood.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = "Foreground"; // UI보다 앞에 나오도록
                sr.sortingOrder = 10;
            }
        }    
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
    public void CustomerOutCo()
    {
        StartCoroutine("CustomerOut");
    }
    public void Exittuto()
    {
        Tutorial.gameObject.SetActive(false);
        StartCoroutine("GameStart");
    }
    
    IEnumerator GameStart()
    {       
        CountText.gameObject.SetActive(true);
        CountText.text = "3";
        yield return new WaitForSeconds(1);
        CountText.text = "2";
        yield return new WaitForSeconds(1);
        CountText.text = "1";
        yield return new WaitForSeconds(1);
        CountText.text = "Start!";
        yield return new WaitForSeconds(1);
        CountText.gameObject.SetActive(false);
        timer.gameStart = true;
        StartCoroutine("CustomerIn");
    }
    IEnumerator CustomerIn()
    {
        customers[randomList[cindex]].transform.DOLocalMoveY(-100, 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1);
        Dialogue.SetActive(true);
        DialogueText.text = "안녕하세요";
        yield return new WaitForSeconds(1);
        Dialogue.SetActive(false);
    }
    IEnumerator CustomerOut()
    {
        cindex++;
        cnt++;
        customers[randomList[cindex]].transform.DOLocalMoveY(-785, 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1);
        Dialogue.SetActive(true);
        DialogueText.text = "감사합니다";
        yield return new WaitForSeconds(1);
        Dialogue.SetActive(false);       
    }
}
