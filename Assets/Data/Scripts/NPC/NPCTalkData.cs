using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalkData : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        // id = 1 BeKindSim // 2 = 보스 전 후 BeKindSim
        talkData.Add(1, new string[] { "깜짝이야! 휴 사람이구나..",
            "이 앞을 지나가려는데 무서운 몬스터가 있지 뭐야",
            "너 강해보이는데 내가 지나갈수 있도록 몬스터를 처치해 줄수 있을까? 사례는 할께!",
            "몬스터는 왼쪽에 있는 문 너머에 있어!" });
        talkData.Add(2, new string[] { "고마워 덕분에 여기까지 올 수 있었어",
        "여기 별거 아니지만 모험에 도움이 될 거야!"});
    }
    // Update is called once per frame
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
