using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject Meats;
    [SerializeField] GameObject Grains;
    [SerializeField] GameObject Fruits;
    [SerializeField] GameObject Dairy;
    [SerializeField] GameObject Table;
    [SerializeField] Score score;   

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

}
