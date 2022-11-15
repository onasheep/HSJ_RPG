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
    
    // ���콺 �� �������� ȭ�� Y�� �������� ȸ�� 
    void CamRotate()
    {
        if (Input.GetMouseButton(2))
        {
            float X = Input.GetAxis("Mouse X");
            this.transform.Rotate(Vector3.up * X * HorizontalRotSpeed, Space.World);

            Vector3 rot = this.transform.rotation.eulerAngles;
        }
    }

    // ���콺 �� ������� �ܾƿ� , �ø� ��� ����
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
