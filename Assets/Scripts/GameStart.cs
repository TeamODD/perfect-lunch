using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStart : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Main");
    }
    public void QuitButton()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // 빌드된 게임에서는 완전 종료
        #endif
    }
}
