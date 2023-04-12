using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    OnPlayer,
    Special,
    Title,
    Target
}
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CameraState cameraState;
    public float moveSpeed;
    public float rotateSpeed;

    public GameObject target;
    private Player player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        player = Player.instance;
    }
    private void FixedUpdate()
    {
        CameraMove();

    }
    private void CameraMove()
    {
        switch (cameraState)
        {
            case CameraState.OnPlayer:
                Vector3 pos = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0, 5, -10), moveSpeed * Time.deltaTime);
                Vector3 realpos = new Vector3(Mathf.Clamp(pos.x, -5, 5), Mathf.Clamp(pos.y, 2.5f, 5f), pos.z);
                transform.position = realpos;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(20, 0, 0), rotateSpeed * Time.deltaTime);
                break;
            case CameraState.Special:
                break;
            case CameraState.Title:
                break;
            case CameraState.Target:
                transform.LookAt(target.transform);
                break;
            default:
                break;
        }
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
