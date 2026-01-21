using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private LayerMask trapLayer;

    private RespawnManager respawnManager;

    void Start()
    {
        respawnManager = FindFirstObjectByType<RespawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 물체의 레이어가 trapLayer에 포함되어 있는가?
        if ((trapLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (respawnManager != null)
            {
                respawnManager.RespawnPlayers();
            }
        }
    }
}

