using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer;//food
    public float power = 0.4f;
    private List<GameObject> sliceList = new List<GameObject>();
    private float min = 100f;//오른쪽 젤 작은 마우스 클릭 위치
    private float originalImageWidth;
    private Vector3 originalLeftEdge;//이미지 왼모퉁이
    void Start()
    {
            AddSliceList(targetSpriteRenderer.gameObject);
            originalImageWidth = targetSpriteRenderer.bounds.size.x;
            originalLeftEdge = targetSpriteRenderer.bounds.min;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SlicePosition();
        }
    }
    void AddSliceList(GameObject obj)
    {
        obj.AddComponent<BoxCollider2D>();//추가
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
        col.size = sr.sprite.bounds.size;//콜라이더 맞추기
        col.offset = sr.sprite.bounds.center;//중심점 맞추기
        sliceList.Add(obj);
    }
    void SlicePosition()//자를 거 찾기
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        GameObject slicePart=null;
        for (int i = 0; i < sliceList.Count; i++)
        {
            GameObject slice = sliceList[i];
            BoxCollider2D col = slice.GetComponent<BoxCollider2D>();
            if (col.OverlapPoint(worldPos))//클릭이 콜라이더 내부
            {
                slicePart = slice;
            }
        }
        if (slicePart == null)
        {
            return;
        }
        Slicing(slicePart, worldPos);
    }

    void Slicing(GameObject sliceObj, Vector3 worldClick)//자름
    {
        SpriteRenderer sr = sliceObj.GetComponent<SpriteRenderer>();
        Sprite sprite = sr.sprite;
        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
        tex.SetPixels(pixels);
        tex.Apply();

        //비율 계산
        float ImagePosition = worldClick.x - originalLeftEdge.x; //클릭과 모퉁이 거리
        float rightPercent = Mathf.Clamp01(1f - (ImagePosition / originalImageWidth)) * 100f;//최소 오른쪽%
        if (rightPercent < min)
        {
            min = rightPercent;
            Debug.Log($"r= {min}%");
        }

        Vector3 localClick = sr.transform.InverseTransformPoint(worldClick);//오브젝트 중심 기준 위치

        float ppu = sprite.pixelsPerUnit;//월드-픽셀 비율
        Vector2 pivot = sprite.pivot;//중심점
        int texWidth = tex.width;
        int texHeight = tex.height;

        int leftWidth = Mathf.RoundToInt(pivot.x + localClick.x * ppu );//몇 번째 픽셀
        leftWidth = Mathf.Clamp(leftWidth, 1, texWidth - 1);//범위 나가지 않게
        int rightWidth = texWidth - leftWidth;

        Texture2D leftTex = new Texture2D(leftWidth, texHeight);
        Texture2D rightTex = new Texture2D(rightWidth, texHeight);
        leftTex.SetPixels(tex.GetPixels(0, 0, leftWidth, texHeight));
        rightTex.SetPixels(tex.GetPixels(leftWidth, 0, rightWidth, texHeight));
        leftTex.Apply();
        rightTex.Apply();

        Sprite leftSprite = Sprite.Create(leftTex, new Rect(0, 0, leftWidth, texHeight), new Vector2(0.5f, 0.5f), ppu);
        Sprite rightSprite = Sprite.Create(rightTex, new Rect(0, 0, rightWidth, texHeight), new Vector2(0.5f, 0.5f), ppu);

        GameObject leftObj = new GameObject("Left", typeof(SpriteRenderer));//오브젝트화
        GameObject rightObj = new GameObject("Right", typeof(SpriteRenderer));
        SpriteRenderer leftImage = leftObj.GetComponent<SpriteRenderer>();
        SpriteRenderer rightImage = rightObj.GetComponent<SpriteRenderer>();

        leftImage.sprite = leftSprite;//적용
        rightImage.sprite = rightSprite;

        leftImage.sortingOrder = sr.sortingOrder;
        rightImage.sortingOrder = sr.sortingOrder;

        leftObj.transform.localScale = sr.transform.localScale;
        rightObj.transform.localScale = sr.transform.localScale;

        Vector3 basePos = sr.transform.position;//위치
        float worldLeftWidth = leftWidth / ppu;
        float worldRightWidth = rightWidth / ppu;

        leftObj.transform.position = basePos + Vector3.left * (worldRightWidth / 2f);//중심점
        rightObj.transform.position = basePos + Vector3.right * (worldLeftWidth / 2f);

        AddSliceList(leftObj);//리스트에 넣기
        AddSliceList(rightObj);
       
        Rigidbody2D leftRb = leftObj.AddComponent<Rigidbody2D>();//밀기
        Rigidbody2D rightRb = rightObj.AddComponent<Rigidbody2D>();
        leftRb.gravityScale = 0f;
        rightRb.gravityScale = 0f;
        leftRb.linearDamping = 3f;
        rightRb.linearDamping = 3f;
        Vector2 distanse = Vector2.right * power;
        leftRb.AddForce(-distanse, ForceMode2D.Impulse);
        rightRb.AddForce(distanse, ForceMode2D.Impulse);

        sliceList.Remove(sliceObj);//원형 삭제
        Destroy(sliceObj);
    }

}
