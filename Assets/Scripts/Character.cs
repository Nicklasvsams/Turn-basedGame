using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator animator;

    public Vector3 startPosition,
        targetPosition;

    public float attackMoveSpeed;

    private bool isAttacking, 
        finishedAttack, 
        isDefending;

    protected SpriteRenderer spriteRenderer;

    public string characterName;
    public int health, maxHealth;
    public int mana, maxMana;
    public int atkPower, defPower, defOrigin;
    public List<Spell> spells;

    private void Update()
    {
        AttackAnimation();
    }

    public bool CastSpell(Spell spell, Character targetCharacter)
    {
        if (spell.cost <= mana)
        {
            var spellPosition = new Vector3(targetCharacter.transform.position.x, targetCharacter.transform.position.y, targetCharacter.transform.position.z - 2f);
            

            Spell spellToCast = Instantiate<Spell>(spell, spellPosition, spell.spellName == "Fire Pillar" ? new Quaternion(0.7071f, 0, 0, 0.7071f) : Quaternion.identity);


            mana -= spellToCast.cost;

            spellToCast.Cast(targetCharacter);

            return true;
        }
        else
        {
            BattleController.Instance.battleLog.Log(string.Format("Not enough mana to cast {0}", spell.spellName));
            return false;
        }
    }

    public void AttackAnimation()
    {
        if (isAttacking)
        {
            spriteRenderer = this.GetComponent<SpriteRenderer>();

            animator.SetBool("FinishedAttack", false);

            if (!this.finishedAttack)
            {
                if (Vector3.Distance(this.transform.position, targetPosition) > 0.1f)
                {
                    animator.SetBool("MovingToTarget", true);
                    transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, attackSpeed * Time.deltaTime);
                }
                else if (Vector3.Distance(this.transform.position, targetPosition) <= 0.1f)
                {
                    animator.SetBool("MovingToTarget", false);
                    animator.SetBool("ReachedTarget", true);

                    if (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && this.animator.GetCurrentAnimatorStateInfo(0).IsName(this.characterName + "_Attack"))
                    {
                        this.finishedAttack = true;
                        spriteRenderer.flipX = !spriteRenderer.flipX;
                    }
                }
            }
            else
            {
                if (Vector3.Distance(this.transform.position, startPosition) > 0)
                {
                    animator.SetBool("ReachedTarget", false);
                    animator.SetBool("MovingToTarget", true);

                    transform.position = Vector3.MoveTowards(this.transform.position, startPosition, attackSpeed * Time.deltaTime);
                }
                else if (Vector3.Distance(this.transform.position, startPosition) <= 0)
                {
                    animator.SetBool("MovingToTarget", false);
                    animator.SetBool("FinishedAttack", true);
                    isAttacking = false;

                    spriteRenderer.flipX = !spriteRenderer.flipX;
                }
            }
        }
    }

    public void Attack(Vector3 target)
    {
        this.isAttacking = true;
        this.startPosition = this.transform.position;
        this.targetPosition = new Vector3(target.x, this.transform.position.y, this.transform.position.z);
        this.finishedAttack = false;
    }

    public void Damage(int damage)
    {
        health = Mathf.Max(health - (damage < defPower ? 0 : damage - defPower), 0);

        BattleController.Instance.battleLog.Log(string.Format("{0} damaged {1} for {2} damage", BattleController.Instance.GetCurrentCharacter().characterName, this.characterName, Mathf.Max((damage < defPower ? 0 : damage - defPower), 0)));

        if (health == 0)
        {
            Die();
        }
    }

    public void Heal(int healing)
    {
        health += healing;
        BattleController.Instance.battleLog.Log(string.Format("{0} healed for {1} health", this.characterName, healing));
    }

    public void Buff(Spell spell)
    {

    }

    public void Defend()
    {
        defPower += (int)(defPower * 0.5);
        BattleController.Instance.battleLog.Log(string.Format("{0} used defend! (+50% damage reduction)", this.characterName));
    }

    public virtual void Die()
    {
        if (health == 0)
        {
            Destroy(this.gameObject);
            BattleController.Instance.battleLog.Log(string.Format("{0} has died!", this.characterName));
        }
    }
}
