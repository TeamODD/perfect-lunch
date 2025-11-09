using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    [SerializeField] GameObject setting;
    [SerializeField] AudioSource click;
    [SerializeField] bool settingon;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!settingon)
            {
                ClickSetting();
            }
            else
            {
                ExitSetting();
            }
        }
    }
    public void ClickSetting()
    {
        settingon=true;
        click.Play();
        setting.SetActive(true);
    }
    public void ExitSetting()
    {
        settingon =false;
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
