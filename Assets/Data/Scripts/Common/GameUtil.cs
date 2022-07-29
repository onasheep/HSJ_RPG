using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CharacterStat
{
    public float AttackRange;
    public float AttackDelay;
    public float AP;
    public float HP;
}

public struct ROTATEDATA
{
    public float Angle;
    public float Dir;
}
public class GameUtil
{

    public static void CalAngle(Vector3 src, Vector3 des, Vector3 right, out ROTATEDATA data)
    {
        data = new ROTATEDATA();
        float r = Mathf.Acos(Vector3.Dot(src, des));
        data.Angle = 180.0f * (r / Mathf.PI);
        data.Dir = 1.0f;

        if (Vector3.Dot(right, des) < 0.0f)
        {
            data.Dir = -1.0f;
        }
    }

}

public interface BattleSystem
{
    void OnDamage(int Damage);
    Transform transform { get; }
    bool IsLive();

}
