using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance; // 单例，方便其他脚本调用

    [Header("--- 拖入明暗界父物体 ---")]
    public GameObject lightWorldParent; // 拖入 World_Light
    public GameObject darkWorldParent;  // 拖入 World_Dark

    [Header("--- 状态 ---")]
    public bool isDarkWorld = false; // 当前是不是在暗界

    private PlayerMaskController playerMask;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // 自动找主角的面具脚本
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMask = player.GetComponent<PlayerMaskController>();
        }

        // 游戏开始，默认显示明界，隐藏暗界
        UpdateWorldState();
    }

    // ... 保持原有引用和变量不动 ...

    void Update()
    {
        // ★★★ 监听 E 键 ★★★
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 1. 获取场景中所有的弹窗脚本
            AnimationController[] allPanels = Object.FindObjectsByType<AnimationController>(FindObjectsSortMode.None);
            bool anyPanelOpen = false;

            // 2. 检查是否有任何一个窗口处于打开状态
            foreach (var p in allPanels)
            {
                if (p.isShown)
                {
                    p.HideCurrentPanel(); // 执行关闭
                    anyPanelOpen = true;
                }
            }

            // 3. 如果刚才有关闭过窗口，直接返回，不播音效，不切世界
            if (anyPanelOpen)
            {
                return;
            }

            // 4. 只有所有窗口都关着，按 E 才会播放音效并切换世界
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null) audio.Play();

            TrySwitchWorld();
        }
    }

    // ... TrySwitchWorld() 和 UpdateWorldState() 完全不动 ...

    void TrySwitchWorld()
    {
        // 1. 检查主角是否存活
        if (playerMask == null) return;

        // 2. ★★★ 核心检查：只有戴着“阴阳面具 (ID 0)”才能切换世界！★★★
        // 如果你想让所有面具都能切，就把 if 里的条件删掉
        if (playerMask.currentMask == PlayerMaskController.MaskType.YinYang)
        {
            isDarkWorld = !isDarkWorld; // 切换状态
            UpdateWorldState();         // 刷新物体显示
            Debug.Log(isDarkWorld ? ">>> 进入暗界" : ">>> 回到明界");
        }
        else
        {
            Debug.Log("只有【阴阳面具】才能切换世界！请按 0 切换回阴阳面具。");
        }
    }

    // 执行显示/隐藏的脏活累活
    public void UpdateWorldState()
    {
        if (lightWorldParent != null) lightWorldParent.SetActive(!isDarkWorld);
        if (darkWorldParent != null) darkWorldParent.SetActive(isDarkWorld);
    }
}