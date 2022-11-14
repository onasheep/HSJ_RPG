using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    // ��ȭ ������
    public NPCTalkData npcTalkData;
    // UIâ
    public GameObject talkLog;
    public TMPro.TMP_Text talkText;
    public TMPro.TMP_Text NPCNameText;
    public bool isAction = false;
    public int talkIndex;
    public Image talkMark;
    // ����Ʈ
    public QuestManager questManager;
    // ����Ʈ ����
    public ItemDropper myDropper;
    public Transform rewardPos;

    public void Action(int id, bool isNPC, string NPCName)
    {
        Talk(id, isNPC, NPCName);
    }
    void Talk(int id, bool isNPC, string NPCName)
    {
        string talkData = npcTalkData.GetTalk(id, talkIndex);
        if(talkData == null)
        {
            isAction = false;
            talkLog.SetActive(isAction);
            questManager.OnQuestLog();
            if (id == 2)
            myDropper.ItemDrop(rewardPos);
            talkIndex = 0;
            return;
        }
        NPCNameText.text = NPCName;
        talkText.text = talkData;
        isAction = true;
        talkLog.SetActive(isAction);
        talkIndex++;
    }

 
}
