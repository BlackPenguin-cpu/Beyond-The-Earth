using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCam : MonoBehaviour
{
    public static ActionCam instance;
    private Player player;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        player = Player.instance;
    }
    private void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 1, 2.5f);
    }
    public IEnumerator CameraShake(float scale, float duration, float rpm)
    {
        while (duration > 0)
        {
            transform.position += (Vector3)Random.insideUnitCircle * scale;
            duration -= rpm;
            yield return new WaitForSeconds(rpm);
        }
    }
}
