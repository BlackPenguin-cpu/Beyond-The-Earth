using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class endingCredit : MonoBehaviour
{
    public Image image;
    private void Start()
    {
        StartCoroutine(ending());
        SoundManager.instance.PlaySound("Ending", SoundType.SFX);
        SoundManager.instance.PlaySound("GameBgm", SoundType.BGM);
    }
    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * 80;
    }
    IEnumerator ending()
    {
        float duration = 0;
        while (duration < 30)
        {
            duration += Time.deltaTime;
            yield return null;
        }
        duration = 0;
        Color color = Color.clear;
        while (duration < 10)
        {
            image.color = color;
            color.a += (Time.deltaTime / 10);
            duration += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
}
