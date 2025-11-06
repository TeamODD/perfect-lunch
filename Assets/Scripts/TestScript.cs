using UnityEngine;

public class TestScript : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Entry()
    {
        Debug.Log("Application loaded");
    }
}
