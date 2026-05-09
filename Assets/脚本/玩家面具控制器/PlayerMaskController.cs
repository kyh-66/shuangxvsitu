using UnityEngine;

public class PlayerMaskController : MonoBehaviour
{
    public enum MaskType
    {
        YinYang = 0,
        Ice = 1,
        Wind = 2,
        Earth = 3,   // 土面具
        Wood = 4,    // 木面具
        Thunder = 5
    }

    [Header("--- 设置 ---")]
    public MaskType currentMask = MaskType.YinYang;
    public Animator playerAnimator;
    public bool[] isMaskUnlocked = new bool[6];

    [Header("--- 木面具爬墙设置 ---")]
    public float climbSpeed = 5f;
    public string climbableTag = "Climbable";
    private bool isTouchingClimbable;

    private PlayerMovement movement;
    private Rigidbody2D rb;

    private AudioSource[] allAudioSources;
    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource nmSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        allAudioSources = GetComponents<AudioSource>();

        // 这里的逻辑对应你 Inspector 里的三个 AudioSource
        if (allAudioSources.Length >= 3)
        {
            musicSource = allAudioSources[0];
            sfxSource = allAudioSources[1];
            nmSource = allAudioSources[2];
        }

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.InitUnlockState(isMaskUnlocked);
            GameUIManager.Instance.UpdateMaskUI((int)currentMask);
        }
        if (playerAnimator != null) playerAnimator.SetInteger("MaskID", (int)currentMask);

        ChangeMask(currentMask);
    }

    void Update()
    {
        // 监听按键
        if (Input.GetKeyDown(KeyCode.Alpha0)) TrySwitchMask(MaskType.YinYang);
        if (Input.GetKeyDown(KeyCode.Alpha1)) TrySwitchMask(MaskType.Ice);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TrySwitchMask(MaskType.Wind);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TrySwitchMask(MaskType.Earth);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TrySwitchMask(MaskType.Wood);
        if (Input.GetKeyDown(KeyCode.Alpha5)) TrySwitchMask(MaskType.Thunder);
        // 动画同步
        if (playerAnimator != null && movement != null)
        {
            playerAnimator.SetBool("IsJumping", !movement.IsGrounded);
            playerAnimator.SetFloat("Speed", Mathf.Abs(movement.Rb.velocity.x));
        }

        // 木面具垂直移动逻辑
        if (currentMask == MaskType.Wood && isTouchingClimbable)
        {
            float v = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, v * climbSpeed);
            rb.gravityScale = 0; // 爬墙时不受重力影响
        }
        else
        {
            // 非木面具或没在墙上时，恢复正常重力
            if (rb != null && rb.gravityScale == 0) rb.gravityScale = 3;
        }
    }

    void TrySwitchMask(MaskType targetMask)
    {
        int index = (int)targetMask;
        if (index >= 0 && index < isMaskUnlocked.Length && isMaskUnlocked[index])
        {
            // 切换音效播放
            if (nmSource != null) nmSource.Play();
            ChangeMask(targetMask);
        }
    }

    void ChangeMask(MaskType newMask)
    {
        if (currentMask == newMask && Time.time > 0.1f) return;

        // 穿墙检测相关物体
        GameObject[] earthTerrains = GameObject.FindGameObjectsWithTag("EarthGround");
        Collider2D playerCollider = GetComponent<Collider2D>();

        currentMask = newMask;

        if (playerAnimator != null) playerAnimator.SetInteger("MaskID", (int)currentMask);
        if (GameUIManager.Instance != null) GameUIManager.Instance.UpdateMaskUI((int)currentMask);

        if (movement != null)
        {
            int id = (int)newMask;

            // 清理土面具旧碰撞
            foreach (GameObject terrain in earthTerrains)
            {
                Collider2D tc = terrain.GetComponent<Collider2D>();
                if (tc != null) Physics2D.IgnoreCollision(playerCollider, tc, false);
            }

            if (id == 2) // 风面具
            {
                movement.SetMaxJumps(3);
            }
            else if (id == 3) // 土面具能力
            {
                movement.SetMaxJumps(1);
                foreach (GameObject terrain in earthTerrains)
                {
                    Collider2D tc = terrain.GetComponent<Collider2D>();
                    if (tc != null) Physics2D.IgnoreCollision(playerCollider, tc, true);
                }
            }
            else // 默认（包括木面具）恢复跳跃次数
            {
                movement.SetMaxJumps(1);
            }
        }
    }

    // --- 碰撞检测（木面具用） ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(climbableTag)) isTouchingClimbable = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(climbableTag)) isTouchingClimbable = false;
    }

    public void UnlockMask(int maskID)
    {
        if (maskID >= 0 && maskID < isMaskUnlocked.Length)
        {
            isMaskUnlocked[maskID] = true;
            if (GameUIManager.Instance != null) GameUIManager.Instance.InitUnlockState(isMaskUnlocked);
        }
    }

    // ★ 关键：保留原有的读取方法
    public MaskType GetCurrentMask()
    {
        return currentMask;
    }
}