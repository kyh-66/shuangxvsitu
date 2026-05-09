using UnityEngine;

public class UniversalMaskPickup : MonoBehaviour
{
    [Header("--- 设置 ---")]
    [Tooltip("填入ID：1=冰, 2=风, 5=瞬移...")]
    public int maskIDToUnlock = 2; // 默认填2 (风)

    [Header("--- 特效 (可选) ---")]
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    private bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            PlayerMaskController player = other.GetComponent<PlayerMaskController>();
            if (player != null)
            {
                // 1. 解锁
                player.UnlockMask(maskIDToUnlock);

                // 2. 播放声音
                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                // 3. 播放特效
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // 4. 销毁道具
            Destroy(gameObject);
        }
    }
}