using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GateOpen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            this.GetComponent<NavMeshObstacle>().enabled = false;
            StartCoroutine(SmoothOpen());
        }
    }

    IEnumerator SmoothOpen()
    {
        while (this.transform.localRotation.y < 130.0f)
        {
            Quaternion temp = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(0.0f, 130.0f, 0.0f), 0.01f);
            yield return null;
        }
       
    }
}
