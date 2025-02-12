using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * Mathf.Sin(Time.time) * 0.1f;
    }

    public void OnStart()
    {
        StartCoroutine(BarrelRoll());
        ps.Play();
    }
    private IEnumerator BarrelRoll()
    {
        Vector3 rot = new Vector3(0, transform.rotation.y, 0);
        while (rot.z < 360)
        {
            transform.eulerAngles = rot;
            rot.z += 4;
            yield return null;
        }
        transform.eulerAngles = Vector3.zero;
    }

}
