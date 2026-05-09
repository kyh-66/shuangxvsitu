using UnityEngine;
using UnityEngine.SceneManagement; // 必加：场景管理

public class LevelPortal : MonoBehaviour
{
    [Header("--- 传送设置 ---")]
    [Tooltip("在 Inspector 面板里填入下一关的具体名字 (比如 Level2, Level3...)")]
    public string nextSceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        // 只有主角能触发
        if (other.CompareTag("Player"))
        {
            // 检查是否填了名字
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                // 检查场景是否在 Build Settings 里 (可选的安全检查)
                if (Application.CanStreamedLevelBeLoaded(nextSceneName))
                {
                    Debug.Log($"传送！前往: {nextSceneName}");
                    SceneManager.LoadScene(nextSceneName);
                }
                else
                {
                    Debug.LogError($"❌ 报错：找不到场景 '{nextSceneName}'！\n请检查：1.名字拼对了吗？ 2.把该场景拖进 Build Settings 了吗？");
                }
            }
            else
            {
                Debug.LogError("❌ 报错：你忘记在传送门的 Inspector 里填下一关的名字了！");
            }
        }
    }
}