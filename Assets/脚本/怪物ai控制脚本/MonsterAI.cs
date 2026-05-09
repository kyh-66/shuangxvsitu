using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 3f;
    public float chaseRange = 20f; // 只有主角进入这个范围才开始追

    private Transform playerTarget;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 自动寻找场景里的主角 (前提是主角Tag是 Player)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
    }

    void Update()
    {
        if (playerTarget == null) return;

        // 1. 计算距离
        float distance = Vector2.Distance(transform.position, playerTarget.position);

        // 2. 只有在范围内才追
        if (distance < chaseRange)
        {
            // 确定方向：主角在左边就是 -1，在右边就是 1
            float direction = Mathf.Sign(playerTarget.position.x - transform.position.x);

            // 移动 (保留原本的 Y 轴速度，只改变 X 轴)
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

            // 转向 (让怪物脸朝向移动方向)
            if (direction > 0) transform.localScale = new Vector3(1, 1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // --- 掉入虚空即死 ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // 记得确保你的虚空物体 Tag 是 "DeadZone"
        if (other.CompareTag("DeadZone"))
        {
            Destroy(gameObject); // 销毁自己
        }
    }
}