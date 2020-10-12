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
            transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, spellSpeed * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, targetPosition) < 0.2f)
            {
                Destroy(this.gameObject, 1f);
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
