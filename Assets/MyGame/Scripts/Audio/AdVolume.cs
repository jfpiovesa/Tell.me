using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class AdVolume : MonoBehaviour
{

    public static AdVolume au;

    [Header("Audio Manager")]
    public AudioMixer audioMixer;
    private float volumenivel;
    private float vlr;
    public Slider sl;
    public Sprite[] statBtnImage;
    public Image audioStat;

    private void Awake()
    {
        if (au == null)
        {
            au = this;
        }
    }
    private void Start()
    {
        vlr = PlayerPrefs.GetFloat("audiovolume", volumenivel);
        sl.value = vlr;
        volumenivel = vlr;
    }

    private void Update()
    {
        if(volumenivel <= -80)
        {
            audioStat.sprite =  statBtnImage[1];
        }else
        {
            audioStat.sprite = statBtnImage[0];
        }
    }
    public void SetVolume(float volume)
    {
        volumenivel = volume;
        audioMixer.SetFloat("volumeaudio", volumenivel);
        PlayerPrefs.SetFloat("audiovolume", volumenivel);
        sl.value = volumenivel;


    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void Mutar( )
    {
        
        
            vlr = -80;
            volumenivel = -80;
            audioMixer.SetFloat("volumeaudio", volumenivel);
            PlayerPrefs.SetFloat("audiovolume", volumenivel);
        
       
    }
}
