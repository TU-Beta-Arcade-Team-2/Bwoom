using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombo : MonoBehaviour
{
    private PlayerInput m_playerInput;

    [SerializeField] private Animator m_playerAnimator;
    [SerializeField] private PlayerAttackHitbox m_playerAttackHitbox;
    [SerializeField] private HitstopManager m_hitstopManager;
    private float m_attackTimer;

    [System.Serializable]
    private struct Combo
    {
        public string AnimationName;
        public float Damage;
        public float Cooldown;
        public Vector2 EnemyLaunchVector;
        public Vector2 PlayerLaunchVector;
        public float HitStopDuration;
    }

    [SerializeField] private List<Combo> m_combos;
    private int m_comboCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();

        BetterDebugging.Assert(m_hitstopManager != null, "REMEMBER TO ASSIGN THE HITSTOP MANAGER!");
        BetterDebugging.Assert(m_playerAttackHitbox != null, "REMEMBER TO ASSIGN THE ATTACK HITBOX MANAGER!");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_comboCounter != m_combos.Count)
        {
            // If the attack timer has exceeded the cooldown, we can attack again! 
            if (m_attackTimer > m_combos[m_comboCounter].Cooldown)
            {
                Attack();
            }
        }
        else
        {
            if (m_attackTimer > m_combos[m_comboCounter - 1].Cooldown)
            {
                m_comboCounter = 0;
            }
        }

        m_attackTimer += Time.deltaTime;
    }

    private void Attack()
    {
        if (m_playerInput.actions["Attack"].triggered)
        {
            m_attackTimer = 0f;

            m_playerAttackHitbox.GetDamage(m_combos[m_comboCounter].Damage);

            // TODO: We should make this go through the player controller... PlayerController.setAniamtion(string)...
            m_playerAnimator.Play(m_combos[m_comboCounter].AnimationName);
            m_playerAttackHitbox.SetLaunchForce(m_combos[m_comboCounter].EnemyLaunchVector, m_combos[m_comboCounter].PlayerLaunchVector);
            m_hitstopManager.SetHitstopDuration(m_combos[m_comboCounter].HitStopDuration);

            m_comboCounter++;
        }
    }
}
