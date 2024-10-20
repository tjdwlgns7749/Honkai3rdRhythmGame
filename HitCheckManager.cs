using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class HitCheckManager : MonoBehaviour
{
    GameManager gameMgr;
    AudioManager audioMgr;
    public Effect effect;

    //��Ʈ
    float PosX;//��Ʈ�񱳿� X��ǥ
    int TargetNumber; //���� ���� ��Ʈ�� �迭��ȣ
    public Transform CheckBoxPos;
    public Transform DeadPos;
    public Vector2 HitBoxsize;
    public Vector2 DeadBoxsize;
    public LayerMask whatIsLayer;

    //ĳ����
    float cPosX;
    int cTargetNumber;
    public Transform CharacterBoxPos;
    public LayerMask CharacterLayer;

    //TestGizmo
    public Vector2 PerpectBoxsize;
    public Vector2 GoodBoxsize;
    public Vector3 CharacterBoxSize;
    public Vector3 test;

    Check eCheck;

    private void Start()
    {
        gameMgr = GameManager.Instance;
        audioMgr = AudioManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale == 1)//�����̽��� �Է�
        {
            //Debug.Log(audioMgr.bgmSource.time);
            if (NoteCheck())
                CharacterCheck();
        }
    }

    bool NoteCheck()
    {
        Collider2D[] HitBox = Physics2D.OverlapCapsuleAll(CheckBoxPos.position, HitBoxsize, 0, whatIsLayer);

        if (HitBox.Length > 0)//��Ʈ�� �մ��� ������ Ȯ��
        {

            PosX = HitBox[0].GetComponent<Rigidbody2D>().position.x;
            TargetNumber = 0;

            for (int i = 1; i < HitBox.Length; i++)
            {
                var rigid = HitBox[i].GetComponent<Rigidbody2D>();
                if (PosX > rigid.position.x)
                {
                    PosX = rigid.position.x;
                    TargetNumber = i;
                }
            }
            effect.NoteHitEffect();
            audioMgr.PlaySFX("SFX_Clap1");
            ScoreCheck(CheckBoxPos.position.x - PosX);
            HitBox[TargetNumber].gameObject.GetComponent<Note>().NoteDestroy();
            return true;
        }
        return false;
    }

    void CharacterCheck()
    {
        Collider[] cHitBox = Physics.OverlapBox(CharacterBoxPos.position + test, CharacterBoxSize, Quaternion.identity, CharacterLayer);

        if (cHitBox.Length > 0)
        {

            cPosX = cHitBox[0].GetComponent<Rigidbody>().position.x;

            cTargetNumber = 0;

            for (int i = 1; i < cHitBox.Length; i++)
            {

                var cRigid = cHitBox[i].GetComponent<Rigidbody>();
                if (cPosX > cRigid.position.x)
                {
                    cPosX = cRigid.position.x;
                    cTargetNumber = i;
                }
            }
            cHitBox[cTargetNumber].gameObject.GetComponent<Character>().KickorMiss("Hit", true);
        }
    }



    void ScoreCheck(float a)//�����ϰ� ���ӸŴ����� �ѷ��� �Լ� MISS�� Note����
    {
        if (a < 0)
            a = a * -1;

        if (a >= 0 && a <= 20)
            eCheck = Check.PERFECT;
        else if (a > 20 && a <= 50)
            eCheck = Check.GOOD;


        gameMgr.GetCheck(eCheck);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;//Hit
        Gizmos.DrawWireCube(CharacterBoxPos.position + test, CharacterBoxSize);

        Gizmos.color = Color.black;//Hit
        Gizmos.DrawWireCube(CheckBoxPos.position, HitBoxsize);

        Gizmos.color = Color.black;//Miss
        Gizmos.DrawWireCube(DeadPos.position, DeadBoxsize);



        Gizmos.color = Color.blue;//Good
        Gizmos.DrawWireCube(CheckBoxPos.position, GoodBoxsize);

        Gizmos.color = Color.yellow;//Perpect
        Gizmos.DrawWireCube(CheckBoxPos.position, PerpectBoxsize);

    }

}
