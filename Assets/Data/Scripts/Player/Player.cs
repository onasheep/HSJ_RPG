using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : Character, BattleSystem
{

    public enum STATE
    {
        NONE, CREATE, PLAY, DEAD
    }
    public STATE myState = STATE.NONE;

    public LayerMask PickingMask;
    public LayerMask InteractiveMask;
    public LayerMask AttackMask;

    public GameObject GameOver;
    public GameObject MoveMarker;
    private GameObject obj;


    public SoundManager mySound;

    public Transform myWeapon;

    public GameObject levelUpEffect;
   

    public CharacterStat myStat;
    float count = 0;
  

    public bool IsLive()
    {
        return myState == STATE.PLAY;
    }
    public void OnDamage(float Damage)
    {
        if (myState != STATE.PLAY) return;
        if (myStat.HP > 0.0f)
        {
            // 0 ���Ϸ� �������� �Ǹ� 1�� �������� ����
            Damage -= myStat.DEF;
            if(Damage <= 0)
            {
                Damage = 1.0f;
            }
            myAnim.SetTrigger("Damage");
            myStat.HP -= Damage;


        }
        if ( myStat.HP <= 0.0f)
        {
            ChangeState(STATE.DEAD);
        }
    }

    public void OnAttack()
    {
        Collider[] list = Physics.OverlapSphere(myWeapon.position, 1.0f, AttackMask); // �ѹ� �ֵθ��� �������� �����������Ƿ� �迭���·� ���ϵ�
        foreach (Collider col in list)
        {
            BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
            if (bs != null)
            {
                bs.OnDamage(myStat.ATK);
            }
            
        }

    }
    //
    void Start()
    {
        ChangeState(STATE.CREATE);
    }
    // ������� 


    void Update()
    {
        StateProcese();
    }


    #region Picking �̵�/ ���� 
    void Picking()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 9999.9f, PickingMask))
        {
            // ��Ŭ���� �̵� , IsPointerOverGameObject �� ���ؼ� ���� ���콺�� UI���� �ִ� ��찡 �ƴ� ��쿡�� Ŭ���̺�Ʈ�� �� �� �ֵ��� �Ѵ�.
            if (Input.GetMouseButtonDown(1) & !EventSystem.current.IsPointerOverGameObject())
            {
                DestroyMarker();
                int mask = 1 << NavMesh.GetAreaFromName("Walkable");
                if (NavMesh.CalculatePath(this.transform.position, hit.point, mask, path))
                {
                    base.MoveByNav(path.corners, () => DestroyMarker());
                }
                obj = Instantiate(MoveMarker, hit.point + new Vector3(0.0f, 0.1f, 0.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
            }

            // ��Ŭ���� �Ϲݰ��� , IsPointerOverGameObject �� ���ؼ� ���� ���콺�� UI���� �ִ� ��찡 �ƴ� ��쿡�� Ŭ���̺�Ʈ�� �� �� �ֵ��� �Ѵ�.
            else if (Input.GetMouseButtonDown(0) & !EventSystem.current.IsPointerOverGameObject())
            {
                // ��� �ڷ�ƾ ���� �� NavMeshPath ��ã�� �ʱ�ȭ 
                StopAllCoroutines();
                myNav.ResetPath();

                DestroyMarker();
                base.RotateWhenAttacking(hit.point);
                myAnim.SetBool("Run", false);
                myAnim.SetTrigger("Attack1");
            }
            
        }

    }
    #endregion 

    void Skill()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            StopAllCoroutines();
            myNav.ResetPath();
            myAnim.SetTrigger("QSkill");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StopAllCoroutines();
            myNav.ResetPath();
            myAnim.SetTrigger("WSkill");

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopAllCoroutines();
            myNav.ResetPath();
            myAnim.SetTrigger("ESkill");

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
            myNav.ResetPath();
            myAnim.SetTrigger("RSkill");

        }
    }
    // �׺�Ž� ������ �۵��ϴ��� Ȯ��ġ������ �ϴ� ����
    /// <summary>
    /// ���� ������ �������� ����
    /// </summary>
    void StickOnGround()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 1.0f, PickingMask, QueryTriggerInteraction.Collide))
        {
            Mathf.Lerp(this.transform.position.y, hit.point.y, 1.0f);           
        }

    }

    void Dodging()
    {
        myNav.ResetPath();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 9999.9f, PickingMask))
        {
            base.DodgeToPosition(hit.point);
        }
    }

    #region ��Ŀ ����
    void DestroyMarker()
    {
        Destroy(obj);
    }
    #endregion
 
    void GetItem()
    {
      
    }


    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.CREATE:
                mySound.GetComponent<AudioSource>().clip = mySound.BGM;
                mySound.GetComponent<AudioSource>().Play();
                myAnimEvent.Attack += OnAttack;
                SetInitializeStat();
                ChangeState(STATE.PLAY);
                break;
            case STATE.PLAY:
                myAnim.SetBool("Moveable", true);
                break;
            case STATE.DEAD:
                mySound.GetComponent<AudioSource>().clip = mySound.GameOver;
                mySound.GetComponent<AudioSource>().Play();
                base.StopAllCoroutines();
                myStat.HP = 0.0f;
                myAnim.SetTrigger("Dead");               
                break;
        }
    }

    void StateProcese()
    {
        switch (myState)
        {
            case STATE.CREATE:
                break;
            case STATE.PLAY:
                CalculateStat();
                #region NavMeshPath ����� �� 
                //for (int i = 0; i < path.corners.Length - 1; i++)
                //{
                //    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                //}
                #endregion
                if (Input.GetKeyDown(KeyCode.A))
                {
                    myStat.HP -= 10;
                    myStat.MP -= 10;
                }
                if (myAnim.GetBool("Moveable") & !myAnim.GetBool("IsAttacking"))
                {
                    Picking();
                }
                if (Input.GetKeyDown(KeyCode.Space) & myAnim.GetBool("Moveable") & !myAnim.GetBool("IsAttacking"))
                {
                    DestroyMarker();
                    Dodging();
                }
                if (myAnim.GetBool("Moveable") & !myAnim.GetBool("IsAttacking"))
                {
                    Skill();
                }

                   
                break;
            case STATE.DEAD:
                count += Time.deltaTime;
                if (count > 5.0f)
                {
                    GameOver.SetActive(true);
                    count = 0;
                }
                break;
        }

    }

 

    void CalculateStat()
    {

        // ���� �ؽ�Ʈ�� ����
        LVCenter.text = "LV:"+ myStat.LV.ToString();
        LVt.text = "LV : " + myStat.LV.ToString();
        EXPt.text = "EXP : " + myStat.EXP.ToString();
        HPt.text = "HP : " + myStat.HP.ToString();
        MPt.text = "MP : " + myStat.MP.ToString();
        ATKt.text = "ATK : " + myStat.ATK.ToString();
        DEFt.text = "DEF : " + myStat.DEF.ToString();




        // HP
        if (myStat.HP > myStat.MaxHP)
        {
            myStat.HP = myStat.MaxHP;

        }
        else if (myStat.HP <= 0)
        {
            myStat.HP = 0;
            ChangeState(STATE.DEAD);
        }
        if (myStat.HP == 0)
        {
            ChangeState(STATE.DEAD);
        }

        // MP
        if (myStat.MP > myStat.MaxMP)
        {
            myStat.MP = myStat.MaxMP;
        }
        else if (myStat.MP <= 0)
        {
            myStat.MP = 0;
        }

        // EXP
        if (myStat.EXP == myStat.MaxEXP)
        {
            LevelUp();


        }
        else if(myStat.EXP > myStat.MaxEXP)
        {
            float overEXP = myStat.EXP - myStat.MaxEXP;
            LevelUp();
            myStat.EXP += overEXP;
        }
    }


    public void SetInitializeStat()
    {
        // ���� �⺻��
        myStat.LV = 1;
        myStat.EXP = 0;
        myStat.HP = 100;
        myStat.MP = 50;
        myStat.ATK = 200;
        myStat.DEF = 10;
        myStat.MaxEXP = 30;
        myStat.MaxHP = myStat.HP;
        myStat.MaxMP = myStat.MP;
    }

    public void LevelUp()
    {
        Instantiate(levelUpEffect, this.transform);
            int level = myStat.LV;
            myStat.EXP = 0;
            myStat.LV++;
            myStat.MaxEXP += 10 * level;
            myStat.MaxHP += 20 * level;
            myStat.MaxMP += 10 * level;
            myStat.ATK += 5 * level;
            myStat.DEF += 5 * level;


            myStat.HP = myStat.MaxHP;
            myStat.MP = myStat.MaxMP;

    }

}
