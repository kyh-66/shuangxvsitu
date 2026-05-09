using UnityEngine;

public class MagicMarkerFX : MonoBehaviour
{
    [Header("视觉特效")]
    public float rotateSpeed = 50f; // 旋转速度
    public float floatSpeed = 2f;   // 上下浮动频率
    public float floatHeight = 0.2f; // 上下浮动幅度

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 1. 让它慢慢旋转
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        // 2. 让它上下浮动 (利用 Sin 函数)
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}