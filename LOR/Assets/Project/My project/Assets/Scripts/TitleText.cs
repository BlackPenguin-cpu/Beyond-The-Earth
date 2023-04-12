using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleText : MonoBehaviour
{
    public bool UIOn;
    public bool isLocked;
    private Outline outline;
    private RectTransform rect;
    void Start()
    {
        rect = GetComponent<RectTransform>();
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            UIOn = true;
        }
        outline.effectColor = Color.Lerp(Color.red, Color.blue, Mathf.Abs(Mathf.Cos(Time.time))) - new Color(0, 0, 0, 0.4f);
        if (UIOn && !isLocked)
        {
            rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, new Vector3(645, -141), Time.deltaTime * 6);
        }
        else
        {
            rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, new Vector3(-645, -141), Time.deltaTime * 6);
        }
    }

}
