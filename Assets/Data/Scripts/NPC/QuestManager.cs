using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject questChecker;
    GameObject obj;
    int count = 0;
    float delta = 0;
    public float lerpTime = 20.0f;
    int questCnt1 = 0;
    int questCnt2 = 0;
    int questCnt3 = 0;
    // 퀘스트 목표치 확인용 변수
    int endQuestCnt = 0;
    public TMPro.TMP_Text questTxt;
    public TMPro.TMP_Text questClearTxt;

    public GameObject metalFence;
    public GameObject fenceEffect;
    public Transform effectPos;
    void Start()
    {
        
    }

    void Update()
    {
        SetQuest();
        endQuestCnt = questCnt1 + questCnt2 + questCnt3;

        if (endQuestCnt >= 3)
        {
            OpenFence();
            questClearTxt.text = "퀘스트 완료";
        }
    }

    public void OnQuestLog()
    {
        questChecker.SetActive(true);
    }
    public void OffQuestLog()
    {
        questChecker.SetActive(false);
    }
    void SetQuest()
    {

        string txt = string.Format("미니언 제거 {0} / 1 \n미라 제거 {1} / 1 \n거대 거미 제거 {2} / 1 ", questCnt1, questCnt2, questCnt3);
        questTxt.text = txt;
        if (GameObject.Find("Boximon Cyclopes") == null && questCnt1 < 1)
            questCnt1 += 1;
        if (GameObject.Find("BossMummy_Mon") == null && questCnt2 < 1)
            questCnt2 += 1;
        if (GameObject.Find("Polygonal Metalon Green") == null && questCnt3 < 1)
            questCnt3 += 1;

    }
    void OpenFence()
    {
        delta += Time.deltaTime;
        Vector3 pos = metalFence.transform.localPosition;
        Vector3 dest = new Vector3(-7.736f, -3.50f, -1.648f);
        metalFence.transform.localPosition = Vector3.Lerp(pos, dest, delta / lerpTime);
        if(delta >= lerpTime)
        {
            delta = lerpTime;
        }

        if(count == 0)
        {
            obj = Instantiate(fenceEffect, effectPos);
            count++;
        }
        if(delta > 2.0f)
        {
            Destroy(obj);
        }
        
        
    }
}
