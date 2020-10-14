﻿using UnityEngine;

public class Enemy : Character
{
    public void Act()
    {
        int randomAction = Random.Range(0, 2);
        Character target = BattleController.Instance.GetPlayer();

        switch (randomAction)
        {
            case 0:
                Defend();
                break;
            case 1:
                Spell spellToCast = GetRandomSpell();
                if (spellToCast.spellType == Spell.SpellType.Heal)
                {
                    target = BattleController.Instance.GetWeakestEnemy();
                }
                else if (!CastSpell(spellToCast, target))
                {
                    BattleController.Instance.PerformAttack(this, target);
                }
                break;
            case 2:
                BattleController.Instance.PerformAttack(this, target);
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
        Debug.LogFormat("Enemy {0} died", this.characterName);
        BattleController.Instance.characters[1].Remove(this);
    }
}
