using UnityEngine;

public class Enemy : Character
{
    private void Start()
    {
        this.animator.Play(this.characterName + "_Idle", 0, Random.Range(0.0f, 1.0f));
    }

    public void Act()
    { 
        int randomAction = 2 /*Random.Range(0, 2)*/;
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
                    StartCoroutine(BattleController.Instance.PerformAttack(this, target));
                }
                break;
            case 2:
                StartCoroutine(BattleController.Instance.PerformAttack(this, target));
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
        BattleController.Instance.characters[1].Remove(this);
    }
}
