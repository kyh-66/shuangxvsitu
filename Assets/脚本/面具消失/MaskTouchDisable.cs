using UnityEngine;

public class MaskTouchDisable : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("填入要解锁的面具ID (0=阴阳, 1=冰, 2=火...)")]
    public int maskID = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 只有主角能触发
        if (other.CompareTag("Player"))
        {
            // 2. 尝试解锁主角的面具
            PlayerMaskController player = other.GetComponent<PlayerMaskController>();
            if (player != null)
            {
                player.UnlockMask(maskID);
                Debug.Log($"解锁面具 {maskID} 成功！");
            }

            // 3. 【核心要求】直接取消右上角的勾 (隐藏物体)
            gameObject.SetActive(false);
        }
    }
}