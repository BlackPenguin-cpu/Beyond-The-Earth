using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class joyStickManager : MonoBehaviour, IPointerDownHandler
{
    public float speed;

    [SerializeField] private RectTransform joyStick;
    [SerializeField] private GameObject obj;
    private Vector3 originMousePos;
    private bool btnClicked;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originMousePos = joyStick.position;
    }
    public void Update()
    {
        MouseClicked();
    }
    void MouseClicked()
    {
        if (btnClicked == true)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rectTransform.position;
            mousePos = Vector2.ClampMagnitude(mousePos, 0.5f);
            joyStick.position = originMousePos + mousePos;

            obj.transform.position += mousePos.normalized * speed * Time.deltaTime;

            if (Input.GetMouseButtonUp(0)) MouseBtnUpEvent();
        }
    }

    void MouseBtnUpEvent()
    {
        btnClicked = false;
        joyStick.transform.position = originMousePos;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        btnClicked = true;
    }

}
