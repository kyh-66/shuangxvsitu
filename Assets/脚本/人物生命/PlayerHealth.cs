using UnityEngine;
using UnityEngine.SceneManagement; // 必须保留，用于场景跳转

public class PlayerHealth : MonoBehaviour
{
    [Header("生命设置")]
    public int maxLives = 5;       // 最大生命值
    private int currentLives;      // 当前生命值

    [Header("重生设置")]
    public Transform respawnPoint; // 重生点位置

    [Header("死亡标签 (碰到这些会死)")]
    public string monsterTag = "Monster";
    public string voidTag = "DeadZone";

    [Header("战败场景设置")]
    [Tooltip("在这里填入你那个战败场景的具体名字")]
    public string deathSceneName = "战败";
    
    
    private AudioSource[] allAudioSources;
    private AudioSource musicSource;
    private AudioSource sfxSource;
    void Start()
    {
        allAudioSources = GetComponents<AudioSource>();

        if (allAudioSources.Length >= 2)
        {
            musicSource = allAudioSources[0]; // 对应 Inspector 里的第一个
            sfxSource = allAudioSources[1];   // 对应 Inspector 里的第二个
        }
        // 游戏开始，满血
        currentLives = maxLives;

        // 初始化 UI
        if (LifeUIManager.Instance != null)
        {
            LifeUIManager.Instance.UpdateLifeUI(currentLives);
        }
    }

    // --- 碰撞检测逻辑 (完全未动) ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(monsterTag) || other.CompareTag(voidTag))
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(monsterTag))
        {
            Die();
        }
    }

    // --- 核心死亡逻辑修改 ---
    public void Die()
    {
        // 1. 扣血
        currentLives--;
        if (sfxSource != null)
        {
            sfxSource.Play();
            Debug.Log("播放了切换面具的音效");
        }

        // 2. 刷新 UI
        if (LifeUIManager.Instance != null)
        {
            LifeUIManager.Instance.UpdateLifeUI(currentLives);
        }

        // 3. 判断是否还有命
        if (currentLives > 0)
        {
            Respawn(); // 还有命，重生
        }
        else
        {
            Debug.Log("符咒已耗尽，跳转至战败场景！");

            // ★ 修改点：直接跳转到你的战败场景
            if (!string.IsNullOrEmpty(deathSceneName))
            {
                SceneManager.LoadScene(deathSceneName);
            }
            else
            {
                Debug.LogError("你忘记在 Inspector 里填战败场景的名字了！");
                gameObject.SetActive(false);
            }
        }
    }

    // --- 重生逻辑 (完全未动) ---
    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;
        }
        else
        {
            Debug.LogError("请在 Inspector 里拖入 RespawnPoint！");
        }
    }
}