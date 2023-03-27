using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    public static Player instance;
    //마나 같은 느낌으로 해볼까
    public float fuel;

    public float[] skillCooldown = new float[3];
    public float[] skillNowCooldown = new float[3];

    public int attackLevel = 1;
    public float attackRadius;
    public float attackDamage;
    public float invincibleDuration;

    [SerializeField]
    private GameObject parringShieldObj;

    [SerializeField]
    private GameObject skillCooldownTextParents;
    [SerializeField]
    private ParticleSystem parryingParticles;
    private Text skillCooldownText;

    private float parryingDuration;

    public override float Hp
    {
        get => base.Hp;
        set
        {
            if (value - hp <= 0 && invincibleDuration > 0)
            {
                return;
            }
            base.Hp = value;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        skillCooldownText = Resources.Load<Text>("SkillCooldownText");
    }
    private void Update()
    {
        InputFunc();
        for (int i = 0; i < skillNowCooldown.Length; i++)
        {
            skillNowCooldown[i] -= Time.deltaTime;
        }
        parryingDuration -= Time.deltaTime;
        invincibleDuration -= Time.deltaTime;
        parringShieldObj.SetActive(parryingDuration > 0);
        parringShieldObj.transform.Rotate(0, Time.deltaTime * 50, 0);
    }

    private void InputFunc()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Skill3();
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
    private void ParryingAttack()
    {
        Debug.Log("parrying");

        int targetLayer = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, attackRadius, targetLayer);
        switch (attackLevel)
        {
            case 1:
                foreach (Collider target in hitObjects)
                {
                    Entity enemy = target.GetComponent<Entity>();
                    enemy.Hp -= attackDamage;
                }
                break;
            default:
                break;
        }
    }
    //heal
    private void Skill1()
    {
        if (skillNowCooldown[0] > 0)
        {
            SkillCooldownNotYetText("수리");
            return;
        }
        skillNowCooldown[0] = skillCooldown[0];

    }
    //bomb
    private void Skill2()
    {
        if (skillNowCooldown[1] > 0)
        {
            SkillCooldownNotYetText("폭탄");
            return;
        }
        skillNowCooldown[1] = skillCooldown[1];

    }
    //parrying
    private void Skill3()
    {
        if (skillNowCooldown[2] > 0)
        {
            SkillCooldownNotYetText("패링");
            return;
        }

        StartCoroutine(OnParryingEffect());
        parryingDuration = 1;
        skillNowCooldown[2] = skillCooldown[2];
    }
    IEnumerator OnParryingEffect()
    {
        parringShieldObj.transform.localScale = Vector3.one;
        while (parringShieldObj.transform.localScale.z < attackLevel + 4)
        {
            parringShieldObj.transform.localScale += new Vector3(0.4f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void SkillCooldownNotYetText(string SkillName)
    {
        string text = $"{SkillName}이 사용 준비 중 입니다...";

        GameObject obj = Instantiate(skillCooldownText.gameObject, skillCooldownTextParents.transform);
        Vector3 vec = obj.transform.position + new Vector3(0, 100);

        Text txt = obj.GetComponent<Text>();
        txt.text = text;
        StartCoroutine(TextEvade(txt, vec));

    }
    private IEnumerator TextEvade(Text text, Vector3 vec)
    {
        while (text.color.a > 0.05f)
        {
            text.color = Color.Lerp(text.color, Color.clear, Time.deltaTime);
            text.transform.position = Vector3.Lerp(text.transform.position, vec, Time.deltaTime);

            yield return null;
        }
        Destroy(text.gameObject);
    }

    protected override void Die()
    {
        //CameraManager.instance.CameraShake(5, 5f);

    }
    protected override void Hit()
    {
        if (parryingDuration > 0)
        {
            parryingDuration = 0;
            DoParrying();
            return;
        }

        CameraManager.instance.CameraShake(0.1f, 0.5f, 0.01f);
        invincibleDuration = 0.4f;
    }
    private void DoParrying()
    {
        CameraManager.instance.Flash(0.2f, 0.8f);
        CameraManager.instance.OnParryingCameraEffect();
        skillNowCooldown[2] = 0;
        ParryingAttack();
        StartCoroutine(TimeStop(0.2f, 0.2f));
        StartCoroutine(BarrelRoll(0.2f));
        parryingParticles.Play();
    }
    #region EffectFunc
    private IEnumerator BarrelRoll(float duration)
    {
        Vector3 rot = Vector3.zero;
        float rotPlus = 0;
        while (rotPlus < 360)
        {
            rotPlus += Time.deltaTime * 360 * (1 / duration);
            rot.z = rotPlus;
            transform.eulerAngles = rot;
            yield return null;
        }
        transform.eulerAngles = Vector3.zero;
    }
    private IEnumerator TimeStop(float timeScale, float duration)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }
    #endregion
    protected override void Move()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(hor, ver) * speed * Time.deltaTime;
    }
}
