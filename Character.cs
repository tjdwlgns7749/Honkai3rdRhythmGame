using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody rigid;
    public Animator anim;
    public bool run = true;
    int StartLayer;
    int PlayAniLayer;
    CharacterManager characterMgr;
    public Vector3 resultscale;
    public Vector3 resultreturnscale;
    public int characternum;

    private void Start()
    {
        characterMgr = CharacterManager.Instance;
        StartLayer = LayerMask.NameToLayer("Character");
        PlayAniLayer = LayerMask.NameToLayer("PlayAnim");
    }

    private void OnEnable()
    {
        run = true;
    }

    public void SetPosX(Transform transform)
    {
        this.transform.position = transform.position;

    }

    public void ResultAction()
    {
        this.transform.rotation = Quaternion.Euler(0, 180, 0);
        this.transform.localScale = resultscale;
        anim.SetBool("Result", true);
        
    }

    public void LayerReset()
    {
        gameObject.layer = StartLayer;
    }


    public void CharacterMove(float noteSpeed, int bpm)
    {
        if (run)
            rigid.position += Vector3.left * noteSpeed * bpm * Time.deltaTime;
    }

    public void KickorMiss(string Anim, bool OnOff)
    {
        gameObject.layer = PlayAniLayer;

        if (Anim == "Hit")
        {
            run = true;
            anim.SetBool(Anim, OnOff);

        }
        else
        {
            run = false;
            anim.SetBool(Anim, OnOff);
        }
    }

    public void ResultDestroy()
    {
        gameObject.layer = StartLayer;
        this.transform.rotation = Quaternion.Euler(0, -90, 0);
        this.transform.localScale = resultreturnscale;
        characterMgr.ReturnObject(this);
    }

    public void CharacterDestroy()
    {
        gameObject.layer = StartLayer;
        characterMgr.ReturnObject(this);
    }
}

