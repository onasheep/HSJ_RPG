using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 
public class AnimEvent : MonoBehaviour
{
    public event UnityAction Attack = null;
    public event UnityAction Skill = null;
    public Player myPlayer;
    public GameObject QEffect;
    public GameObject WEffect;
    public GameObject EEffect;
    public GameObject REffect;
    public GameObject bossEffect;
    public void OnAttack()
    {
        Attack?.Invoke();
    }
    public void OnSkill()
    {
        Skill?.Invoke();
        Instantiate(bossEffect, this.transform);
    }
    public void QSkill()
    {
        myPlayer.OnSkillAttack(this.transform.position, 3f, myPlayer.myStat.ATK * 3f);
        Instantiate(QEffect, this.transform);
    }
    public void WSkill()
    {
        myPlayer.OnSkillAttack(myPlayer.myWeapon.position, 2f, myPlayer.myStat.ATK * 2f);

        Instantiate(WEffect, this.transform);
    }
    public void ESkill()
    {
        myPlayer.OnSkillAttack(this.transform.position, 2f, myPlayer.myStat.ATK * 2f);

        Instantiate(EEffect, this.transform);
    }
    public void RSkill()
    {
        Instantiate(REffect, this.transform);
    }


}
