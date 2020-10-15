using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class BattleUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject spellPanel;
    [SerializeField]
    private Button button;
    [SerializeField]
    private Button[] actionButtons;
    [SerializeField]
    private Text[] enemyInfo;

    private void Start()
    {
        spellPanel.SetActive(false);
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Character") && BattleController.Instance.battleTurnIndex == 0)
            {
                BattleController.Instance.SelectCharacter(hitInfo.collider.GetComponent<Character>());
            }
        }

        UpdateCharacterUI();
    }

    public void ToggleSpellPanel(bool state)
    {
        spellPanel.SetActive(state);

        if (state == true)
        {
            BuildSpellList(BattleController.Instance.GetCurrentCharacter().spells);
        }
    }

    public void ToggleActionState(bool state)
    {
        ToggleSpellPanel(state);

        foreach(var button in actionButtons)
        {
            button.interactable = state;
        }
    }

    public void BuildSpellList(List<Spell> spells)
    {
        if (spellPanel.transform.childCount > 0)
        {
            foreach(var button in spellPanel.transform.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }

        foreach(var spell in spells)
        {
            Button spellButton = Instantiate<Button>(button, spellPanel.transform);
            spellButton.GetComponentInChildren<Text>().text = spell.spellName;
            spellButton.onClick.AddListener(() => SelectSpell(spell));
        }
    }

    void SelectSpell(Spell spell)
    {
        BattleController.Instance.playerSelectedSpell = spell;
        BattleController.Instance.playerSelectedAttack = false;
    }

    public void SelectAttack()
    {
        BattleController.Instance.playerSelectedSpell = null;
        BattleController.Instance.playerSelectedAttack = true;
    }

    public void Defend()
    {
        BattleController.Instance.PerformDefense();
    }

    public void UpdateCharacterUI()
    {
        for(int i = 0; i < BattleController.Instance.characters[1].Count; i++)
        {
            Character character = BattleController.Instance.characters[1][i];
            enemyInfo[i].text = string.Format("{0} > HP: {1}/{2} - MP: {3}/{4}", character.characterName, character.health, character.maxHealth, character.mana, character.maxMana);
        }

        if (enemyInfo.Length > BattleController.Instance.characters[1].Count)
        {
            for (int i = BattleController.Instance.characters[1].Count; i < enemyInfo.Length; i++)
            {
                enemyInfo[i].text = "";
            }
        }
    }

    public void Test()
    {
        Debug.Log("Button clicked.");
    }
}
