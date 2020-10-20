using UnityEngine;

public class Spell : MonoBehaviour
{
    private Vector3 targetPosition;

    public enum SpellType { Attack, Heal, Buff }
    public SpellType spellType;
    public string spellName;
    public int power;
    public int cost;
    public float spellSpeed;

    private void Update()
    {
        if (targetPosition != Vector3.zero)
        {
            var offset = new Vector3(this.transform.position.x, this.transform.position.y, -2);
            var targetOffset = new Vector3(targetPosition.x, targetPosition.y + (this.transform.localScale.y * 0.5f), -2);

            transform.position = Vector3.MoveTowards(offset, targetOffset, spellSpeed * Time.deltaTime);

            if (Vector3.Distance(offset, targetOffset) < 1f)
            {
                Destroy(this.gameObject, 3f);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Cast(Character target)
    {
        var targetComponent = target.GetComponent<BoxCollider2D>().offset;
        targetPosition = new Vector3(targetComponent.x + target.transform.position.x, targetComponent.y + target.transform.position.y, target.transform.position.z);

        if (spellType == SpellType.Attack)
        {
            BattleController.Instance.battleLog.Log(string.Format("{0} was cast on {1}", spellName, target.characterName));

            target.Damage(power);
        }
        else if (spellType == SpellType.Heal)
        {
            BattleController.Instance.battleLog.Log(string.Format("{0} was cast on {1}", spellName, target.characterName));
            
            target.Heal(power);
        }
        else if (spellType == SpellType.Buff)
        {
            // Give beneficial status
        }
    }
}
