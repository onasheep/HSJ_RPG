using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour
{
    private float HorizontalRotSpeed = 5.0f;
    private Camera myCam;
    private float ZoomSpeed = 10.0f;

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
        if (myCam.fieldOfView < 25)
        {
            temp = this.transform;
            myCam.fieldOfView = 25;

        }
        if (myCam.fieldOfView > 60)
        {
            myCam.fieldOfView = 60;

        }


    }
}
