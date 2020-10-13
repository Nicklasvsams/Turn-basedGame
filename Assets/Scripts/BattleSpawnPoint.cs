using UnityEngine;

public class BattleSpawnPoint : MonoBehaviour
{
    public Character Spawn(Character character)
    {
        Character charToSpawn = Instantiate<Character>(character, this.transform);
        return charToSpawn;
    }
}
