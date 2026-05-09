using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    [Header("声音设置")]
    public AudioSource audioSource;
    public AudioClip footstepSound;

    [Header("节奏设置")]
    [Range(0.1f, 1.0f)]
    public float stepInterval = 0.4f; // 跑步间隔

    [Header("音调随机")]
    public bool useRandomPitch = true;
    [Range(0.8f, 1.2f)]
    public float minPitch = 0.9f;
    [Range(0.8f, 1.2f)]
    public float maxPitch = 1.1f;

    private PlayerMovement movement;
    private float timer;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        // 初始让计时器归零，保证按下的瞬间就会响第一声
        timer = 0;
    }

    void Update()
    {
        if (movement == null) return;

        // 1. 输入检测：是不是按住了 A 或 D (为了保险我也加了左右箭头)
        bool isPressingKey = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

        // 2. 地面检测：必须踩在地上 (跳起来不能响)
        bool isGrounded = movement.IsGrounded;

        // 3. 最终判定
        if (isPressingKey && isGrounded)
        {
            timer -= Time.deltaTime; // 开始倒计时

            if (timer <= 0)
            {
                PlayStepSound();
                timer = stepInterval; // 重置计时，准备下一声
            }
        }
        else
        {
            // 如果松开了手，或者跳到了空中
            // 把计时器重置为 0
            // 这样下次你一落地或者一按键，声音会立刻触发，没有延迟
            timer = 0;
        }
    }

    void PlayStepSound()
    {
        if (footstepSound == null || audioSource == null) return;

        if (useRandomPitch)
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        else
            audioSource.pitch = 1f;

        audioSource.PlayOneShot(footstepSound);
    }
}