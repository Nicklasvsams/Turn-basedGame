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
        targetPosition = target.transform.position;

        if (spellType == SpellType.Attack)
        {
            Debug.Log(spellName + " was cast on " + target.name);
            target.Damage(power);
        }
        else if (spellType == SpellType.Heal)
        {
            // Heal damage
            Debug.Log(spellName + " was cast on " + target.name);
            target.Heal(power);
        }
        else if (spellType == SpellType.Buff)
        {
            // Give beneficial status
            Debug.Log(spellName + " was cast on " + target.name);
        }
    }
}
