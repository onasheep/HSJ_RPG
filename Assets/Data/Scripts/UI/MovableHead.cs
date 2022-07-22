using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableHead : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    
    [SerializeField]
    // ������ UIâ
    private Transform _Target;
   
    // UIâ�� ��ġ��
    private Vector2 _beginPoint;

    // Ŭ�� ��ġ��
    private Vector2 _moveBegin;
    private void Awake()
    {
        // Ÿ���� �������� ������ �θ� Ÿ������
        if (_Target == null)
        {
            _Target = this.transform.parent;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _beginPoint = _Target.position;
        _moveBegin = eventData.position;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        _Target.position = _beginPoint + (eventData.position - _moveBegin);

    }


}
