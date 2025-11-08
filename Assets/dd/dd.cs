using UnityEngine;

public class dd : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer;
    public float separateDistance = 0.2f;
    public float separateSpeed = 3f;     
    public bool destroyOriginal = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SliceAtMousePosition();
        }
    }

    void SliceAtMousePosition()
    {
        if (targetSpriteRenderer == null) return;

        Sprite sprite = targetSpriteRenderer.sprite;
        Texture2D sourceTex = sprite.texture;
        Rect texRect = sprite.textureRect;

        Texture2D tex = new Texture2D((int)texRect.width, (int)texRect.height);
        tex.SetPixels(sourceTex.GetPixels(
            (int)texRect.x, (int)texRect.y,
            (int)texRect.width, (int)texRect.height
        ));
        tex.Apply();

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 localPos = targetSpriteRenderer.transform.InverseTransformPoint(worldPos);

        float pixelsPerUnit = sprite.pixelsPerUnit;
        Vector2 pivot = sprite.pivot;
        int texWidth = tex.width;
        int texHeight = tex.height;

        int clickX = Mathf.RoundToInt(localPos.x * pixelsPerUnit + pivot.x);
        clickX = Mathf.Clamp(clickX, 1, texWidth - 1);

        int leftWidth = clickX;
        int rightWidth = texWidth - clickX;

        Texture2D leftTex = new Texture2D(leftWidth, texHeight);
        Texture2D rightTex = new Texture2D(rightWidth, texHeight);

        leftTex.SetPixels(tex.GetPixels(0, 0, leftWidth, texHeight));
        rightTex.SetPixels(tex.GetPixels(clickX, 0, rightWidth, texHeight));
        leftTex.Apply();
        rightTex.Apply();

        Sprite leftSprite = Sprite.Create(leftTex, new Rect(0, 0, leftWidth, texHeight), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        Sprite rightSprite = Sprite.Create(rightTex, new Rect(0, 0, rightWidth, texHeight), new Vector2(0.5f, 0.5f), pixelsPerUnit);

        GameObject leftObj = new GameObject("LeftSlice", typeof(SpriteRenderer));
        GameObject rightObj = new GameObject("RightSlice", typeof(SpriteRenderer));

        SpriteRenderer leftR = leftObj.GetComponent<SpriteRenderer>();
        SpriteRenderer rightR = rightObj.GetComponent<SpriteRenderer>();

        leftR.sprite = leftSprite;
        rightR.sprite = rightSprite;

        Vector3 basePos = targetSpriteRenderer.transform.position;
        float worldLeftWidth = leftWidth / pixelsPerUnit;
        float worldRightWidth = rightWidth / pixelsPerUnit;

        leftObj.transform.position = basePos + Vector3.left * (worldRightWidth / 2f);
        rightObj.transform.position = basePos + Vector3.right * (worldLeftWidth / 2f);

        leftObj.transform.localScale = targetSpriteRenderer.transform.localScale;
        rightObj.transform.localScale = targetSpriteRenderer.transform.localScale;

        leftR.sortingOrder = targetSpriteRenderer.sortingOrder;
        rightR.sortingOrder = targetSpriteRenderer.sortingOrder;

        if (destroyOriginal)
            Destroy(targetSpriteRenderer.gameObject);

        StartCoroutine(Move(leftObj.transform, rightObj.transform));
    }

    System.Collections.IEnumerator Move(Transform left, Transform right)
    {
        Vector3 leftTarget = left.position + Vector3.left * separateDistance;
        Vector3 rightTarget = right.position + Vector3.right * separateDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * separateSpeed;
            left.position = Vector3.Lerp(left.position, leftTarget, t);
            right.position = Vector3.Lerp(right.position, rightTarget, t);
            yield return null;
        }
    }
}
