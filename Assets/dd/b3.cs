using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class b3 : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer;//타겟 이미지
    public GameObject linePrefab;//선
    private GameObject currentGuideLine;

    public float distance = 0.3f;//벌어지는 거리
    public float speed = 3f;//벌어지는 속도

    public List<float> cutRatios = new List<float> { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 0.99f };//비율
    public float clickError = 0.04f;//오차

    private int currentCutIndex = 0;//%
    private float originalWidth;//원래길이
    private Vector3 originalLeftEdge;//가장 왼쪽
    private float currentStartRatio = 0f;//자른 비율 시작점

    void Start()
    {
        originalWidth = targetSpriteRenderer.bounds.size.x;
        originalLeftEdge = targetSpriteRenderer.bounds.min;
        ShowLine();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            Click(worldPos);
        }
    }

    void ShowLine()//선
    {
        if (currentCutIndex >= cutRatios.Count) return;
        float targetRatio = cutRatios[currentCutIndex];//퍼센트
        float targetX = originalLeftEdge.x + originalWidth * targetRatio;//위치

        currentGuideLine = Instantiate(linePrefab);//새로 생성
        currentGuideLine.transform.position = new Vector3(targetX, targetSpriteRenderer.transform.position.y, 0);
    }

    void Click(Vector3 worldClick)
    {
        if (currentCutIndex >= cutRatios.Count) return;
        float targetRatio = cutRatios[currentCutIndex];
        float targetX = originalLeftEdge.x + originalWidth * targetRatio;
        float clickX = worldClick.x;//클릭좌표

        if (Mathf.Abs(clickX - targetX) <= originalWidth * clickError)//오차가 오차 이내면
        {
            Slicing(targetRatio);
            Destroy(currentGuideLine);
            currentCutIndex++;
            ShowLine();
        }
    }

    void Slicing(float targetRatio)
    {
        Sprite sprite = targetSpriteRenderer.sprite;
        Texture2D tex = sprite.texture;
        Rect texRect = sprite.textureRect;

        Texture2D texCopy = new Texture2D((int)texRect.width, (int)texRect.height);
        texCopy.SetPixels(tex.GetPixels((int)texRect.x, (int)texRect.y, (int)texRect.width, (int)texRect.height));//픽셀 가져오기
        texCopy.Apply();
        
        float pixelsPerUnit = sprite.pixelsPerUnit;//잘린 조각 비율
        int totalWidth = texCopy.width;//전체 길이

        int sliceX = Mathf.RoundToInt((targetRatio - currentStartRatio) * totalWidth / (1f - currentStartRatio));//자를 곳
        sliceX = Mathf.Clamp(sliceX, 1, texCopy.width - 1);//최소

        Texture2D leftTex = new Texture2D(sliceX, texCopy.height);//왼
        Texture2D rightTex = new Texture2D(texCopy.width - sliceX, texCopy.height);//오
        leftTex.SetPixels(texCopy.GetPixels(0, 0, sliceX, texCopy.height));//왼
        rightTex.SetPixels(texCopy.GetPixels(sliceX, 0, texCopy.width - sliceX, texCopy.height));//오
        leftTex.Apply();
        rightTex.Apply();
        Sprite leftSprite = Sprite.Create(leftTex, new Rect(0, 0, leftTex.width, leftTex.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);//왼
        Sprite rightSprite = Sprite.Create(rightTex, new Rect(0, 0, rightTex.width, rightTex.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);//오

        GameObject leftObj = new GameObject("LeftSlice", typeof(SpriteRenderer));//오브젝트화
        GameObject rightObj = new GameObject("RightSlice", typeof(SpriteRenderer));

        SpriteRenderer leftR = leftObj.GetComponent<SpriteRenderer>();//가져오기
        SpriteRenderer rightR = rightObj.GetComponent<SpriteRenderer>();
        leftR.sprite = leftSprite;//이미지
        rightR.sprite = rightSprite;

        Vector3 basePos = targetSpriteRenderer.transform.position;//배치 기준
        float worldLeftWidth = leftTex.width / pixelsPerUnit;
        float worldRightWidth = rightTex.width / pixelsPerUnit;
        leftObj.transform.position = basePos + Vector3.left * (worldRightWidth / 2f);
        rightObj.transform.position = basePos + Vector3.right * (worldLeftWidth / 2f);

        leftObj.transform.localScale = rightObj.transform.localScale = targetSpriteRenderer.transform.localScale;//크기
        leftR.sortingOrder = rightR.sortingOrder = targetSpriteRenderer.sortingOrder;//순서
        Destroy(targetSpriteRenderer.gameObject);//원본 파괴

        targetSpriteRenderer = rightR;//새 타겟 지정
        currentStartRatio = targetRatio;

        float rightPercent = (1f - targetRatio) * 100f;//계산
        Debug.Log("{currentCutIndex + 1}번째. 오른쪽: {rightPercent:F1}%");

        StartCoroutine(Move(leftObj.transform));
    }

    IEnumerator Move(Transform left)
    {
        Vector3 targetPos = left.position + Vector3.left * distance;
        Vector3 start = left.position;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            left.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }
    }
}
