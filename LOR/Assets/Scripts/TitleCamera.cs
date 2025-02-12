using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private GameObject target;
    private void Update()
    {
        transform.position += new Vector3(Mathf.Sin(Time.time) * 10, -Mathf.Cos(Time.time / 2) * 5, Mathf.Cos(Time.time) * 10) * Time.deltaTime * speed;
        transform.LookAt(target.transform);

    }
}
