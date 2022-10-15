using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
public class EventCam : MonoBehaviour
{
    public Transform cageGate;
    public GameObject player;
    public GameObject canvas;
    Vector3 pos;
    Vector3 dest;
    float timer;
    bool introScene = true;
    float delaytime;
    public Transform bossCamPos;

    Camera eventCam;
    void Awake()
    {
        // UI ���� 
        canvas.SetActive(false);
        eventCam = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CamMoveIntro();
    }

    void CamMoveIntro()
    {
       
        if (introScene)
        {
            // ��Ʈ�� �� ���� Ŭ��, ���� ��Ÿ ����
            player.GetComponent<Player>().enabled = false;

            pos = this.transform.localPosition;
            dest = new Vector3(0.0f, -0.3f, 10.0f);
            this.transform.localPosition = Vector3.Lerp(pos, dest, 0.01f);

            cageGate.localRotation = Quaternion.Slerp(cageGate.localRotation, Quaternion.Euler(0.0f, -180.0f, 0.0f), 0.05f);

            DelayTime(() => eventCam.depth = -1);
            player.GetComponent<Player>().enabled = true;
        }
    }


    //<summary>
    // ��Ʈ�� �� ���� �� �̺�Ʈī�޶� ���� ���߰�, ĳ���� ��ġ ��ȯ, �׺�Ž� ������Ʈ ���ֱ�, UI ���ֱ�
    void DelayTime(UnityAction done)
    {
        delaytime = 3.0f;
        timer += Time.deltaTime;
        if (timer > delaytime)
        {
            done?.Invoke();
            if (introScene)
            {
                player.transform.position = new Vector3(35.0f, -5.5f, -20.0f);
            }
            player.GetComponent<NavMeshAgent>().enabled = true;
            canvas.SetActive(true);
            introScene = false;

        }
    }

    public void BossCamMove()
    {
        // �̺�Ʈ ķ depth ����, ĵ���� ��Ȱ��ȭ, �׺�Ž�������Ʈ ��Ȱ��ȭ
        eventCam.depth = 1;
        canvas.SetActive(false);
        player.GetComponent<NavMeshAgent>().enabled = false;

        this.transform.position = bossCamPos.position;
        this.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        // DelayTime ���� depth ���� �� ��� Ȱ��ȭ 
        DelayTime(() => eventCam.depth = -1);


    }
}
