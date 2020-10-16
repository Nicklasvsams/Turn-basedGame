using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; set; }

    public Dictionary<int, List<Character>> characters = new Dictionary<int, List<Character>>();
    public int characterTurnIndex;
    public int battleTurnIndex;
    public Spell playerSelectedSpell;
    public bool playerSelectedAttack;

    public BattleLog battleLog;
    [SerializeField]
    private BattleSpawnPoint[] spawnPoints;
    [SerializeField]
    private BattleUIController uiController;

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

    private void NextAction()
    {
        Debug.LogFormat("Next action started Char:{0}/{1}", battleTurnIndex, characterTurnIndex);
        if(characters[0].Count > 0 && characters[1].Count > 0)
        {
            if (characterTurnIndex < characters[battleTurnIndex].Count - 1)
            {
                characterTurnIndex++;
            }
            else
            {
                NextTurn();
                characterTurnIndex = 0;
            }

            switch (battleTurnIndex)
            {
                case 0:
                    Debug.Log("Characters turn: " + BattleController.Instance.GetCurrentCharacter().characterName + " - " + battleTurnIndex + "/" + characterTurnIndex);
                    uiController.ToggleActionState(true);
                    uiController.BuildSpellList(GetCurrentCharacter().spells);
                    break;
                case 1:
                    Debug.Log("Enemy turn: " + BattleController.Instance.GetCurrentCharacter().characterName + " - " + battleTurnIndex + "/" + characterTurnIndex);
                    uiController.ToggleActionState(false);
                    StartCoroutine(PerformAction());
                    break;
            }

        }
        else
        {
            Debug.Log("Battle over!");
        }
    }

    IEnumerator PerformAction()
    {
        yield return new WaitForSeconds(2f);

        if (GetCurrentCharacter().health > 0 && battleTurnIndex == 1)
        {
            GetCurrentCharacter().GetComponent<Enemy>().Act();
        }

        yield return new WaitForSeconds(2f);

        NextAction();
    }

    public IEnumerator PerformAttack(Character attacker, Character target)
    {
        attacker.Attack(target.GetComponentInParent<Transform>().position);

        yield return new WaitForSeconds(Vector3.Distance(attacker.transform.position, target.transform.position) / 8f);

        target.Damage(attacker.atkPower);

        if (battleTurnIndex == 0)
        {
            NextAction();
        }
    }

    public void PerformDefense()
    {
        GetCurrentCharacter().Defend();
        NextAction();
    }

    public void SelectCharacter (Character character)
    {
        if (playerSelectedAttack)
        {
            StartCoroutine(PerformAttack(GetCurrentCharacter(), character));
            
        }
        else if (playerSelectedSpell != null)
        {
            if(GetCurrentCharacter().CastSpell(playerSelectedSpell, character))
            {
                NextAction();
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
        if (battleTurnIndex == 0)
        {
            return characters[battleTurnIndex][0];
        }
        else
        {
            return characters[battleTurnIndex][characterTurnIndex];
        }
    }
}
