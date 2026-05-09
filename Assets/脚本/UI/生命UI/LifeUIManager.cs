using UnityEngine;

public class LifeUIManager : MonoBehaviour
{
    public static LifeUIManager Instance; // 单例，方便主角脚本调用

    [Header("把5个符咒图片拖进来")]
    public GameObject[] lifeCharms; // 存放符咒图片的数组

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // --- 更新生命 UI 的核心方法 ---
    public void UpdateLifeUI(int currentLives)
    {
        // 遍历所有符咒
        for (int i = 0; i < lifeCharms.Length; i++)
        {
            // 如果 当前索引 < 剩余生命值，显示符咒
            // 否则，隐藏符咒
            if (i < currentLives)
            {
                lifeCharms[i].SetActive(true);
            }
            else
            {
                lifeCharms[i].SetActive(false);
            }
        }
    }
}