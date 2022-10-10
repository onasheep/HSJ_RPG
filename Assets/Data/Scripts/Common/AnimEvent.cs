using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 
public class AnimEvent : MonoBehaviour
{
    public event UnityAction Attack = null;
    public void OnAttack()
    {
        Attack?.Invoke();
    }
}
