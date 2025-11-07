using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class d6 : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer;
    public float distance = 0.3f; 
    public float speed = 3f;      
    public bool destroyOriginal = true;

    private List<GameObject> slices = new List<GameObject>();
    private float smallestRightPercent = 100f; 
    private float originalWidthWorld;           
    private Vector3 originalLeftEdge;          

    void Start()
    {
        if (targetSpriteRenderer != null)
        {
            AddSlice(targetSpriteRenderer.gameObject);
            originalWidthWorld = targetSpriteRenderer.bounds.size.x;
            originalLeftEdge = targetSpriteRenderer.bounds.min;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SlicePosition();
        }
    }
    void SlicePosition()
    {
        if (slices.Count == 0) return;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        GameObject clickedSlice = GetSliceUnderMouse(worldPos);
        if (clickedSlice == null) return;
        Slice(clickedSlice, worldPos);
    }
    GameObject GetSliceUnderMouse(Vector3 worldPos)
    {
        foreach (var slice in slices)
        {
            var col = slice.GetComponent<BoxCollider2D>();
            if (col != null && col.OverlapPoint(worldPos))
                return slice;
        }
        return null;
    }

    void AddSlice(GameObject obj)
    {
        if (obj.GetComponent<BoxCollider2D>() == null)
            obj.AddComponent<BoxCollider2D>();

        var sr = obj.GetComponent<SpriteRenderer>();
        var col = obj.GetComponent<BoxCollider2D>();
        if (sr.sprite != null)
        {
            col.size = sr.sprite.bounds.size;
            col.offset = sr.sprite.bounds.center;
        }

        slices.Add(obj);
    }

    void Slice(GameObject sliceObj, Vector3 worldClick)
    {
        SpriteRenderer sr = sliceObj.GetComponent<SpriteRenderer>();
        Sprite sprite = sr.sprite;
        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height
        );
        tex.SetPixels(pixels);
        tex.Apply();
        float totalDistanceFromLeft = worldClick.x - originalLeftEdge.x;
        float rightPercent = Mathf.Clamp01(1f - (totalDistanceFromLeft / originalWidthWorld)) * 100f;

        if (rightPercent < smallestRightPercent)
        {
            smallestRightPercent = rightPercent;
        }
    
        Vector3 localClick = sr.transform.InverseTransformPoint(worldClick);
        float ppu = sprite.pixelsPerUnit;
        Vector2 pivot = sprite.pivot;
        int texWidth = tex.width;
        int texHeight = tex.height;

        int clickX = Mathf.RoundToInt(localClick.x * ppu + pivot.x);
        clickX = Mathf.Clamp(clickX, 1, texWidth - 1);

        int leftWidth = clickX;
        int rightWidth = texWidth - clickX;

        Texture2D leftTex = new Texture2D(leftWidth, texHeight);
        Texture2D rightTex = new Texture2D(rightWidth, texHeight);
        leftTex.SetPixels(tex.GetPixels(0, 0, leftWidth, texHeight));
        rightTex.SetPixels(tex.GetPixels(clickX, 0, rightWidth, texHeight));
        leftTex.Apply();
        rightTex.Apply();

        Sprite leftSprite = Sprite.Create(leftTex, new Rect(0, 0, leftWidth, texHeight), new Vector2(0.5f, 0.5f), ppu);
        Sprite rightSprite = Sprite.Create(rightTex, new Rect(0, 0, rightWidth, texHeight), new Vector2(0.5f, 0.5f), ppu);

        GameObject leftObj = new GameObject("LeftSlice", typeof(SpriteRenderer));
        GameObject rightObj = new GameObject("RightSlice", typeof(SpriteRenderer));

        SpriteRenderer leftR = leftObj.GetComponent<SpriteRenderer>();
        SpriteRenderer rightR = rightObj.GetComponent<SpriteRenderer>();
        leftR.sprite = leftSprite;
        rightR.sprite = rightSprite;
        leftR.sortingOrder = sr.sortingOrder;
        rightR.sortingOrder = sr.sortingOrder;

        leftObj.transform.localScale = sr.transform.localScale;
        rightObj.transform.localScale = sr.transform.localScale;

        Vector3 basePos = sr.transform.position;
        float worldLeftWidth = leftWidth / ppu;
        float worldRightWidth = rightWidth / ppu;

        leftObj.transform.position = basePos + Vector3.left * (worldRightWidth / 2f);
        rightObj.transform.position = basePos + Vector3.right * (worldLeftWidth / 2f);

        AddSlice(leftObj);
        AddSlice(rightObj);

        if (destroyOriginal)
        {
            slices.Remove(sliceObj);
            Destroy(sliceObj);
        }

        StartCoroutine(Move(leftObj.transform));
    }

    IEnumerator Move(Transform left)
    {
        Vector3 target = left.position + Vector3.left * distance;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            left.position = Vector3.Lerp(left.position, target, t);
            yield return null;
        }
    }
}
