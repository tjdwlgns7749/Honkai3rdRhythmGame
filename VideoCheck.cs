using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoCheck : MonoBehaviour
{
    public RawImage video;
    public Image resultImg;

    public void VideoActive(bool Active)
    {
        video.enabled = Active;
    }

    public void ImgActive(bool Active)
    {
        if (Active)
            resultImg.GetComponent<Image>().sprite = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].spriteBackGround;
        resultImg.enabled = Active;
    }
}
