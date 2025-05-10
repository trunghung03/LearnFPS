using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public class Score : MonoBehaviour
    {
        [Tooltip("Health ratio at which the critical health vignette starts appearing")]
        public float CriticalHealthRatio = 0.3f;

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnHealed;
        public UnityAction OnDie;

        public float CurrentScore { get; set; }
        public bool Invincible { get; set; }

        bool m_IsDead;

        void Start()
        {
            CurrentScore = 0.0f;
        }

        public void Heal(float healAmount)
        {
            float healthBefore = CurrentScore;
            CurrentScore += healAmount;

            // call OnHeal action
            float trueHealAmount = CurrentScore - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnHealed?.Invoke(trueHealAmount);
            }
        }

        public void TakeDamage(float damage, GameObject damageSource)
        {
            if (Invincible)
                return;

            float healthBefore = CurrentScore;
            CurrentScore += damage;

            // call OnDamage action
            float trueDamageAmount = healthBefore - CurrentScore;
            if (trueDamageAmount > 0f)
            {
                OnDamaged?.Invoke(trueDamageAmount, damageSource);
            }

            HandleDeath();
        }

        public void Kill()
        {
            CurrentScore = 0f;


            HandleDeath();
        }

        void HandleDeath()
        {
            if (m_IsDead)
                return;

            // call OnDie action
            if (CurrentScore <= 0f)
            {
                m_IsDead = true;
                OnDie?.Invoke();
            }
        }
    }
}