using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PhoneController : MonoBehaviour
{
    
    [Header("ATTACK PARAMETERS")]
    [Header("References")]
    public Transform phonePrefab;
    public Transform screen;
    public Transform body;

    [Header("Attack Timing")]
    public float attackDuration   = 1f;
    public float intervalDuration = 2f;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnOffset = new Vector3(0f, 5f, 5f);
    public float bulletLifetime = 3f;

    [HorizontalLine(2f, 0.5f, 0.5f, 0.5f)]

    [Header("ANIMATION")]
    [Header("Anim Portions")]
    public float close_portion     = 0.8f;
    public float open_portion      = 0.1f;
    public float snapback_portion  = 0.1f;
    public float idleshake_portion = 0.8f;

    [Header("Anim Attack")]
    public float closeScreenAngle = 110f;
    public float closeBodyAngle = 30f;

    [Header("Anim Snap Back")]
    public float snapBackScreenAngle = 40f;
    public float snapBackBodyAngle = 10f;

    [Header("Anim Float")]
    public float floatHeight = 0.1f;

    [Header("Anim Shake")]
    public float idleShakeStrength = 0.5f;

    float closeDuration     => attackDuration   * close_portion;
    float openDuration      => attackDuration   * open_portion;
    float snapBackDuration  => attackDuration   * snapback_portion;
    float idleShakeDuration => intervalDuration * idleshake_portion;
    float idlePauseDuration => intervalDuration * (1 - idleshake_portion);



    void Start() => StartCoroutine(AnimateLoop());

    IEnumerator AnimateLoop()
    {
        while (true)
        {
            yield return Attack();
            yield return IdleShake();
        }
    }

    IEnumerator Attack()
    {
        float startY = phonePrefab.localPosition.y;

        float openScreenAngle = closeScreenAngle + snapBackScreenAngle;
        float openBodyAngle = closeBodyAngle + snapBackBodyAngle;
        Sequence s = DOTween.Sequence();

        s.Append(screen.DOLocalRotate(new Vector3(closeScreenAngle, 0f, 0f), closeDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));
        s.Join(body.DOLocalRotate(new Vector3(-closeBodyAngle, 0f, 0f), closeDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));
        s.Join(phonePrefab.DOLocalMoveY(startY + floatHeight, closeDuration).SetEase(Ease.InSine));

        s.Append(screen.DOLocalRotate(new Vector3(-openScreenAngle, 0f, 0f), openDuration, RotateMode.LocalAxisAdd).SetEase(Ease.OutExpo));
        s.Join(body.DOLocalRotate(new Vector3(openBodyAngle, 0f, 0f), openDuration, RotateMode.LocalAxisAdd).SetEase(Ease.OutExpo));
        s.Join(phonePrefab.DOLocalMoveY(startY, openDuration).SetEase(Ease.OutExpo));

        s.Append(screen.DOLocalRotate(new Vector3(snapBackScreenAngle, 0f, 0f), snapBackDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));
        s.Join(body.DOLocalRotate(new Vector3(-snapBackBodyAngle, 0f, 0f), snapBackDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));

        s.InsertCallback(closeDuration + openDuration * 0.5f, SpawnBullet);

        yield return s.WaitForCompletion();
    }

    void SpawnBullet()
    {
        if (bulletPrefab == null) return;

        Vector3 spawnPos = transform.TransformPoint(bulletSpawnOffset);
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Destroy(bullet, bulletLifetime);
    }

    IEnumerator IdleShake()
    {
        Sequence s = DOTween.Sequence();

        s.Join(phonePrefab.DOPunchPosition(new Vector3(0, idleShakeStrength, 0), idleShakeDuration, 5, 0.5f));
        s.Join(phonePrefab.DOPunchRotation(new Vector3(0, 0, 5f), idleShakeDuration, 5, 0.5f));

        yield return s.WaitForCompletion();

        yield return new WaitForSeconds(idlePauseDuration);
    }
}