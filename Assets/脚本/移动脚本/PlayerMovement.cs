using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("--- 核心参数 ---")]
    public float moveSpeed = 6f;
    public float jumpForce = 13f;

    [Header("--- 跳跃系统 ---")]
    public int defaultJumpCount = 1; // 在 Inspector 改成 2 即可实现二段跳
    private int currentMaxJumps;
    private int jumpsLeft;

    [Header("--- 直线地面检测 (Raycast) ---")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float rayDistance = 0.4f; // 射线的长度，你可以在 Inspector 随意调节
    public Vector3 rayOffset;        // 起点偏移量

    // --- 供 SkillManager 等脚本调用的公开属性 ---
    public bool IsGrounded { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    private Vector3 originalScale;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();

        // 初始化跳跃参数
        currentMaxJumps = defaultJumpCount;
        jumpsLeft = currentMaxJumps;

        // 记录初始缩放，确保翻转逻辑正常
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 1. 直线检测逻辑
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position + rayOffset, Vector2.down, rayDistance, groundLayer);
        IsGrounded = hit.collider != null;

        // 2. 落地重置跳跃次数
        if (IsGrounded && Rb.velocity.y <= 0.1f)
        {
            jumpsLeft = currentMaxJumps;
        }

        // 3. 跳跃输入判断
        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<AudioSource>().Play(); // 播跳跃音效
            // 核心逻辑：只有次数够，且满足“在地上”或“已经是多段跳”的情况下才能跳
            if (jumpsLeft > 0)
            {
                // 如果是第一跳（jumpsLeft等于最大值），但射线没探测到地面，则不准跳
                if (jumpsLeft == currentMaxJumps && !IsGrounded)
                {
                    return;
                }

                Jump();
            }
        }

        // 4. 左右移动
        float x = Input.GetAxis("Horizontal");
        Rb.velocity = new Vector2(x * moveSpeed, Rb.velocity.y);

        // 5. 翻转逻辑
        if (x != 0)
        {
            float newX = Mathf.Sign(x) * Mathf.Abs(originalScale.x);
            transform.localScale = new Vector3(newX, originalScale.y, originalScale.z);
        }
    }

    void Jump()
    {
        Rb.velocity = new Vector2(Rb.velocity.x, 0); // 重置垂直速度，保证手感
        Rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsLeft--;
    }

    // ★★★ 你的 SetMaxJumps 保留在这里，没删 ★★★
    public void SetMaxJumps(int count)
    {
        currentMaxJumps = count;
        // 如果改完后在地上，立刻补满次数
        if (IsGrounded) jumpsLeft = currentMaxJumps;
    }

    // --- 用 Gizmos 画出那根探测线 ---
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            // 探测到绿，探测不到红
            Gizmos.color = IsGrounded ? Color.green : Color.red;

            Vector3 startPos = groundCheck.position + rayOffset;
            Vector3 endPos = startPos + Vector3.down * rayDistance;

            Gizmos.DrawLine(startPos, endPos);
            // 画个底部的短横线，方便看清射线到哪结束
            Gizmos.DrawLine(endPos + Vector3.left * 0.1f, endPos + Vector3.right * 0.1f);
        }
    }

    // 在 PlayerMovement 类中添加
    [Header("木面具爬墙设置")]
    public float climbSpeed;
    public string climbableTag = "Climbable";
    private bool isTouchingClimbable;

    void FixedUpdate()
    {
        // 获取面具控制器组件
        PlayerMaskController maskControl = GetComponent<PlayerMaskController>();

        // 只有切换到木面具(ID 4)且碰到特定的墙时才能爬
        if (maskControl != null && maskControl.currentMask == PlayerMaskController.MaskType.Wood && isTouchingClimbable)
        {
            float v = Input.GetAxisRaw("Vertical"); // 获取上下键输入
            Rb.velocity = new Vector2(Rb.velocity.x, 5 * climbSpeed);
            Rb.gravityScale = 0; // 爬墙时关掉重力，防止滑落
        }
        else
        {
            Rb.gravityScale = 3; // 恢复正常重力倍率（根据你原本的设置调整）
        }
    }

    // 碰撞检测墙体
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(climbableTag)) isTouchingClimbable = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(climbableTag)) isTouchingClimbable = false;
    }
}