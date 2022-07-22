using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System;

public class Character : MonoBehaviour
{
    #region 애니메이터
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = this.GetComponentInChildren<Animator>();
            }
            return _anim;
        }

    }
    #endregion

    #region 내비매쉬 에이전트
    NavMeshAgent _navagent = null;
    protected NavMeshAgent myNav
    {
        get
        {
            if (_navagent == null)
            {
                _navagent = this.GetComponentInChildren<NavMeshAgent>();
            }
            return _navagent;
        }
    }
    #endregion
    
    [Serializable]
    public struct CharacterStat
    {
        public int LV;
        public int HP;
        public int MP;
        public int EXP;
        public int ATK;
        public int DEF;
        public int MaxHP;
        public int MaxMP;
        public int MaxEXP;
    }
    [Header("캐릭터스탯")]
    public CharacterStat mystat;

    #region 텍스트
    // 개별 텍스트 오브젝트
    public TMPro.TMP_Text LVCenter;
    public TMPro.TMP_Text LVt;
    public TMPro.TMP_Text EXPt;
    public TMPro.TMP_Text HPt;
    public TMPro.TMP_Text MPt;
    public TMPro.TMP_Text ATKt;
    public TMPro.TMP_Text DEFt;
    #endregion

    Coroutine moveRoutine = null;

    float RotSpeed = 360.0f;
    Coroutine rotRoutine = null;
    float Angle;
    float Dir;

    // 네브매쉬 패스 
    public NavMeshPath path = null;

    Coroutine dodgeRoutine = null;


    private void Awake()
    {
        path = new NavMeshPath();
    }

    protected void MoveByNav(Vector3[] poslist,UnityAction done)
    {
        StopAllCoroutines();
        StartCoroutine(MovingByNav(poslist, done));
    }

    IEnumerator MovingByNav(Vector3[] poslist,UnityAction done)
    {
        for (int i = 1; i < poslist.Length; i++)
        {
            if (rotRoutine != null) StopCoroutine(rotRoutine);
            rotRoutine = StartCoroutine(Rotating(poslist[i]));
            if (moveRoutine != null) StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(Moving((poslist[i]), done));
            yield return moveRoutine;
        }

    }

    IEnumerator Moving(Vector3 pos, UnityAction done)
    {
        Vector3 dir = pos - this.transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        while (dist > Mathf.Epsilon)
        {
            float delta = myNav.speed * Time.deltaTime;
            if (dist < delta)
            {
                delta = dist;
            }
            else
            {
                myAnim.SetBool("Run", true);
                myNav.SetDestination(pos);
            }
            dist -= delta;
            yield return null;
        }
        // 클릭 지점으로 이동완료후 마커 삭제
        myAnim.SetBool("Run", false);
        done?.Invoke();
        moveRoutine = null;


    }
    protected void DodgeToPosition(Vector3 pos)
    {
        if (dodgeRoutine != null)
        {
            StopCoroutine(dodgeRoutine);
        }
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);

        }
        dodgeRoutine = StartCoroutine(Dodge(pos));
        myAnim.SetBool("Run", false);

        // 닷지 중 움직임 코루틴 정지 


        if (rotRoutine != null)
        {
            StopCoroutine(rotRoutine);
        }
        rotRoutine = StartCoroutine(Rotating(pos));


    }
    IEnumerator Dodge(Vector3 pos)
    {
        float dodgeSpeed = 5.0f;
        Vector3 dir = pos - this.transform.position;
        float dist = 5.0f;
        dir.Normalize();
        myAnim.SetTrigger("Dodge");
        while (dist > Mathf.Epsilon)
        {
            float delta = dodgeSpeed * Time.deltaTime;
            if (dist < delta)
            {
                delta = dist;
            }

            else
            {
               this.transform.Translate(dir * delta, Space.World);
            }
            dist -= delta;
            yield return null;
        }
        dodgeRoutine = null;
    }
    protected void RotateWhenAttacking(Vector3 pos)
    {
        if (rotRoutine != null) StopCoroutine(rotRoutine);
        rotRoutine = StartCoroutine(Rotating(pos));
        
    }
    IEnumerator Rotating(Vector3 pos)
    {
        Vector3 dir = (pos - this.transform.position).normalized;
        CalAngle(myAnim.transform.forward, dir, myAnim.transform.right);
        while (Angle > Mathf.Epsilon)
        {
            float delta = RotSpeed * Time.deltaTime;
            if (Angle < delta)
            {
                delta = Angle;

            }
            myAnim.transform.Rotate(Vector3.up * delta * Dir);
            Angle -= delta;
            yield return null;
        }
        rotRoutine = null;
    }

    public void CalAngle(Vector3 src, Vector3 des, Vector3 right)
    {
        float r = Mathf.Acos(Vector3.Dot(src, des));
        Angle = 180.0f * (r / Mathf.PI);
        Dir = 1.0f;
        if (Vector3.Dot(right, des) < 0.0f)
        {
            Dir = -1.0f;
        }
    }


    public void SetInitializeStat()
    {
        // 스탯 기본값
        mystat.LV = 1;
        mystat.EXP = 0;
        mystat.HP = 100;
        mystat.MP = 50;
        mystat.ATK = 10;
        mystat.DEF = 10;
        mystat.MaxHP = mystat.HP;
        mystat.MaxMP = mystat.MP;
    }
}

