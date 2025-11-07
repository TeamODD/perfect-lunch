using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject Meats;
    public GameObject Grains;
    public GameObject Fruits;
    public GameObject Dairy;

    public void MovePlusX(GameObject obj)
    {
         obj.transform.DOLocalMoveX(-220, 2.0f);
    }
    public void MovePlusX2(GameObject obj)
    {        
         obj.transform.DOLocalMoveX(1620, 2.0f);
    }
    public void MoveMinusX(GameObject obj)
    {      
        obj.transform.DOLocalMoveX(-1610, 2.0f);
    }
    public void MoveMinusX2(GameObject obj)
    {
        obj.transform.DOLocalMoveX(220, 2.0f);
    }
}
