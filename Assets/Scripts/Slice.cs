using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer; // food
    public float power = 0.4f;
    List<GameObject> sliceList = new List<GameObject>();
    float min = 100f;//최소 오른쪽 비율
    float originalImageWidth;// 전체 길이
    Vector3 originalLeftEdge;// 왼모퉁이
    bool iscook = false;

    void Update()
    {
        if (targetSpriteRenderer == null && !iscook)
        {
            GameObject cookFood = GameObject.Find("CookFood");
            if (cookFood != null)
            {
                Debug.Log("cookFood find");
                targetSpriteRenderer = cookFood.GetComponent<SpriteRenderer>();
                if (targetSpriteRenderer != null)
                {
                    AddSliceList(targetSpriteRenderer.gameObject);
                    originalImageWidth = targetSpriteRenderer.bounds.size.x;
                    originalLeftEdge = targetSpriteRenderer.bounds.min;
                    iscook = true;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && iscook==true)
        {
            SlicePosition();
        }
    }

    void AddSliceList(GameObject obj)
    {
        if (obj.GetComponent<BoxCollider2D>() == null)
            obj.AddComponent<BoxCollider2D>();

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
        col.size = sr.sprite.bounds.size;
        col.offset = sr.sprite.bounds.center;

        if (!sliceList.Contains(obj))
            sliceList.Add(obj);
    }

    void SlicePosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        GameObject slicePart = null;

        for (int i = 0; i < sliceList.Count; i++)
        {
            GameObject slice = sliceList[i];
            BoxCollider2D col = slice.GetComponent<BoxCollider2D>();
            if (col.OverlapPoint(worldPos))
            {
                slicePart = slice;
                break;
            }
        }
        Slicing(slicePart, worldPos);
    }

    void Slicing(GameObject sliceObj, Vector3 worldClick)
    {
        SpriteRenderer sr = sliceObj.GetComponent<SpriteRenderer>();
        Sprite sprite = sr.sprite;

        // textureRect 기준으로 Texture2D 생성
        Rect texRect = sprite.textureRect;
        int texWidth = (int)texRect.width;
        int texHeight = (int)texRect.height;

        Texture2D tex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
        Color[] pixels = sprite.texture.GetPixels((int)texRect.x, (int)texRect.y, texWidth, texHeight);
        tex.SetPixels(pixels);
        tex.Apply();

        // 클릭 위치 계산
        Vector3 localClick = sr.transform.InverseTransformPoint(worldClick);
        float ppu = sprite.pixelsPerUnit;
        Vector2 pivot = sprite.pivot;

        int leftWidth = Mathf.Clamp(Mathf.RoundToInt(pivot.x + localClick.x * ppu), 1, tex.width - 1);
        int rightWidth = tex.width - leftWidth;
        if (leftWidth <= 0 || rightWidth <= 0) return;

        float ImagePosition = worldClick.x - originalLeftEdge.x;
        float rightPercent = Mathf.Clamp01(1f - (ImagePosition / originalImageWidth)) * 100f;
        if (rightPercent < min)
        {
            min = rightPercent;
            Debug.Log($"r= {min}%");
        }
        Texture2D leftTex = new Texture2D(leftWidth, tex.height, TextureFormat.RGBA32, false);
        Texture2D rightTex = new Texture2D(rightWidth, tex.height, TextureFormat.RGBA32, false);

        Color[] leftPixels = tex.GetPixels(0, 0, leftWidth, tex.height);
        Color[] rightPixels = tex.GetPixels(leftWidth, 0, rightWidth, tex.height);

        leftTex.SetPixels(leftPixels);
        rightTex.SetPixels(rightPixels);
        leftTex.Apply();
        rightTex.Apply();

        // 스프라이트 생성
        Sprite leftSprite = Sprite.Create(leftTex, new Rect(0, 0, leftWidth, tex.height), new Vector2(0.5f, 0.5f), ppu);
        Sprite rightSprite = Sprite.Create(rightTex, new Rect(0, 0, rightWidth, tex.height), new Vector2(0.5f, 0.5f), ppu);

        //오브젝트화
        GameObject leftObj = new GameObject("Left", typeof(SpriteRenderer));
        GameObject rightObj = new GameObject("Right", typeof(SpriteRenderer));

        SpriteRenderer leftImage = leftObj.GetComponent<SpriteRenderer>();
        SpriteRenderer rightImage = rightObj.GetComponent<SpriteRenderer>();

        leftImage.sprite = leftSprite;
        rightImage.sprite = rightSprite;
        leftImage.sortingOrder = sr.sortingOrder;
        rightImage.sortingOrder = sr.sortingOrder;

        leftObj.transform.localScale = sr.transform.localScale;
        rightObj.transform.localScale = sr.transform.localScale;

        Vector3 basePos = sr.transform.position;
        float worldLeftWidth = leftWidth / ppu;
        float worldRightWidth = rightWidth / ppu;

        leftObj.transform.position = basePos + Vector3.left * (worldRightWidth / 2f);
        rightObj.transform.position = basePos + Vector3.right * (worldLeftWidth / 2f);

        AddSliceList(leftObj);
        AddSliceList(rightObj);

        // 움직임
        Rigidbody2D leftRb = leftObj.AddComponent<Rigidbody2D>();
        Rigidbody2D rightRb = rightObj.AddComponent<Rigidbody2D>();
        leftRb.gravityScale = 0f;
        rightRb.gravityScale = 0f;
        leftRb.linearDamping = 4f;
        rightRb.linearDamping = 4f;

        Vector2 distance = Vector2.right * power;
        leftRb.AddForce(-distance, ForceMode2D.Impulse);
        rightRb.AddForce(distance, ForceMode2D.Impulse);

        // 삭제
        sliceList.Remove(sliceObj);
        Destroy(sliceObj);
    }
}
