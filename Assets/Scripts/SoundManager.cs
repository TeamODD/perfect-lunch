using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    // 슬라이더
    public Slider SoundSlider;

    // 볼륨 조절
    public void SetVolme()
    {
        // 로그 연산 값 전달
        audioMixer.SetFloat("Vol", Mathf.Log10(SoundSlider.value) * 20);
    }
}
