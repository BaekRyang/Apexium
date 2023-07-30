using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PierceShot : AttackableSkill
{
    private const float RANGE         = 50f;
    private const float COOLDOWN      = 6f;
    private const float STUN_DURATION = 1f;
    private const float SKILL_DAMAGE  = 3f;

    public void OnEnable()
    {
        SkillType   = SkillTypes.Secondary;
        Cooldown    = COOLDOWN;
        SkillDamage = SKILL_DAMAGE;
    }

    public override bool Play()
    {
        if (!CanUse()) return false;
        if (!ConsumeResource()) return false;


        Transform _cachedTransform = transform;
        Vector3   _position        = _cachedTransform.position;

        RaycastHit2D[] _hit = Physics2D.RaycastAll(_position, _cachedTransform.right * (int)Facing, RANGE);

        foreach (var _hitObject in _hit)
        {
            StartCoroutine(VFXManager.PlayVFX("BulletPop", _hitObject.point, (int)Player.Controller.PlayerFacing));
            Collider2D _hitCollider = _hitObject.collider;

            if (_hitCollider == null) continue;

            if (_hitCollider.CompareTag("Tile")) break; //벽에 맞았다면 더 뒤에있는 물체들은 무시

            if (_hitCollider.CompareTag("Enemy"))
            {
                int _damage = GetDamage();
                _hitCollider.GetComponent<EnemyBase>().Attacked(_damage, STUN_DURATION, Player);
            }
        }

        if (Stats.Resource == 0)
            return RevolverShot.Reload();

        LastUsedTime = Time.time;
        return true;
    }
}