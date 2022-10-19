using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 
public class AnimEvent : MonoBehaviour
{
    public event UnityAction Attack = null;
    public Player myPlayer;
    public GameObject QEffect;
    public GameObject WEffect;
    public GameObject EEffect;
    public GameObject REffect;
    public void OnAttack()
    {
        Attack?.Invoke();
    }

    public bool QSkill()
    {
        myPlayer.OnSkillAttack(this.transform.position, 3.0f, myPlayer.myStat.ATK * 2.0f);
        Instantiate(QEffect, this.transform);
        return true;
    }
    public void WSkill()
    {
        myPlayer.OnSkillAttack(myPlayer.myWeapon.position, 2.0f, myPlayer.myStat.ATK * 1.5f);

        Instantiate(WEffect, this.transform);
    }
    public void ESkill()
    {
        myPlayer.OnSkillAttack(this.transform.position, 2.0f, myPlayer.myStat.ATK * 1.5f);

        Instantiate(EEffect, this.transform);
    }
    public void RSkill()
    {
        Instantiate(REffect, this.transform);
    }


}
