using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SynergyAPI
{
    public class ExampleModule : ETGModule
    {
        public override void Init()
        {
        }

        public override void Start()
        {
            SynergyBuilder.Init();
            //creates a synergy
            //this synergy will be named "Test Synergy 1", the player will need to have thompson and magic lamp for it's completion, it will not be active when the player is not holding the guns required for it's completion, will halve spread when
            //  active and will also not be activated by lich's eye bullets.
            SynergyBuilder.CreateSynergy("Test Synergy 1", new List<string> { "thompson", "magic_lamp" }, null, false, new List<StatModifier> { StatModifier.Create(PlayerStats.StatType.Accuracy, StatModifier.ModifyMethod.MULTIPLICATIVE, 0.5f) }, 
                true, 2, false, false, null);
            //this synergy will be named "Test Synergy 2", the player will need to have the potion of lead skin and either alien sidearm or the void core assault rifle for it's completion, it will be active when the player is not holding the guns
            //  required for it's completion, will increase movement speed by 1 and will also be activated by lich's eye bullets.
            SynergyBuilder.CreateSynergy("Test Synergy 2", new List<string> { "potion_of_lead_skin" }, new List<string> { "alien_sidearm", "void_core_assault_rifle" }, true, new List<StatModifier> { 
                StatModifier.Create(PlayerStats.StatType.MovementSpeed, StatModifier.ModifyMethod.ADDITIVE, 1f) }, false, 2, false, false, null);
            //this synergy will be named "Test Synergy 3", the player will need to have either 4 of these items: elephant gun, makarov, hegemony rifle, void shotgun, bullet time, bomb, scope, honeycomb for it's completion, but the player will HAVE
            //  to have at least one gun and one item from that list, so the player can't complete the synergy by just having elephant gun, makarov and hegemony rifle or just having bullet time, bomb, scope and honeycomb.
            //  also because of how gungeon code works if a synergy has more than 2 items, lich's eye bullets will not complete the synergy by itself but instead can replace one of the elements of the synergy. so instead of having for example
            //  elephant gun, bullet time, scope and void shotgun the player can instead have for example elephant gun, bullet time, lich's eye bullets and void shotgun. the synergy will be active when the player is not holding the guns required for
            //  it's completion, when it's active it will multiply the thrown gun damage of the player by 3 and lich's eye bullets will be able to replace one element of the synergy. also this synergy will not have a vfx.
            SynergyBuilder.CreateSynergy("Test Synergy 3", null, new List<string> { "elephant_gun", "makarov", "hegemony_rifle", "void_shotgun", "bullet_time", "bomb", "scope", "honeycomb" }, true, new List<StatModifier> {
                StatModifier.Create(PlayerStats.StatType.ThrownGunDamage, StatModifier.ModifyMethod.MULTIPLICATIVE, 3f) }, false, 4, true, true, null);
        }

        public override void Exit()
        {
        }
    }
}
