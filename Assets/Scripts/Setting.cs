using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    [SerializeField] GameObject setting;
    [SerializeField] AudioSource click;

    public void ClickSetting()
    {
        click.Play();
        setting.SetActive(true);
    }
    public void ExitSetting()
    {
        click.Play();
        setting.SetActive(false);
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("Start");
    }
    public void Restart()
    {
        click.Play();
        SceneManager.LoadScene("Main");
    }
    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }
}
