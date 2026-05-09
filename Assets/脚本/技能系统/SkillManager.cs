using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("--- 通用设置 ---")]
    public float skillCooldown = 0.5f;
    private float nextFireTime = 0f;
    private PlayerMaskController playerMask;

    [Header("--- 技能预制体 ---")]
    public GameObject[] skillPrefabs;

    [Header("--- 雷面具传送锚点 (ID 5) ---")]
    // 在 Inspector 里拖入你设置好的传送目标点
    public Transform thunderTeleportAnchor;

    [Header("--- 环境 ---")]
    public Transform darkWorldParent;

    void Start()
    {
        playerMask = GetComponent<PlayerMaskController>();
    }

    void Update()
    {
        if (playerMask == null) return;

        int currentID = (int)playerMask.currentMask;

        // --- 分支 A：雷面具 (ID 5) 监听 J 键直接传送 ---
        if (currentID == 5)
        {
            if (Input.GetKeyDown(KeyCode.J)) TeleportToFixedAnchor();
        }
        // --- 分支 B：其他面具 监听 J 键 ---
        else
        {
            if (Input.GetKey(KeyCode.J)) FireSkill(currentID);
        }
    }

    void TeleportToFixedAnchor()
    {
        if (thunderTeleportAnchor == null)
        {
            Debug.LogError("未设置雷面具传送锚点！");
            return;
        }

        if (Time.time < nextFireTime) return;

        transform.position = thunderTeleportAnchor.position;
        Debug.Log(">>> 雷面具 (ID 5) 传送成功！");
        nextFireTime = Time.time + skillCooldown;
    }

    void FireSkill(int index)
    {
        if (index < 0 || index >= skillPrefabs.Length || skillPrefabs[index] == null) return;
        if (Time.time < nextFireTime) return;

        GameObject prefab = skillPrefabs[index];
        bool isFacingRight = transform.localScale.x > 0;
        Vector3 spawnPos = transform.position + new Vector3(isFacingRight ? 1f : -1f, 12f, 0);

        GameObject skillObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        Vector3 scale = skillObj.transform.localScale;
        scale.x = isFacingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        skillObj.transform.localScale = scale;

        if (darkWorldParent != null && transform.position.y < -50)
            skillObj.transform.SetParent(darkWorldParent);

        nextFireTime = Time.time + skillCooldown;
    }
}