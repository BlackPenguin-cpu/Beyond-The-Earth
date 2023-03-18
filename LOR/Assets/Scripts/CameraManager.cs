using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum ECameraManagerState
{
    Normal,
    FollowPlayer,
    SpecialEffet,
}
public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;

    public float cameraSpeed;
    public Image flashImage;
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
                transform.position = Vector3.Lerp(transform.position, Player.instance.transform.position + new Vector3(0, 1, -10), Time.deltaTime * cameraSpeed);
                break;
            case ECameraManagerState.SpecialEffet:
                break;
            default:
                break;
        }
    }
    public void CameraShake(float scale, float duration, float delay = 0.1f)
    {
        StartCoroutine(CamearaShakeCoroutine(scale, duration, delay));
    }
    private IEnumerator CamearaShakeCoroutine(float scale, float duration, float delay = 0.1f)
    {
        Vector3 originPos = transform.position;
        while (duration > 0)
        {
            transform.position += (Vector3)((Vector2)Random.insideUnitSphere * scale);

            duration -= delay;
            yield return new WaitForSeconds(delay);
            transform.position = originPos;
        }
    }
    public void Flash(float duration, float startAlpha)
    {
        StartCoroutine(FlashCoroutine(duration, startAlpha));
    }
    private IEnumerator FlashCoroutine(float duration, float startAlpha)
    {
        float vaule = 1 / duration;
        Color alphaColor = new Color(1, 1, 1, startAlpha);

        while (alphaColor.a > 0)
        {
            flashImage.color = alphaColor;
            alphaColor.a -= Time.deltaTime * vaule;
            yield return null;
        }
        flashImage.color = Color.clear;
    }
}
