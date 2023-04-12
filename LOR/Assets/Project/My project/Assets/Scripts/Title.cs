using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[System.Serializable]
public class ScoreInfo
{
    public string name;
    public float score;
}
[System.Serializable]
public class ScoreInfoList
{
    public List<ScoreInfo> infos = new List<ScoreInfo>();
}
public class Title : MonoBehaviour
{
    [SerializeField]
    private GameObject RankingBoard;
    [SerializeField]
    private Text[] rankingText;

    public ScoreInfoList scoreInfoList;

    [SerializeField]
    TitleSceneEffect titleSceneEffect;
    [SerializeField]
    TitleText TitleText;
    private void Start()
    {
        SaveLoad();
        SoundManager.instance.PlaySound("TitleBgm", SoundType.BGM, 0.5f, 0.2f);
    }
    private void SaveLoad()
    {
        string saveData = PlayerPrefs.GetString("Rank", "null");
        if (saveData != "null")
            scoreInfoList = JsonUtility.FromJson<ScoreInfoList>(saveData);
        else
            scoreInfoList = new ScoreInfoList();
    }

    public void GameStart()
    {
        StartCoroutine(StartEffect());
    }
    private IEnumerator StartEffect()
    {
        TitleText.UIOn = false;
        TitleText.isLocked = true;
        titleSceneEffect.OnStart();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }
    public void RankingClose()
    {
        StartCoroutine(RankingEaseScale(false));
    }
    public void Ranking()
    {
        RankingBoard.SetActive(true);
        StartCoroutine(RankingEaseScale(true));

        for (int i = 0; i < 5; i++)
        {
            if (scoreInfoList.infos.Count > i)
            {
                rankingText[i].text = $"{scoreInfoList.infos[i].name} : {numStringFormat((int)scoreInfoList.infos[i].score)}";
            }
            else
                rankingText[i].text = "--- : 0";
        }
    }

    private IEnumerator RankingEaseScale(bool isActive)
    {
        float duration = 0.001f;
        while (duration <= 1)
        {
            if (isActive)
                RankingBoard.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, duration);
            else
                RankingBoard.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, duration);
            duration += Time.deltaTime * 3.5f;
            yield return null;
        }
        RankingBoard.SetActive(isActive);
    }
    public void GameQuit()
    {
        Application.Quit();
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
