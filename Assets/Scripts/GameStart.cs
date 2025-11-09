using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStart : MonoBehaviour
{
    public AudioSource AudioSource;
    public void StartButton()
    {
        AudioSource.Play();
        SceneManager.LoadScene("Main");
    }
    public void QuitButton()
    {
        AudioSource.Play();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // 빌드된 게임에서는 완전 종료
        #endif
    }
}
