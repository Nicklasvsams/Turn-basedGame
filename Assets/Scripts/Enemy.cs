using System.Linq;
using UnityEngine;

public class Enemy : Character
{
    public void Act()
    {
        int randomAction = Random.Range(0, 2);

        switch (randomAction)
        {
            case 0:
                Defend();
                break;
            case 1:
                Spell spellToCast = GetRandomSpell();
                if (spellToCast.spellType == Spell.SpellType.Heal)
                {
                    // Heal friendly target with lowest HP
                }
                else if (!CastSpell(spellToCast, null))
                {
                    Debug.LogFormat("Not enough mana to cast {0}", spellToCast);
                    // Normal attack
                }
                break;
            case 2:
                // attack
                break;
        }
    }

    Spell GetRandomSpell()
    {
        return spells[Random.Range(0, spells.Count - 1)];
    }

    public override void Die()
    {
        base.Die();
    }
}
