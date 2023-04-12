using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int stageLevel;
    public float score = 0;
    public bool isBossOn;
    public float bossTimer = 120;
    public GameObject BossUI;

    private bool isClear;
    private float nowBossTimer = 0;

    [SerializeField]
    private BaseEnemy[] enemies;
    [SerializeField]
    private BaseBoss[] bosses;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text stageText;
    [SerializeField]
    private Text[] clearInfo;
    [SerializeField]
    private Image clearBoard;
    [SerializeField]
    private GameObject anotherPlanet;
    [SerializeField]
    private Text GameOverScore;
    [SerializeField]
    private Text GameOverTitleText;
    [SerializeField]
    private Image GameOverBoard;
    [SerializeField]
    private Text NameText;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartCoroutine(MonsterWave());
    }
    private void Update()
    {
        CheatKey();
        nowBossTimer += Time.deltaTime;
        if (nowBossTimer > bossTimer && !isBossOn)
        {
            BossEvade();
            SoundManager.instance.PlaySound("BossEvade");
            SoundManager.instance.PlaySound("BossBgm", SoundType.BGM);
            isBossOn = true;
            nowBossTimer = -9999999;
        }
        if (isBossOn)
            BossUI.transform.position = Vector3.Lerp(BossUI.transform.position, new Vector3(960, 1050, 0), Time.deltaTime * 10);
        else
            BossUI.transform.position = Vector3.Lerp(BossUI.transform.position, new Vector3(960, 1400, 0), Time.deltaTime * 10);
        scoreText.text = (numStringFormat((int)score));
        stageText.text = $"Stage {stageLevel}";
    }
    private IEnumerator MonsterWave()
    {
        SoundManager.instance.PlaySound("GameBgm", SoundType.BGM, 0.5f, 1f);
        while (!isBossOn)
        {
            BaseEnemy enemy = enemies[Random.Range(0, 2 + stageLevel)];
            Instantiate(enemy, new Vector3(Random.Range(-15, 15), Random.Range(-5, 5), 50), enemy.transform.rotation);
            enemy.speed = Random.Range(2, 5);
            if (stageLevel == 2)
            {
                enemy.maxHp *= 2.5f;
                enemy.Hp *= 2.5f;
            }

            if (enemy.name == "Meteor") continue;
            yield return new WaitForSeconds(7.5f - stageLevel);
        }
    }
    private void BossEvade()
    {
        BaseBoss boss = bosses[stageLevel - 1];
        StartCoroutine(BossView(boss.gameObject));
        Instantiate(boss, new Vector3(0, 40, 10), boss.transform.rotation);
    }
    private IEnumerator BossView(GameObject target)
    {
        CameraManager.instance.cameraState = CameraState.Target;
        CameraManager.instance.target = target;
        yield return new WaitForSeconds(3);
        CameraManager.instance.cameraState = CameraState.OnPlayer;

    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].ForceDie();
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Player.instance.weaponLevel = 4;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Player.instance.nowSkillCooldown[0] = 0;
            Player.instance.nowSkillCooldown[1] = 0;
            Player.instance.nowSkillCooldown[2] = 0;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Player.instance.Hp = 100;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Player.instance.fuel = 100;
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            NextStage();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            nowBossTimer = bossTimer;
        }
    }

    public void GameOver(bool isComplete = false)
    {
        if (isComplete == true)
            GameOverTitleText.text = "Game Clear!!!";

        isClear = isComplete;
        GameOverBoard.gameObject.SetActive(true);
        GameOverScore.text = $"Total Score : {numStringFormat((int)score)}";
    }
    public void GotoTitle()
    {
        RankSave(NameText.text);
        GameOverBoard.gameObject.SetActive(false);
        Time.timeScale = 1;
        if (isClear)
            SceneManager.LoadScene(2);
        else
            SceneManager.LoadScene(0);
    }
    public void RankSave(string name)
    {
        ScoreInfo info = new ScoreInfo();
        info.score = score;
        info.name = name;
        ScoreInfoList scoreList;

        string saveData = PlayerPrefs.GetString("Rank", "null");
        if (saveData != "null")
            scoreList = JsonUtility.FromJson<ScoreInfoList>(saveData);
        else
            scoreList = new ScoreInfoList();

        scoreList.infos.Add(info);
        scoreList.infos.Sort((x, y) => sortFunc(x, y));
        scoreList.infos.Reverse();
        for (int i = 0; i < scoreList.infos.Count; i++)
            Debug.Log(scoreList.infos[i].name);
        if (scoreList.infos.Count > 5)
            scoreList.infos.RemoveAt(5);
        Debug.Log(scoreList.infos[0].name);
        PlayerPrefs.SetString("Rank", JsonUtility.ToJson(scoreList));
    }
    private int sortFunc(ScoreInfo a, ScoreInfo b)
    {
        if (a.score > b.score)
            return 1;
        else
            return -1;
    }
    public void StageClear()
    {
        StartCoroutine(ClearBoardOn());
        int hpScore = (int)((Player.instance.Hp / 100) * 10000);
        int fuelScore = (int)(Mathf.Clamp(Player.instance.fuel, 0, 100) / 100 * 10000);

        clearInfo[0].text = $"Stage {stageLevel} Clear!";
        clearInfo[1].text = $"남은 체력 : +{numStringFormat(hpScore)}";
        clearInfo[2].text = $"남은 연료 : +{numStringFormat(fuelScore)}";
        clearInfo[3].text = $"스테이지 클리어! : +10,000";

        score += (hpScore + fuelScore) + 10000;


    }

    private IEnumerator ClearBoardOn()
    {
        float duration = 0;
        while (duration < 1)
        {
            clearBoard.rectTransform.anchoredPosition = Vector3.Lerp(clearBoard.rectTransform.anchoredPosition, new Vector3(0, -530, 0), Time.deltaTime * 10);
            duration += Time.deltaTime;
            yield return null;
        }
    }
    public void NextStage()
    {
        if (stageLevel == 2)
        {
            clearBoard.rectTransform.anchoredPosition = new Vector3(0, 530, 0);
            GameOver(true);
            return;
        }
        stageLevel++;
        clearBoard.rectTransform.anchoredPosition = new Vector3(0, 530, 0);
        nowBossTimer = 0;
        Player.instance.Hp = 100;
        Player.instance.Fuel = 100;
        anotherPlanet.SetActive(stageLevel == 2 ? true : false);
        StartCoroutine(MonsterWave());
    }
    public string numStringFormat(int str)
    {
        string returnStr = str.ToString();
        if (returnStr.Length > 3)
        {
            int index = 0;
            for (int i = returnStr.Length - 1; i > 0; i--)
            {
                index++;
                if (index >= 3)
                {
                    returnStr = returnStr.Insert(i, ",");
                    index = 0;
                }
            }
        }

        return returnStr;
    }
}
