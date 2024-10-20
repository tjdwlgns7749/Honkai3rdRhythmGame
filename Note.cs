using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public int noteNum;

    NoteManager noteMgr;
    public Rigidbody2D rigid2d;

    private void Start()
    {
        noteMgr = NoteManager.Instance;
    }

    public void SetSpawnPosX(Transform transform)
    {
        this.transform.position = transform.position;
    }


    public void NoteMove(int BPM, float noteSpeed)
    {
        rigid2d.position += Vector2.left * noteSpeed * BPM * Time.deltaTime;
    }

    public void NoteDestroy()
    {
        //NoteManager -> NoteReturn »£√‚
        noteMgr.ReturnObject(this);

    }
}



