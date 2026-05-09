using UnityEngine;

public class DualWorldObstacle : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("通过此障碍需要的面具")]
    public PlayerMaskController.MaskType requiredMask;

    [Tooltip("通过方式：勾选=销毁障碍，不勾选=变成可穿透")]
    public bool destroyOnPass = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerMaskController>();
            if (player && player.GetCurrentMask() == requiredMask)
            {
                // 面具正确！
                if (destroyOnPass)
                {
                    Destroy(gameObject);
                }
                else
                {
                    GetComponent<Collider2D>().isTrigger = true; // 变成穿透模式
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 离开后恢复碰撞（如果不销毁的话）
        if (other.CompareTag("Player") && !destroyOnPass)
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
