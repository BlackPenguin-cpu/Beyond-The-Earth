using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float fuel;
    public float speed;

    public float skill1Cooldown;
    public float skill1NowCooldown;
    public float skill2Cooldown;
    public float skill2NowCooldown;
    public float skill3Cooldown;
    public float skill3NowCooldown;
    private void Start()
    {

    }
    private void Update()
    {
        InputFunc();
        skill1Cooldown -= Time.deltaTime;
        skill2Cooldown -= Time.deltaTime;
    }

    private void InputFunc()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Skill1();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Skill2();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
    }
    private void Attack()
    {

    }
    //heal
    private void Skill1()
    {
        if (skill1NowCooldown > 0)
        {
            SkillCooldownNotYetText("¼ö¸®");
            return;
        }
    }
    //bomb
    private void Skill2()
    {
        if (skill2NowCooldown > 0)
        {
            SkillCooldownNotYetText("ÆøÅº");
            return;
        }
    }
    //parrying
    private void Skill3()
    {
        if (skill3NowCooldown > 0)
        {
            SkillCooldownNotYetText("ÆÐ¸µ");
            return;
        }
    }
    private void SkillCooldownNotYetText(string SkillName)
    {

    }
    private void Move()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        transform.position = new Vector3(hor, ver) * speed;
    }

    protected override void Die()
    {
        throw new System.NotImplementedException();
    }

    protected override void Hit()
    {
        throw new System.NotImplementedException();
    }
}
