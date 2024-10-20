using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumOption : MonoBehaviour
{
    public Slider BGMvolum;
    public Slider SFXvolum;

    AudioManager audioMgr;

    private void OnEnable()
    {
        audioMgr = AudioManager.Instance;
    }

    private void Update()
    {
        audioMgr.bgmSource.volume = BGMvolum.value;
        audioMgr.sfxSource.volume = SFXvolum.value;
    }

    public void SFXtest()
    {
        audioMgr.PlaySFX("SFX_Clap1");
    }
}
