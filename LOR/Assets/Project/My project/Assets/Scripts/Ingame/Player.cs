using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : Entity
{
    public static Player instance;
    public float fuel;
    public float Fuel
    {
        get { return fuel; }
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            fuel = value;
            if (fuel <= 0) Die();
        }
    }


    public int weaponLevel;
    public float invincibleTime;
    public float parryingDuration;
    public float[] skillCooldown;
    public float[] nowSkillCooldown;

    [SerializeField]
    private Text skillCooldownText;
    [SerializeField]
    private Transform skillCooldownTextParent;

    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private ParticleSystem parryingEffect;

    private BezierBullet bezierBullet;

    [SerializeField]
    private Image[] CooldownIcon;
    [SerializeField]
    private Text[] CooldownText;
    [SerializeField]
    private Text[] statusText;
    [SerializeField]
    private Image[] statusBar;
    [SerializeField]
    private Material playerMaterial;
    [SerializeField]
    private ParticleSystem bombParticle;
    [SerializeField]
    private ParticleSystem healParticle;
    [SerializeField]
    private ParticleSystem deadParticle;
    [SerializeField]
    private ParticleSystem hitParticle;
    [SerializeField]
    private Material ShieldMaterial;
    public override float Hp
    {
        get => base.Hp;
        set
        {
            if (hp - value > 0 && parryingDuration > 0)
            {
                parryingDuration = 0;
                DoParrying();
                return;
            }
            if (invincibleTime > 0 && hp - value > 0) return;
            if (value <= 1) Die();
            base.Hp = value;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        bezierBullet = Resources.Load<BezierBullet>("Bullet/PlayerBezierBullet");
        nowSkillCooldown = new float[skillCooldown.Length];
    }
    private void Update()
    {
        InputFunc();
        CooldownShow();
        UiShow();
        shield.SetActive(parryingDuration > 0);
        for (int i = 0; i < nowSkillCooldown.Length; i++)
        {
            nowSkillCooldown[i] -= Time.deltaTime;
        }
        parryingDuration -= Time.deltaTime;
        invincibleTime -= Time.deltaTime;
        fuel -= Time.deltaTime;

        if (invincibleTime > 0)
            playerMaterial.color = new Color(0.1f, 0.55f, 0.75f, 0.1f);
        else
            playerMaterial.color = new Color(0.1f, 0.55f, 0.75f, 1);

        ShieldMaterial.color = Color.Lerp(new Color(1, 1, 1, 0.4f), new Color(1, 0.3f, 0, 0.4f), (float)(weaponLevel - 1) / 3);
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void InputFunc()
    {
        if (Input.GetKeyDown(KeyCode.Space) && SkillCooldownCheck(0))
        {
            OnParrying();
        }
        if (Input.GetKeyDown(KeyCode.Q) && SkillCooldownCheck(1))
        {
            Heal();
        }
        if (Input.GetKeyDown(KeyCode.W) && SkillCooldownCheck(2))
        {
            Bomb();
        }
    }
    private void UiShow()
    {
        statusBar[0].fillAmount = Hp / maxHp;
        statusText[0].text = $"남은 내구도 {(int)Hp} / {(int)maxHp} ";

        statusBar[1].fillAmount = Mathf.Clamp(fuel, 0, 100) / 100;
        statusText[1].text = $"남은 연료 {Mathf.Clamp((int)Fuel, 0, 100)} / 100 ";
    }
    private bool SkillCooldownCheck(int skillNum)
    {
        if (nowSkillCooldown[skillNum] < 0)
        {
            if (skillNum == 0)
                nowSkillCooldown[skillNum] = skillCooldown[skillNum] - weaponLevel * 0.4f;
            else
                nowSkillCooldown[skillNum] = skillCooldown[skillNum];
            return true;
        }
        else
        {
            SoundManager.instance.PlaySound("SkillCanNotUse", SoundType.SFX, 0.5f, 0.5f);
            Text text = Instantiate(skillCooldownText, skillCooldownTextParent);
            StartCoroutine(CooldownTextShow(text));
            return false;
        }
    }
    private void CooldownShow()
    {
        for (int i = 0; i < skillCooldown.Length; i++)
        {
            if (nowSkillCooldown[i] < 1) CooldownText[i].text = "";
            else CooldownText[i].text = ((int)nowSkillCooldown[i]).ToString();

            if (i == 0)
            {
                CooldownIcon[0].fillAmount = nowSkillCooldown[0] / (skillCooldown[0] - weaponLevel * 0.4f);
            }
            else
            {
                CooldownIcon[i].fillAmount = nowSkillCooldown[i] / skillCooldown[i];
            }
        }
    }
    private IEnumerator CooldownTextShow(Text text)
    {
        text.color = Color.white;
        float index = 0;
        while (text.color != Color.clear)
        {
            text.color = Color.Lerp(text.color, Color.clear, index);
            text.transform.position += Vector3.up * 100 * Time.deltaTime;
            index += Time.deltaTime / 50;
            yield return null;
        }
        Destroy(text.gameObject);
    }
    private void Heal()
    {
        SoundManager.instance.PlaySound("Heal", SoundType.SFX, 0.5f, 1.2f);
        healParticle.Play();
        Hp += 10;
    }
    private void Bomb()
    {
        SoundManager.instance.PlaySound("Bomb", SoundType.SFX, 0.5f, 0.8f);
        bombParticle.Play();
        BaseBullet[] bullets = FindObjectsOfType<BaseBullet>();
        for (int i = 0; i < bullets.Length; i++)
        {
            Destroy(bullets[i].gameObject);
        }
        BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Hp -= 100;
        }
    }
    #region 패링관련 이벤트
    private void OnParrying()
    {
        SoundManager.instance.PlaySound("OnParrying", SoundType.SFX, 0.5f, 2);
        parryingDuration = 0.4f + (float)weaponLevel * 0.05f;
        StartCoroutine(ShieldOn());
    }

    private IEnumerator ShieldOn()
    {
        shield.transform.localScale = Vector3.zero;
        while (shield.transform.localScale.x < 2.9f + (float)weaponLevel * 0.5f)
        {
            shield.transform.localScale = Vector3.Lerp(shield.transform.localScale, Vector3.one * (3 + (float)weaponLevel * 0.5f), Time.deltaTime * 10);
            yield return null;
        }
    }

    private void DoParrying()
    {
        SoundManager.instance.PlaySound("Parrying", SoundType.SFX, 0.5f, 0.8f);
        SoundManager.instance.PlaySound("Parrying2", SoundType.SFX, 0.5f, 1.3f);

        StartCoroutine(TimeStop());
        StartCoroutine(BarrelRoll());
        parryingEffect.Play();
        if (invincibleTime < 0.2f)
            invincibleTime = 0.2f;
        nowSkillCooldown[0] = 0.1f;
        Fuel += 2;

        Attack();
    }
    private void Attack()
    {
        for (int i = 0; i < 2 + weaponLevel; i++)
        {
            BezierBullet bezier = Instantiate(bezierBullet, transform.position, Quaternion.identity);

            BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();
            float value = 99999;
            GameObject target = null;
            for (int j = 0; j < enemies.Length; j++)
            {
                BaseEnemy obj = enemies[j];
                if (obj.Hp <= 0) continue;
                if (obj.transform.position.z < transform.position.z) continue;
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (value > distance)
                {
                    value = distance;
                    target = obj.gameObject;
                }
            }
            bezier.target = target;

            bezier.atkDmg = weaponLevel + 15;
            bezier.bezierSpeed = 1 + weaponLevel;

            Vector3[] poses = new Vector3[3];
            poses[0] = transform.position;
            poses[1] = transform.position + Random.insideUnitSphere * 40;
            if (target == null)
                poses[2] = transform.position + Vector3.forward * 50;
            else
                poses[2] = bezier.target.transform.position;

            bezier.poses = poses;
        }
    }
    private IEnumerator TimeStop()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.05f);
        Time.timeScale = 1f;
    }
    private IEnumerator BarrelRoll()
    {
        Vector3 rot = Vector3.zero;
        while (rot.z < 360)
        {
            transform.eulerAngles = rot;
            rot.z += 4;
            yield return null;
        }
        transform.eulerAngles = Vector3.zero;
    }
    #endregion
    protected override void Die()
    {
        UiShow();
        Time.timeScale = 0;
        GameManager.instance.GameOver();
        gameObject.SetActive(false);
    }

    protected override void Hit()
    {
        SoundManager.instance.PlaySound("PlayerHit");
        StartCoroutine(CameraManager.instance.CameraShake(0.4f, 0.3f, 0.05f));
        StartCoroutine(ActionCam.instance.CameraShake(0.1f, 0.3f, 0.05f));
        invincibleTime = 0.4f;
    }

    protected override void Move()
    {
        if (Input.GetKey(KeyCode.W)) return;
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        if (hor > 0 && transform.position.x > 15) hor = 0;
        if (ver > 0 && transform.position.y > 5) ver = 0;

        if (hor < 0 && transform.position.x < -15) hor = 0;
        if (ver < 0 && transform.position.y < -5) ver = 0;

        transform.position += new Vector3(hor, ver) * Time.deltaTime * speed;
    }

}
