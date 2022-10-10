using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider Hpbar;
    public TMPro.TMP_Text hpbartxt;
    public Slider Mpbar;
    public TMPro.TMP_Text mpbartxt;
    public Slider Expbar;

    public Player myPlayer;
    void Start()
    {
        Hpbar.value = (float)myPlayer.myStat.HP / (float)myPlayer.myStat.MaxHP;
        Mpbar.value = (float)myPlayer.myStat.HP / (float)myPlayer.myStat.MaxHP;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTextSlider();
        barController();
    }

    void UpdateTextSlider()
    {
        hpbartxt.text = myPlayer.myStat.HP + " / " + myPlayer.myStat.MaxHP;
        mpbartxt.text = myPlayer.myStat.MP + " / " + myPlayer.myStat.MaxMP;
    }

    void barController()
    {
        Hpbar.value = Mathf.Lerp(Hpbar.value, (float)myPlayer.myStat.HP / (float)myPlayer.myStat.MaxHP, Time.deltaTime * 10.0f);
        Mpbar.value = Mathf.Lerp(Mpbar.value, (float)myPlayer.myStat.MP / (float)myPlayer.myStat.MaxMP, Time.deltaTime * 10.0f);

    }
}
