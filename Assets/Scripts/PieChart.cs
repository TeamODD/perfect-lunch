using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    [SerializeField]
    private Image[] _partitionImages;

    public void Draw(float[] values)
    {
        if (values.Any(value => value < 0))
        {
            throw new ArgumentOutOfRangeException("All values must be positive");
        }
        if (values.Length > _partitionImages.Length)
        {
            throw new ArgumentOutOfRangeException("Too many values");
        }
        for (int i = 0; i < values.Length; i++)
        {
            float value = values[i];
            Image image = _partitionImages[i];
            image.fillAmount = value;
            if (i + 1 < _partitionImages.Length)
            {
                Image nextImage = _partitionImages[i + 1];
                float rotation = values[..(i + 1)].Sum() * 360;
                nextImage.transform.localRotation = Quaternion.AngleAxis(rotation, Vector3.forward);
            }
        }
    }

    public void DrawByPercent(int[] values)
    {
        if (values.Any(value => value < 0))
        {
            throw new ArgumentOutOfRangeException("All values must be positive");
        }
        float[] floatValues = values.Select(value => (float) value / 100).ToArray();
        Draw(floatValues);
    }

    private void Awake()
    {
        if (_partitionImages == null || _partitionImages.Length == 0)
        {
            throw new UnassignedReferenceException("The pie chart is not fully initialized.");
        }
    }

#if UNITY_EDITOR

    private void OnGUI()
    {
        if (GUILayout.Button("Demo Pie chart"))
        {
            int percent = 100;
            int[] values = new int[3];
            for (int i = 0; i < 3; i++)
            {
                int value = i + 1 == 3
                    ? percent
                    : UnityEngine.Random.Range(0, percent);
                values[i] = value;
                percent -= value;
            }
            Debug.LogFormat("Visualize values: [{0}]", string.Join(", ", values));
            DrawByPercent(values);
        }
    }

#endif
}
