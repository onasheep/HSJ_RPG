using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour
{
    private float HorizontalRotSpeed = 5.0f;
    private Camera myCam;
    private float ZoomSpeed = 20.0f;
    //float closeUpSpeed = 5.0f;
    //float closeUpRotSpeed = 40.0f;
    Transform temp;
    void Start()
    {
        
        myCam = this.GetComponentInChildren<Camera>();
        
       
    }

    void Update()
    {
        CamRotate();
        CamCloseUp();
    }
    
    // 마우스 휠 눌렀을때 화면 Y축 기준으로 회전 
    void CamRotate()
    {
        if (Input.GetMouseButton(2))
        {
            float X = Input.GetAxis("Mouse X");
            this.transform.Rotate(Vector3.up * X * HorizontalRotSpeed, Space.World);

            Vector3 rot = this.transform.rotation.eulerAngles;
        }
    }

    // 마우스 휠 내릴경우 줌아웃 , 올릴 경우 줌인
    void CamCloseUp()
    {
        float W = Input.GetAxis("Mouse ScrollWheel");        
        myCam.fieldOfView -= W * ZoomSpeed;       
        if(myCam.fieldOfView < 25)
        {
            temp = this.transform;
            myCam.fieldOfView = 25;
            // 로아 처럼 일정 field of View 값이 되고 나면 더 이상 카메라가 forward 방향으로 이동 하는것이 아니라
            // Y 포지션값이 내려오고 X축으로 회전하여 캐릭터의 정면을 바라보도록 되게 만들기..
            // y 2 x.rot 25 정면을 바라보게 되는 값
        }
        if (myCam.fieldOfView > 60)
        {
            myCam.fieldOfView = 60;

        }
        

    }

    void ControlBGAlpha()
    {
        Ray ray = new Ray();
        Physics.Raycast(ray,out RaycastHit hit,9999.9f, 1 << LayerMask.NameToLayer("Player"));
        {
            hit.transform.GetComponent<GameObject>().SetActive(false);
        }

    }
}
