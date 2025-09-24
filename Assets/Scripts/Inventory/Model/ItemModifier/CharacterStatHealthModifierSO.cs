using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Player player = character.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("1");
            /*player.AddHealth((int)val);*/
        }
    }
}
