using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float score;
    public float canBossEvadeCount;
    public bool isBossOn;

    private List<BaseEnemy> baseEnemies;
    private List<BaseEnemy> bosses;
    private int stageNum;

    void Start()
    {
        baseEnemies = Resources.LoadAll<BaseEnemy>("Monster/").ToList();
        bosses = Resources.LoadAll<BaseEnemy>("Boss/").ToList();

        StartCoroutine(MobSpawnWave(stageNum));
    }
    private IEnumerator MobSpawnWave(int stageNum)
    {
        while (true)
        {

            yield return null;
        }
    }
}


