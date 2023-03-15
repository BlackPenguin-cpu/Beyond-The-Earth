using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    //마나 같은 느낌으로 해볼까
    public float fuel;

    public float[] skillCooldown = new float[3];
    public float[] skillNowCooldown = new float[3];

    [SerializeField]
    private GameObject skillCooldownTextObj;
    private void Start()
    {

    }
    private void Update()
    {
        InputFunc();
        for (int i = 0; i < skillCooldown.Length; i++)
        {
            skillCooldown[i] -= Time.deltaTime;
        }
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
    }
    private void Attack()
    {

    }
    //heal
    private void Skill1()
    {
        if (skillNowCooldown[0] > 0)
        {
            SkillCooldownNotYetText("수리");
            return;
        }
    }
    //bomb
    private void Skill2()
    {
        if (skillNowCooldown[1] > 0)
        {
            SkillCooldownNotYetText("폭탄");
            return;
        }
    }
    //parrying
    private void Skill3()
    {
        if (skillNowCooldown[2] > 0)
        {
            SkillCooldownNotYetText("패링");
            return;
        }
    }
    private void SkillCooldownNotYetText(string SkillName)
    {
        string text = $"{SkillName}이 사용 준비 중 입니다...";

        Text textComponent = Resources.Load<Text>("SkillText");
        Instantiate(textComponent, skillCooldownTextObj.transform);
        Vector3 vec = textComponent.transform.position + new Vector3(0, 50);

        textComponent.text = text;
        StartCoroutine(TextEvade(textComponent, vec));

    }
    private IEnumerator TextEvade(Text text, Vector3 vec)
    {
        while (text.color.a > 0.05f)
        {
            text.color = Color.Lerp(text.color, Color.clear, Time.deltaTime);
            text.transform.position = Vector3.Lerp(text.transform.position, vec, Time.deltaTime);

            yield return null;
        }
        Destroy(text);
    }

    protected override void Die()
    {

    }

    protected override void Hit()
    {
        CameraManager.instance.CameraShake(2, 0.5f);
    }

    protected override void Move()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        transform.position = new Vector3(hor, ver) * speed;
    }
}
