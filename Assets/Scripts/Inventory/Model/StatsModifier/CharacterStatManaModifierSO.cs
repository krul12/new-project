using UnityEngine;

[CreateAssetMenu]
public class CharacterStatManaModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Player player = character.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Mana+: " + val);
            player.AddMana(val);
        }
    }
}