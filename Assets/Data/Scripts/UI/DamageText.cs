using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public float alphaSpeed;
    public float destroyTime;
    TMPro.TMP_Text text;
    Color alpha;
    public float damage;
    void Start()
    {
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;
        text = this.GetComponent<TMPro.TMP_Text>();
        text.text = damage.ToString();
        alpha = text.color;
        Invoke("DestroyText", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void DestroyText()
    {
        Destroy(this.gameObject);
    }
}
