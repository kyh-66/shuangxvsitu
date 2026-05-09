using UnityEngine;

public class MaskPickupItem : MonoBehaviour
{
    [Header("解锁设置")]
    [Tooltip("填入ID：0=阴阳, 1=冰, 2=雷...")]
    [Range(0, 5)] // ★ 修改：允许填 0 到 5
    public int maskIDToUnlock = 1; // 默认设为1 (冰)

    [Header("视觉特效")]
    public GameObject pickupEffect;

    void Update()
    {
        float y = Mathf.Sin(Time.time * 3f) * 0.1f;
        transform.localPosition += new Vector3(0, y, 0) * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMaskController player = other.GetComponent<PlayerMaskController>();
            if (player != null)
            {
                // 直接传 ID，非常直观
                player.UnlockMask(maskIDToUnlock);

                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}