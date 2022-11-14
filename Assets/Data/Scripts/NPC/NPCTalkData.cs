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
        // id = 1 BeKindSim // 2 = ���� �� �� BeKindSim
        talkData.Add(1, new string[] { "��¦�̾�! �� ����̱���..",
            "�� ���� ���������µ� ������ ���Ͱ� ���� ����",
            "�� ���غ��̴µ� ���� �������� �ֵ��� ���͸� óġ�� �ټ� ������? ��ʴ� �Ҳ�!",
            "���ʹ� ���ʿ� �ִ� �� �ʸӿ� �־�!" });
        talkData.Add(2, new string[] { "���� ���п� ������� �� �� �־���",
        "���� ���� �ƴ����� ���迡 ������ �� �ž�!"});
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
