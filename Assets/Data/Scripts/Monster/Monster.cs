using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Monster : MonsterMovement, BattleSystem
{

    public enum STATE
    {
        NONE, CREAT, WAITING, BATTLE, DEATH
    }
    public STATE myState = STATE.NONE;

    public CharacterStat myStat;
    public ItemDropper myDropper;
    
    public Transform DamageTextPos;
    public GameObject DamageTextPrefabs;

    public Player myPlayer;

    Vector3 StartPos;

    AIPerception _aiperception = null;
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
   
    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEATH) return;

        if (myStat.HP > 0.0f)
        {
            Damage -= myStat.DEF;
            if (Damage <= 0)
            {
                Damage = 1.0f;
            }
            myAnim.SetTrigger("Damage");
            myStat.HP -= Damage;

            // 데미지 텍스트 출력
            GameObject DamageText = GameObject.Instantiate(DamageTextPrefabs);
            DamageText.transform.SetParent(DamageTextPos);
            DamageText.transform.position = DamageTextPos.position;
            DamageText.transform.localRotation = Quaternion.Euler(45.0f, 0.0f, 0.0f);
            DamageText.GetComponent<DamageText>().damage = Damage;

        }
        if (myStat.HP <= 0)
        {
            ChangeState(STATE.DEATH);
        }

    }
    void OnAttack()
    {
        myPerceptoion.myTarget.OnDamage(myStat.ATK);
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
        if (myState == s) return;   
        myState = s;
        switch (myState)
        {
            case STATE.CREAT:
                myPerceptoion.FindTarget = FindTarget;
                StartPos = this.transform.position;
                myAnimEvent.Attack += OnAttack;

                ChangeState(STATE.WAITING);
                break;
            case STATE.WAITING:
                break;
            case STATE.BATTLE:
                StopAllCoroutines();

                base.AttackTarget(myPerceptoion.myTarget, myStat.AttackRange, myStat.AttackDelay, () => ChangeState(STATE.WAITING));
                break;
            case STATE.DEATH:
                // 플레이어에게 경험치값 더해주기
                myPlayer.myStat.EXP += myStat.EXP;
                // 플레이어에게 골드값 더해주기
                myPlayer.myStat.Gold += myStat.Gold;
                base.StopAllCoroutines();
                this.GetComponent<Rigidbody>().isKinematic = true;
                myStat.HP = 0;               
                myAnim.SetTrigger("Dead");
                StartCoroutine(Disapearing());              
                break;
        }
    }
    void StateProcess()       
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
        float dist = 1.0f;
        myDropper.ItemDrop(this.transform); // 아이템 드롭 
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
