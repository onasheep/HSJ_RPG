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
        Hpbar.value = (float)myPlayer.mystat.HP / (float)myPlayer.mystat.MaxHP;
        Mpbar.value = (float)myPlayer.mystat.HP / (float)myPlayer.mystat.MaxHP;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTextSlider();
        barController();
    }

    void UpdateTextSlider()
    {
        hpbartxt.text = myPlayer.mystat.HP + " / " + myPlayer.mystat.MaxHP;
        mpbartxt.text = myPlayer.mystat.MP + " / " + myPlayer.mystat.MaxMP;
    }

    void barController()
    {
        Hpbar.value = Mathf.Lerp(Hpbar.value, (float)myPlayer.mystat.HP / (float)myPlayer.mystat.MaxHP, Time.deltaTime * 10.0f);
        Mpbar.value = Mathf.Lerp(Mpbar.value, (float)myPlayer.mystat.MP / (float)myPlayer.mystat.MaxMP, Time.deltaTime * 10.0f);

    }
}
