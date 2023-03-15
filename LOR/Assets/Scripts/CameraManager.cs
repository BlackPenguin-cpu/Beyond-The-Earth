using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum ECameraManagerState
{
    Normal,
    FollowPlayer,
    SpecialEffet,
}
public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;

    public ECameraManagerState state;

    private void Start()
    {
        instance = this;
    }
    private void Update()
    {
        switch (state)
        {
            case ECameraManagerState.Normal:
                break;
            case ECameraManagerState.FollowPlayer:
                transform.position  = Player
                break;
            case ECameraManagerState.SpecialEffet:
                break;
            default:
                break;
        }
    }
    public void CameraShake(float scale, float duration, float delay = 0.1f)
    {
        StartCoroutine(CamearaShaking(scale, duration, delay));
    }
    private IEnumerator CamearaShaking(float scale, float duration, float delay = 0.1f)
    {
        Vector3 originPos = transform.position;
        while (duration > 0)
        {
            transform.position += Random.insideUnitSphere * scale;

            duration -= delay;
            yield return new WaitForSeconds(delay);
        }
        transform.position = originPos;
    }

}
