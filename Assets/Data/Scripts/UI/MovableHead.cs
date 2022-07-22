using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableHead : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    
    [SerializeField]
    // 움직일 UI창
    private Transform _Target;
   
    // UI창의 위치값
    private Vector2 _beginPoint;

    // 클릭 위치값
    private Vector2 _moveBegin;
    private void Awake()
    {
        // 타겟이 존재하지 않으면 부모를 타겟으로
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
