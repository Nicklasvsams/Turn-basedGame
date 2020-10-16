using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions;

public class Character : MonoBehaviour
{
    public Animator animator;
    public bool isAttacking;
    public bool finishedAttack;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public float attackSpeed;
    protected SpriteRenderer renderer;

    public string characterName;
    public int health, maxHealth;
    public int mana, maxMana;
    public int atkPower, defPower;
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
            Spell spellToCast = null;

            Debug.Log("Spellname: " + spell.spellName);
            if (spell.spellName == "Fire Pillar")
            {
                Debug.Log("Fire Pillar instantiation");
                spellToCast = Instantiate<Spell>(spell, spellPosition, new Quaternion(0.7071f, 0, 0, 0.7071f));
            }
            else
            {
                Debug.Log("Other spell instantiated");
                spellToCast = Instantiate<Spell>(spell, spellPosition, Quaternion.identity);
            }

            mana -= spellToCast.cost;

            spellToCast.Cast(targetCharacter);

            return true;
        }
        else
        {
            Debug.LogFormat("Not enough mana to cast {0}", spell);
            return false;
        }
    }

    public void AttackAnimation()
    {
        if (isAttacking)
        {
            renderer = this.GetComponent<SpriteRenderer>();

            animator.SetBool("FinishedAttack", false);
            Debug.Log("Reached 0");
            if (!this.finishedAttack)
            {
                Debug.Log("Reached 1");
                if (Vector3.Distance(this.transform.position, targetPosition) > 0.1f)
                {
                    Debug.LogFormat("Reached 2: CharPos - {0} # TarPos: {1}", transform.position, targetPosition);
                    animator.SetBool("MovingToTarget", true);
                    transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, attackSpeed * Time.deltaTime);
                }
                else if (Vector3.Distance(this.transform.position, targetPosition) <= 0.1f)
                {
                    Debug.Log("Reached 3");
                    animator.SetBool("MovingToTarget", false);
                    animator.SetBool("ReachedTarget", true);

                    if (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && this.animator.GetCurrentAnimatorStateInfo(0).IsName(this.characterName + "_Attack"))
                    {
                        Debug.Log("Reached 4");
                        this.finishedAttack = true;
                        renderer.flipX = !renderer.flipX;
                    }
                }
            }
            else
            {
                if (Vector3.Distance(this.transform.position, startPosition) > 0)
                {
                    Debug.Log("Reached 5");
                    animator.SetBool("ReachedTarget", false);
                    animator.SetBool("MovingToTarget", true);

                    transform.position = Vector3.MoveTowards(this.transform.position, startPosition, attackSpeed * Time.deltaTime);
                }
                else if (Vector3.Distance(this.transform.position, startPosition) <= 0)
                {
                    Debug.Log("Reached 6");
                    animator.SetBool("MovingToTarget", false);
                    animator.SetBool("FinishedAttack", true);
                    isAttacking = false;

                    renderer.flipX = !renderer.flipX;
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
        
        BattleController.Instance.battleLog.SendMessageToChat(string.Format("{0} damaged {1} for {2} damage", BattleController.Instance.GetCurrentCharacter().characterName, this.characterName, Mathf.Max((damage < defPower ? 0 : damage - defPower), 0)));

        Debug.Log(BattleController.Instance.GetCurrentCharacter().characterName + " damaged " + this.characterName + " for " + Mathf.Max((damage < defPower ? 0 : damage - defPower), 0) + "damage");
        
        if (health == 0)
        {
            Die();
            Debug.Log(this.characterName + "has died.");
        }
    }

    public void Heal(int healing)
    {
        health = Mathf.Min((int)(Random.Range(100, 151) / 100) * healing, maxHealth);
    }

    public void Buff(Spell spell)
    {

    }

    public void Defend()
    {
        Debug.LogFormat("Def increased from {0} to {1}", defPower, (int)(defPower+defPower*0.5));
        defPower += (int)(defPower * 0.5);
    }

    public virtual void Die()
    {
        if (health == 0)
        {
            Destroy(this.gameObject);
            Debug.LogFormat("{0} has died", characterName);
        }
    }
}
