using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public int health, maxHealth;
    public int mana, maxMana;
    public int atkPower, defPower;
    public List<Spell> spells;

    public bool CastSpell(Spell spell, Character targetCharacter)
    {
        if (spell.cost <= mana)
        {
            Spell spellToCast = Instantiate<Spell>(spell, transform.position, Quaternion.identity);
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

    public void Damage(int damage)
    {
        health = Mathf.Max((int)(Random.Range(0, 101) / 100) * (damage < defPower ? 1 : damage - defPower), 0);
        if (health == 0)
        {
            // dedlul
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
        defPower += (int)(defPower * 0.5);
        Debug.Log("Def increased.");
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
