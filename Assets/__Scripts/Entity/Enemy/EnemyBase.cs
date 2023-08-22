using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private const float VERTICAL_OFFSEET = .7f;

    private EnemyAI          _enemyAI;
    private MMF_Player       _damageFeedback;
    private MMF_FloatingText _floatingText;

    public EnemyStats stats;

    private void Start()
    {
        stats = new(GameManager.Instance.GetEnemyData("Frost"));

        _enemyAI = GetComponent<EnemyAI>();
        _enemyAI.Initialize(this);
        _damageFeedback = transform.Find("DamageFeedback").GetComponent<MMF_Player>();
        _damageFeedback.Initialization();
        
        _floatingText   = _damageFeedback.GetFeedbackOfType<MMF_FloatingText>();
    }

    private readonly Dictionary<uint, uint> _attackID = new();

    public void Attacked(int p_pDamage, bool p_isCritical, float p_stunDuration, Player p_pAttacker, uint? p_attackID = null)
    {
        _floatingText.Value = (p_isCritical ? "<color=#FF5500>" : "<color=#FFDDCC>") + p_pDamage + (p_isCritical ? "!" : "") + "</color>";

        //크리티컬은 빨강 아니면 하양
        _floatingText.Intensity = p_isCritical ? 1.05f : 1;

        if (!p_attackID.HasValue) //ID가 없으면 기본 위치에
            _floatingText.TargetPosition = transform.position;
        else //ID가 있으면
        {
            if (_attackID.ContainsKey(p_attackID.Value)) //해당 ID의 공격이 이미 있으면
                _attackID[p_attackID.Value]++;           //해당 아이디의 value를 증가
            else                                         //없으면
                _attackID.Add(p_attackID.Value, 0);      //해당 ID의 공격을 만들고 value를 초기화


            _floatingText.TargetPosition = transform.position + Vector3.up * (VERTICAL_OFFSEET * _attackID[p_attackID.Value]);
        }

        try
        {
            _damageFeedback.PlayFeedbacks();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        

        if (p_stunDuration == 0)
            _enemyAI.Daze();
        else
            _enemyAI.Stun(p_stunDuration);

        GetDamage(p_pDamage);

        if (stats.canKnockback)
            Knockback(p_pAttacker, 200);


        if (stats.health <= 0)
        {
            _enemyAI.animator.SetTrigger("Die");
            Destroy(this);
            Destroy(_enemyAI);
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<Collider2D>().enabled    = false;
        }
    }

    private void GetDamage(int p_pDamage)
    {
        //TODO: BossHealthBarDisplay에 어떤식으로 연결시켜서 값을 동기화 시킬까
        stats.health -= p_pDamage;
    }

    private void Knockback(Player p_pAttacker, float p_pKnockbackForce)
    {
        Vector2 _knockbackDirection = (transform.position - p_pAttacker.PlayerPosition).normalized;

        // GetComponent<Rigidbody2D>().velocity = _knockbackDirection * p_pKnockbackForce;
        GetComponent<Rigidbody2D>().AddForce(_knockbackDirection * p_pKnockbackForce, ForceMode2D.Impulse);
    }
}