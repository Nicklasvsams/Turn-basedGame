using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; set; }

    public Dictionary<int, List<Character>> characters = new Dictionary<int, List<Character>>();
    public int characterTurnIndex;
    public Spell playerSelectedSpell;
    public bool playerSelectedAttack;

    [SerializeField]
    private BattleSpawnPoint[] spawnPoints;
    [SerializeField]
    private BattleUIController uiController;
    private int battleTurnIndex;


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        characters.Add(0, new List<Character>());
        characters.Add(1, new List<Character>());

        FindObjectOfType<BattleLauncher>().Launch();
    }

    public Character GetPlayer()
    {
        return characters[0][0];
    }

    public Character GetWeakestEnemy()
    {
        Character weakestEnemy = characters[1][0];

        foreach(var character in characters[1])
        {
            if (character.health < weakestEnemy.health)
            {
                weakestEnemy = character;
            }
        }

        return weakestEnemy;
    }

    private void NextTurn()
    {
        battleTurnIndex = battleTurnIndex == 0 ? 1 : 0;
    }

    private void NextEnemyAction()
    {
        if(characters[0].Count > 0 && characters[1].Count > 0)
        {
            if (characterTurnIndex < characters[1].Count)
            {
                characterTurnIndex++;
            }
            else
            {
                characterTurnIndex = 0;
            }

            switch (battleTurnIndex)
            {
                case 0:
                    uiController.ToggleActionState(true);
                    uiController.BuildSpellList(GetCurrentCharacter().spells);
                    break;
                case 1:
                    uiController.ToggleActionState(false);
                    StartCoroutine(PerformAction());
                    break;
            }

            NextTurn();
        }
        else
        {
            Debug.Log("Battle over!");
        }
    }

    IEnumerator PerformAction()
    {
        yield return new WaitForSeconds(.5f);

        if (GetCurrentCharacter().health > 0)
        {
            GetCurrentCharacter().GetComponent<Enemy>().Act();
        }

        uiController.UpdateCharacterUI();

        yield return new WaitForSeconds(1f);
    }

    public void PerformAttack(Character attacker, Character target)
    {
        target.Damage(attacker.atkPower);
    }

    public void SelectCharacter (Character character)
    {
        if (playerSelectedAttack)
        {
            PerformAttack(GetCurrentCharacter(), character);
        }
        else if (playerSelectedSpell != null)
        {
            if(GetCurrentCharacter().CastSpell(playerSelectedSpell, character))
            {
                uiController.UpdateCharacterUI();
                NextEnemyAction();
            }
            else
            {
                Debug.LogWarning("Not enough mana.");
            }
        }
    }

    public void StartBattle(List<Character> player, List<Character> enemies)
    {
        Debug.Log("Setup battle!");

        for (int i = 0; i < player.Count; i++)
        {
            characters[0].Add(spawnPoints[i].Spawn(player[i]));
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            characters[1].Add(spawnPoints[i + 1].Spawn(enemies[i]));
        }
    }

    public Character GetCurrentCharacter()
    {
        return characters[battleTurnIndex][characterTurnIndex];
    }
}
