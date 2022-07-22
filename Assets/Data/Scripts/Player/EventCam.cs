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
    bool check = true;

    void Awake()
    {
        // UI 끄기 
        canvas.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        CamMoveIntro();
        
    }

    void CamMoveIntro()
    {
       
        if (check)
        {
            // 인트로 씬 도중 클릭, 공격 기타 방지
            player.GetComponent<Player>().enabled = false;

            pos = this.transform.localPosition;
            dest = new Vector3(0.0f, -0.3f, 10.0f);
            this.transform.localPosition = Vector3.Lerp(pos, dest, 0.01f);

            cageGate.localRotation = Quaternion.Slerp(cageGate.localRotation, Quaternion.Euler(0.0f, -180.0f, 0.0f), 0.05f);

            DelayTime(() => this.GetComponent<Camera>().depth = -1);
            player.GetComponent<Player>().enabled = true;
        }
    }


    //<summary>
    // 인트로 씬 종료 후 이벤트카메라 뎁스 낮추고, 캐릭터 위치 변환, 네브매쉬 에이전트 켜주기, UI 켜주기
    void DelayTime(UnityAction done)
    {
        float delaytime = 3.0f;
        timer += Time.deltaTime;
        if (timer > delaytime)
        {
            done?.Invoke();
            player.transform.position = new Vector3(35.0f, -5.5f, -20.0f);
            player.GetComponent<NavMeshAgent>().enabled = true;
            canvas.SetActive(true);
            check = false;

        }
    }
}
