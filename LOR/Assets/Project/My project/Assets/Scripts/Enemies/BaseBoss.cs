using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBoss : BaseEnemy
{
    public string bossName;
    public Image bossHpBar;
    public Text bossHpText;

    protected override void Start()
    {
        base.Start();
        GameManager.instance.isBossOn = true;

        bossHpBar = GameManager.instance.BossUI.transform.GetChild(0).GetComponent<Image>();
        bossHpText = GameManager.instance.BossUI.transform.GetChild(1).GetComponent<Text>();
    }
    protected virtual void Update()
    {
        UIFunc();
    }
    protected virtual void UIFunc()
    {
        bossHpText.text = $"BOSS : {bossName}   {hp} / {maxHp}";
        bossHpBar.fillAmount = hp / maxHp;
    }

    protected override void Die()
    {
        SoundManager.instance.PlaySound("BossDie");

        Instantiate(deadParticle, transform.position, deadParticle.transform.rotation);
        Instantiate(deadParticle, transform.position, deadParticle.transform.rotation);
        Instantiate(deadParticle, transform.position, deadParticle.transform.rotation);
        Instantiate(deadParticle, transform.position, deadParticle.transform.rotation);
        GameManager.instance.isBossOn = false;
        Player.instance.fuel = 1000000;
        Destroy(gameObject);
    }

}
