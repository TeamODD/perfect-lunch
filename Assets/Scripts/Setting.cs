using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    [SerializeField] GameObject setting;

    public void ClickSetting()
    {
        setting.SetActive(true);
    }
    public void ExitSetting()
    {
        setting.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }
    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }
}
