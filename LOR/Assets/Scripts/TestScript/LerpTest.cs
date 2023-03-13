using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class LerpTest : MonoBehaviour
{

    public Transform target;
    public Transform[] positions;

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(LerpMove(target, positions[0].position, positions[1].position, 3f));
        } 
    }

    IEnumerator LerpMove(Transform transform, Vector3 start, Vector3 end, float duration)
    {
        float timer = 0f;

        while (timer <= duration)
        {
            transform.position = Vector3.Lerp(start, end, 1 - Mathf.Pow(1 - timer / duration, 3));
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = end;


        yield break;
    }
}
