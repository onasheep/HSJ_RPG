using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : Character
{

    public enum STATE
    {
        NONE, CREATE, PLAY, DEAD
    }
    public STATE mystate = STATE.NONE;
    public LayerMask PickingMask;
    public LayerMask InteractiveMask;

    public GameObject MoveMarker;
    private GameObject obj;




    void Start()
    {
        // 기본 공격 콤보 인식용 컴퍼넌트 
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
    void Interactive()
    {
        
    }


    void ChangeState(STATE s)
    {
        if (mystate == s) return;
        mystate = s;
        switch (mystate)
        {
            case STATE.CREATE:
                SetInitializeStat();
                ChangeState(STATE.PLAY);
                break;
            case STATE.PLAY:
                myAnim.SetBool("Moveable", true);
                break;
            case STATE.DEAD:
                break;
        }
    }

    void StateProcese()
    {
        switch (mystate)
        {
            case STATE.CREATE:
                break;
            case STATE.PLAY:
                #region NavMeshPath 디버그 선 
                //for (int i = 0; i < path.corners.Length - 1; i++)
                //{
                //    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                //}
                #endregion
                if (Input.GetKeyDown(KeyCode.A))
                {
                    mystat.HP -= 10;
                    mystat.MP -= 10;
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

                CalculateStat();
                break;
            case STATE.DEAD:
                break;
        }

    }

 

    void CalculateStat()
    {

        // 스탯 텍스트에 저장
        LVt.text = "LV : " + mystat.LV.ToString();
        EXPt.text = "EXP : " + mystat.EXP.ToString();
        HPt.text = "HP : " + mystat.HP.ToString();
        MPt.text = "MP : " + mystat.MP.ToString();
        ATKt.text = "ATK : " + mystat.ATK.ToString();
        DEFt.text = "DEF : " + mystat.DEF.ToString();





        if (mystat.HP > mystat.MaxHP)
        {
            mystat.HP = mystat.MaxHP;

        }
        else if (mystat.HP <= 0)
        {
            mystat.HP = 0;
        }


        if (mystat.MP > mystat.MaxMP)
        {
            mystat.MP = mystat.MaxMP;
        }
        else if (mystat.MP <= 0)
        {
            mystat.MP = 0;
        }
    }



}
