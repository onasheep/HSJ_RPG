using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public Item item;
    GameObject obj = null;
    public GameObject NormalItem;
    public GameObject RareItem;
    public GameObject EpicItem;
    public Vector3 DefaultForce = new Vector3(0.0f, 0.0f, 0.0f);
    public float DefaultForceScatter = 0.5f;
 

    // item 3개 드랍
    public void ItemDrop(Transform myMon)
    {
        int RanDropNum = Random.Range(1, 4);
        

        for (int i = 0; i < RanDropNum; i++)
        {

            float RanScater = Random.Range(-DefaultForceScatter, DefaultForceScatter);
            int itemPercentage = Random.Range(0, 101);
            if (itemPercentage < 50)
            {
                obj = Instantiate(NormalItem, myMon.position, Quaternion.identity);
                RandData(item.Normallist, obj);
                RandData(item.Potionlist, obj);
            }
            else if (itemPercentage >= 50 & itemPercentage < 80)
            {
                obj = Instantiate(RareItem, myMon.position, Quaternion.identity);
                RandData(item.Rarelist, obj);

            }
            else if (itemPercentage >= 80 && itemPercentage < 101)
            {
                obj = Instantiate(EpicItem, myMon.position, Quaternion.identity);
                RandData(item.Epiclist, obj);

            }
            StartCoroutine(Moving(obj.transform.position, obj.transform.position + new Vector3(RanScater, 0, RanScater), obj));

        }
    }


    IEnumerator Moving(Vector3 start, Vector3 dest, GameObject obj)
    {
        float dist = Vector3.Distance(start, dest);
        float s = 1.0f / (dist/ 2.0f);
        float t = 0.0f;
        while (t < 1.0f)
        {          
            Vector3 pos = Vector3.Lerp(start, dest, t); // t 는 보간시간 / 0에 가까우면 출발지점 , 1에 가까우면 도착지점
            pos.y += Mathf.Sin(t * Mathf.PI) * dist / 3.0f; // y값을 구해서 포탄의 높낮이를 설정 *dist 를 통해서 거리에 따라 높이를 다르게 설정할수있음
            obj.transform.position = pos;
            t += Time.deltaTime * s;
            yield return null;
        }

    }


    void RandData(List<ItemData> datalist, GameObject obj)
    {
        int randNum = Random.Range(0, datalist.Count);
        obj.GetComponent<DropItemData>().dropItemData = datalist[randNum];

    }
}
