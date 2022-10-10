using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MonsterMovement : MonoBehaviour
{
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null) _anim = this.GetComponentInChildren<Animator>();
            return _anim;
        }
    }
    Rigidbody _rigid = null;
    protected Rigidbody myRigid
    {
        get
        {
            if (_rigid == null) _rigid = this.GetComponent<Rigidbody>();
            return _rigid;
        }
    }
    AnimEvent _animEvent = null;
    protected AnimEvent myAnimEvent
    {
        get
        {
            if (_animEvent == null)
            {
                _animEvent = this.GetComponentInChildren<AnimEvent>();
            }
            return _animEvent;
        }
    }
    public float MoveSpeed = 3.0f;
    public float SmoothMoveSpeed = 5.0f;

    float RotSpeed = 360.0f;
    Coroutine moveRoutine = null;
    Coroutine rotRoutine = null;

    Vector3 TargetPos = Vector3.zero;
    public bool bSmoothMove = true;
    protected void MoveToPosition(Vector3 pos, UnityAction done = null)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(Moving(pos, done));
        if (rotRoutine != null) StopCoroutine(rotRoutine);
        rotRoutine = StartCoroutine(Rotating(pos));
    }

    IEnumerator Moving(Vector3 pos, UnityAction done)
    {
        if (bSmoothMove) TargetPos = this.transform.position;
        Vector3 dir = pos - this.transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        myAnim.SetBool("IsWalking", true);

        while (dist > Mathf.Epsilon)
        {
            float delta = MoveSpeed * Time.deltaTime;
            if (dist < delta)
            {
                delta = dist;
            }
            if (bSmoothMove)
            {
                TargetPos += dir * delta;
                this.transform.position = Vector3.Lerp(this.transform.position, TargetPos, Time.deltaTime * SmoothMoveSpeed);
            }
            else
            {
                this.transform.Translate(dir * delta, Space.World);
            }
            dist -= delta;
            myAnim.SetBool("IsWalking", false);

            yield return null;
        }

        if (bSmoothMove)
        {
            while (Vector3.Distance(TargetPos, this.transform.position) > 0.01f)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, TargetPos, Time.deltaTime * SmoothMoveSpeed);
                yield return null;
            }
            this.transform.position = TargetPos;
        }

        moveRoutine = null;
        //목표지점에 도착
        //if (done != null) done();
        myAnim.SetBool("IsWalking", false);

        done?.Invoke();
    }

    IEnumerator Rotating(Vector3 pos)
    {
        Vector3 dir = (pos - this.transform.position).normalized;
        GameUtil.CalAngle(myAnim.transform.forward, dir, myAnim.transform.right, out ROTATEDATA data);

        while (data.Angle > Mathf.Epsilon)
        {
            float delta = RotSpeed * Time.deltaTime;
            if (data.Angle < delta)
            {
                delta = data.Angle;
            }
            myAnim.transform.Rotate(Vector3.up * delta * data.Dir);
            data.Angle -= delta;
            yield return null;
        }
        rotRoutine = null;
    }

    protected void AttackTarget(BattleSystem Target, float AttRange, float AttackDelay, UnityAction EndAttack)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(Attacking(Target, AttRange, AttackDelay, EndAttack));
        if (rotRoutine != null) StopCoroutine(rotRoutine);
        rotRoutine = StartCoroutine(LookingAtTarget(Target));
    }

    IEnumerator Attacking(BattleSystem Target, float AttRange, float AttackDelay, UnityAction EndAttack)
    {
        float playTime = AttackDelay;
        while (true)
        {
            if (Target.IsLive() == false) break;
            Vector3 dir = Target.transform.position - this.transform.position;
            float dist = dir.magnitude;
            if (dist > AttRange)
            {
                myAnim.SetBool("IsWalking", true);
                dir.Normalize();

                float delta = Time.deltaTime * MoveSpeed;
                delta = delta > dist ? dist : delta;
                this.transform.Translate(dir * delta, Space.World);
            }
            else
            {
                myAnim.SetBool("IsWalking", false);
                if (myAnim.GetBool("IsAttacking") == false)
                {
                    playTime += Time.deltaTime;
                    //공격대기
                    if (playTime >= AttackDelay)
                    {
                        //공격
                        myAnim.SetTrigger("Attack");
                        playTime = 0.0f;
                    }
                }
            }
            yield return null;
        }
        EndAttack?.Invoke();
    }

    IEnumerator LookingAtTarget(BattleSystem Target)
    {
        while (true)
        {
            GameUtil.CalAngle(myAnim.transform.forward, (Target.transform.position - this.transform.position).normalized, myAnim.transform.right, out ROTATEDATA data);
            if (data.Angle > Mathf.Epsilon)
            {
                float delta = Time.deltaTime * RotSpeed;
                delta = delta > data.Angle ? data.Angle : delta;
                myAnim.transform.Rotate(Vector3.up * delta * data.Dir, Space.World);
            }
            yield return null;
        }
    }
   

  
}
