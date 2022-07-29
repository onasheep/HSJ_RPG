using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Monster : MonsterMovement, BattleSystem
{

    public enum STATE
    {
        NONE, CREAT, WAITING, BATTLE, DEATH
    }
    public STATE myState = STATE.NONE;

    float _curHP = 0.0f;
    public CharacterStat myStat;

    public float HP
    {
        get
        {
            return _curHP;
        }
        set
        {
            _curHP += value;
            if (_curHP < 0.0f) _curHP = 0.0f;
            //myStatBar.myHP.value = _curHP / myStat.HP;
        }
    }

    AIPerception _aiperception = null;
    Vector3 StartPos;
    AIPerception myPerceptoion
    {
        get
        {
            if (_aiperception == null)
            {
                _aiperception = this.GetComponentInChildren<AIPerception>();

            }
            return _aiperception;
        }
    }
    //

    void FindTarget()
    {
        ChangeState(STATE.BATTLE);

    }


    public bool IsLive()
    {
        return myState == STATE.WAITING || myState == STATE.BATTLE;
    }

    public void OnDamage(int Damage)
    {
        if (myState == STATE.DEATH) return;
        HP = -Damage;
        if (HP <= 0.0f)
        {
            ChangeState(STATE.DEATH);

        }
        else
        myAnim.SetTrigger("Damage");
    }

    //
    void Start()
    {
        ChangeState(STATE.CREAT);

    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();

    }

    IEnumerator Waitting(float t, UnityAction done)
    {
        yield return new WaitForSeconds(t);
        done?.Invoke();
    }



    void ChangeState(STATE s)
    {
        if (myState == s) return;   // 상태가 바뀔때 원래 상태와 같은 경우는 다
        myState = s;
        switch (myState)
        {
            case STATE.CREAT:
                //_curHP = myStat.HP;
                myPerceptoion.FindTarget = FindTarget;
                StartPos = this.transform.position;
                //myAnimEvent.Attack += OnAttack;

                ChangeState(STATE.WAITING);
                break;
            case STATE.WAITING:
                break;
            case STATE.BATTLE:
                StopAllCoroutines();

                base.AttackTarget(myPerceptoion.myTarget, myStat.AttackRange, myStat.AttackDelay, () => ChangeState(STATE.WAITING));
                break;
            case STATE.DEATH:
                base.StopAllCoroutines();
                myStat.HP = 0.0f;
                myAnim.SetTrigger("Death");
                StartCoroutine(Disapearing());

                break;
        }
    }
    void StateProcess()        // 업데이트문에서 호출될꺼고 각각 업데이트 상
    {
        switch (myState)
        {
            case STATE.CREAT:
                break;
            case STATE.WAITING:
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEATH:
                break;
        }
    }

    IEnumerator Disapearing()
    {
        yield return new WaitForSeconds(3.0f);
        //Destroy(myStatBar.gameObject);
        float dist = 1.0f;
        while (dist > 0.0f)
        {
            float delta = Time.deltaTime * 0.5f;
            this.transform.Translate(-Vector3.up * Time.deltaTime);
            dist -= delta;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
