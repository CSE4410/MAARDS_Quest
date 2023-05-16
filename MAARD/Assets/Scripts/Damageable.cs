using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Damageable : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int, int> healthChanged;

    Animator animator;
    
    [SerializeField]
    private int _maxHealth = 100;

    public int MaxHealth { get 
        {
            return _maxHealth;
                
        } private set 
        {
            _maxHealth = value;
        } 
    }

    [SerializeField]
    private int _health = 100;

    public int Health { get
        {
            return _health;
        }  set 
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);

            // If health drops below 0, character is no longer alive 
            if(_health <= 0)
            {
                IsAlive = false;
                Debug.Log("Dead");
                OnPlayerDeath?.Invoke();
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive {
        get
        {
            return _isAlive;
        } set 
        { 
          _isAlive = value; 
          animator.SetBool(AnimationStrings.isAlive, value);
          Debug.Log("isAlive set " + value);

          
        } 
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                // Remove invincibility
                isInvincible= false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if(_isAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            // Notify other subscribed components that the damageable was hit to handle the knockback
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }

        return false;
    }
    
    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }

        return false;
    }

}
