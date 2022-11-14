using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : MonoBehaviour
{
    public QuestManager questManager;
    public Monster mon;
    public string monsterName;
    public int questCnt = 0;
    public void Update()
    {
        if(mon.myState == Monster.STATE.DEATH)
        {
            questCnt += 1;
        }
    }
}

