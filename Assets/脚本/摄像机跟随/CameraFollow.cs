using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("--- 跟随设置 ---")]
    [Tooltip("拖入你的 Player 物体")]
    public Transform target;

    [Tooltip("跟随的平滑时间 (0=死板, 0.5=很慢, 推荐 0.1~0.2)")]
    public float smoothTime = 0.2f;

    [Tooltip("摄像机相对于主角的偏移量 (注意 Z 轴通常要设为 -10)")]
    public Vector3 offset = new Vector3(0f, 1f, -10f);

    // 内部变量，用于计算速度
    private Vector3 velocity = Vector3.zero;

    // 使用 LateUpdate 确保在主角移动完之后摄像机才动，防止抖动
    void LateUpdate()
    {
        if (target == null) return;

        // 1. 计算目标位置 (主角位置 + 偏移量)
        Vector3 targetPosition = target.position + offset;

        // 2. 锁定 Z 轴 (可选)
        // 2D 游戏中，为了防止摄像机拍不到东西，通常强制 Z 轴固定在 -10
        targetPosition.z = -10f;

        // 3. 平滑移动
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}