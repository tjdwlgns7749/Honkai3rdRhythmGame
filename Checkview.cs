using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkview : MonoBehaviour
{
    UIManager uiMgr;

    public Rigidbody2D rigid2d;
    public float Speed = 1.0f;
    float StartTime = 0.0f;
    public Check eCheck;

    private void Start()
    {
        uiMgr = UIManager.Instance;
    }

    private void OnEnable()
    {
        StartCoroutine(view());
    }

    public void SetPosX(Transform transform)
    {
        this.transform.position = transform.position;
    }
    public void Upmove()
    {
        rigid2d.position += Vector2.up * Speed * Time.deltaTime;
    }

    IEnumerator view()
    {
        StartTime = Time.time;

        while(Time.time - StartTime < 0.5f)
        {
            Upmove();
            yield return null;
        }
        uiMgr.ReturnObject(this);
    }
}
