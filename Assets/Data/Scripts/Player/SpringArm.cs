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
        if(myCam.fieldOfView < 25)
        {
            temp = this.transform;
            myCam.fieldOfView = 25;
            // �ξ� ó�� ���� field of View ���� �ǰ� ���� �� �̻� ī�޶� forward �������� �̵� �ϴ°��� �ƴ϶�
            // Y �����ǰ��� �������� X������ ȸ���Ͽ� ĳ������ ������ �ٶ󺸵��� �ǰ� �����..
            // y 2 x.rot 25 ������ �ٶ󺸰� �Ǵ� ��
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
