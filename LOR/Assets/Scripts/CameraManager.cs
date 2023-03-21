using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
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

    private Player player;
    private Camera mainCamera;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        player = Player.instance;
        instance = this;
    }
    private void Update()
    {
        switch (state)
        {
            case ECameraManagerState.Normal:
                break;
            case ECameraManagerState.FollowPlayer:
                transform.position = Vector3.Lerp(transform.position, Player.instance.transform.position + new Vector3(0, 5, -10), Time.deltaTime * cameraSpeed);
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(30, 0, 0), Time.deltaTime * cameraSpeed));
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
        WaitForSeconds waitTime = new WaitForSeconds(delay);
        while (duration > 0)
        {
            transform.position += (Vector3)((Vector2)Random.insideUnitSphere * scale);

            Debug.Log(duration);
            duration -= delay;
            yield return waitTime;
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
    public void OnParryingCameraEffect()
    {
        StartCoroutine(OnParryingViewSize());
    }
    private IEnumerator OnParryingViewSize()
    {
        while (mainCamera.fieldOfView < 70)
        {
            mainCamera.fieldOfView += 5;
            yield return new WaitForSeconds(0.0001f);
        }
        while (mainCamera.fieldOfView > 60)
        {
            mainCamera.fieldOfView -= 1;
            yield return new WaitForSeconds(0.005f);
        }


        yield return null;
    }
}
