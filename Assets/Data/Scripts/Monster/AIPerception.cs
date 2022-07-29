using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AIPerception : MonoBehaviour
{
    public BattleSystem myTarget = null;
    public UnityAction FindTarget = null;
    public LayerMask myEnemyMask;
    public List<GameObject> myEnemyList = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            myEnemyList.Add(other.gameObject);
            if (myTarget == null)
            {

                myTarget = other.gameObject.GetComponentInParent<BattleSystem>();
                FindTarget?.Invoke();
                Debug.Log("1");
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        myEnemyList.Remove(other.gameObject);

    }
}