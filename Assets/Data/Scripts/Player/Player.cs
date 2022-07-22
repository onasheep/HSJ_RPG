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
        // �⺻ ���� �޺� �νĿ� ���۳�Ʈ 
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
                #region NavMeshPath ����� �� 
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

        // ���� �ؽ�Ʈ�� ����
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
