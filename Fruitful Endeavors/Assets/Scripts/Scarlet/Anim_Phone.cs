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
    public float attackPortion = 0.33f;
    private float attackDuration;
    private float intervalDuration;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnOffset = new Vector3(0f, 5f, 5f);
    public float   bulletLifetime  = 3f;
    public Vector3 bulletSpawnRot  = Vector3.zero;

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
    public float idleFloatSpeed    = 1f;

    float closeDuration     => attackDuration   * close_portion;
    float openDuration      => attackDuration   * open_portion;
    float snapBackDuration  => attackDuration   * snapback_portion;
    float idleShakeDuration => intervalDuration * idleshake_portion;
    float idlePauseDuration => intervalDuration * (1 - idleshake_portion);



    private Turret turret;

    void Start()
    {
        turret = GetComponent<Turret>() ?? GetComponentInParent<Turret>();
        float total = turret != null && turret.fireRate > 0f ? 1f / turret.fireRate : 3f;
        attackDuration   = total * Mathf.Clamp01(attackPortion);
        intervalDuration = total * (1f - Mathf.Clamp01(attackPortion));
        StartCoroutine(AnimateLoop());
    }

    IEnumerator AnimateLoop()
    {
        while (true)
        {
            if (turret != null && turret.target != null)
            {
                yield return Attack();
                yield return IdleShake();
            }
            else
            {
                yield return IdleContinuous();
            }
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
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(bulletSpawnRot));
        Destroy(bullet, bulletLifetime);
    }

    IEnumerator IdleShake()
    {
        yield return IdleFloat(idleShakeDuration * 0.5f);
    }

    IEnumerator IdleContinuous()
    {
        float startY = phonePrefab.localPosition.y;
        while (turret == null || turret.target == null)
        {
            float y = startY + Mathf.Sin(Time.time * idleFloatSpeed * Mathf.PI) * idleShakeStrength;
            phonePrefab.localPosition = new Vector3(phonePrefab.localPosition.x, y, phonePrefab.localPosition.z);
            yield return null;
        }
        phonePrefab.localPosition = new Vector3(phonePrefab.localPosition.x, startY, phonePrefab.localPosition.z);
    }

    IEnumerator IdleFloat(float halfDur)
    {
        float startY = phonePrefab.localPosition.y;
        yield return phonePrefab.DOLocalMoveY(startY + idleShakeStrength, halfDur).SetEase(Ease.InOutSine).WaitForCompletion();
        yield return phonePrefab.DOLocalMoveY(startY, halfDur).SetEase(Ease.InOutSine).WaitForCompletion();
    }
}