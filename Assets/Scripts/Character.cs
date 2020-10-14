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

    public void Damage(int damage)
    {
        health = Mathf.Max(health - (damage < defPower ? 0 : damage - defPower), 0);

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
