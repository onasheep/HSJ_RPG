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

    public GameObject HpParticle;
    public GameObject GameOver;
    public GameObject MoveMarker;
    private GameObject obj;

    public GameObject DamageTextPrefabs;

    
    public SoundManager mySound;

    public Transform myWeapon;
    
   

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
        Collider[] list = Physics.OverlapSphere(myWeapon.position, 2.0f, AttackMask); // 한번 휘두를떄 여러명이 맞을수있으므로 배열형태로 리턴됨
        foreach (Collider col in list)
        {
            BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
            if (bs != null)
            {
                GameObject DamageText = GameObject.Instantiate(DamageTextPrefabs);
                DamageText.transform.SetParent(col.gameObject.GetComponent<Monster>().DamageTextPos);
                DamageText.transform.position = col.gameObject.GetComponent<Monster>().DamageTextPos.position;
                DamageText.transform.localRotation = Quaternion.Euler(45.0f, 0.0f, 0.0f);
                DamageText.GetComponent<DamageText>().damage = myStat.ATK;
                bs.OnDamage(myStat.ATK);

            }
        }

    }
    //
    void Start()
    {
        ChangeState(STATE.CREATE);
    }
    // 변경사항 


    void Update()
    {
        StateProcese();
    }


    #region Picking 이동/ 공격 
    void Picking()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 9999.9f, PickingMask))
        {
            // 우클릭시 이동 , IsPointerOverGameObject 를 통해서 현재 마우스가 UI위에 있는 경우가 아닌 경우에만 클릭이벤트가 뜰 수 있도록 한다.
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

            // 좌클릭시 일반공격 , IsPointerOverGameObject 를 통해서 현재 마우스가 UI위에 있는 경우가 아닌 경우에만 클릭이벤트가 뜰 수 있도록 한다.
            else if (Input.GetMouseButtonDown(0) & !EventSystem.current.IsPointerOverGameObject())
            {
                // 모든 코루틴 정지 후 NavMeshPath 길찾기 초기화 
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
    // 네브매쉬 때문에 작동하는지 확실치않으니 일단 보류
    /// <summary>
    /// 발이 땅에서 떨이지지 않음
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

    #region 마커 제거
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
                #region NavMeshPath 디버그 선 
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

        // 스탯 텍스트에 저장
        LVt.text = "LV : " + myStat.LV.ToString();
        EXPt.text = "EXP : " + myStat.EXP.ToString();
        HPt.text = "HP : " + myStat.HP.ToString();
        MPt.text = "MP : " + myStat.MP.ToString();
        ATKt.text = "ATK : " + myStat.ATK.ToString();
        DEFt.text = "DEF : " + myStat.DEF.ToString();





        if (myStat.HP > myStat.MaxHP)
        {
            myStat.HP = myStat.MaxHP;

        }
        else if (myStat.HP <= 0)
        {
            myStat.HP = 0;
            ChangeState(STATE.DEAD);
        }
        if (myStat.MP > myStat.MaxMP)
        {
            myStat.MP = myStat.MaxMP;
        }
        else if (myStat.MP <= 0)
        {
            myStat.MP = 0;
        }

        if(myStat.HP == 0)
        {
            ChangeState(STATE.DEAD);
        }
    }


    public void SetInitializeStat()
    {
        // 스탯 기본값
        myStat.LV = 1;
        myStat.EXP = 0;
        myStat.HP = 100;
        myStat.MP = 50;
        myStat.ATK = 10;
        myStat.DEF = 10;
        myStat.MaxHP = myStat.HP;
        myStat.MaxMP = myStat.MP;
    }

}
