using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UMA;
using UMA.CharacterSystem;
using UnityEngine;

namespace vwgamedev.gamecreator.uma{
[Title("Unequip Wardrobe")]
[Description("Unequips a Wardrobe Recipe from the UMA Character")]

[Category("UMA/Clothing/Unequip Wardrobe")]

[Parameter("wardrobeRecipe", "The Wardrobe Recipe the Character will unequip")]
[Parameter("Character", "The Character that to Equip the Wardrobe Slot")]

[Keywords("UMA", "Character", "Avatar", "Clothing")]
[Image(typeof(IconUMAWardrobeRecipe), ColorTheme.Type.Blue)]

[Serializable]
public class GCUMA_UnequipWardrobe : Instruction
{
    public override string Title => $"{this.m_Character} unequip {getWardrobeName()}";
    [SerializeField] private UMAWardrobeRecipe wardrobeRecipe;
    [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
    protected override Task Run(Args args)
    {
        Character character = this.m_Character.Get<Character>(args);
        if (character == null) return DefaultResult;
        DynamicCharacterAvatar uma = character.Animim.Animator.transform.GetComponent<DynamicCharacterAvatar>();
        if (uma != null)
        {
            uma.ClearSlot(wardrobeRecipe.wardrobeSlot);
            uma.BuildCharacter();
        } else
        {
            Debug.LogWarning("Trying to unequip wardrobe on a non UMA Character");
        }
        return DefaultResult;
    }

    private string getWardrobeName()
    {
        if (wardrobeRecipe != null)
        {
            return wardrobeRecipe.name;
        }
        return "";
    }
}

}
