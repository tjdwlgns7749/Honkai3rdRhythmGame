using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    Slider slHP;
    public Image slimage;

    private void Start()
    {
        slHP = this.GetComponent<Slider>();
        Setting();
    }

    public void GetHP(int hp)
    {
        slHP.value = hp;

        if(slHP.value <= 0)
            slimage.GetComponent<Image>().enabled = false;
    }


    public void Setting()
    {
        slHP.value = 100;
        slimage.GetComponent<Image>().enabled = true;
    }
}
