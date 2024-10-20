using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image Loadingimage;
    public Image LoadgingBar;

    public Sprite[] ImageArr;

    float timer = 0;
    float interval = 0.03f;

    private void OnEnable()
    {
        LoadgingBar.fillAmount = 0.0f;
        int test = Random.Range(0, 3);
        Loadingimage.sprite = ImageArr[test];
       
    }



    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            LoadgingBar.fillAmount += 0.01f;
            timer = 0.0f;
        }
    }


}
