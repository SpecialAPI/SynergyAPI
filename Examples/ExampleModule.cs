using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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


            //adds a synergy called "Test Companion Synergy" between black hole gun and potion of gun friendship and makes black hole gun spawn super space turtle when held with this synergy active.
            SynergyBuilder.CreateSynergy("Test Companion Synergy", new List<string> { "black_hole_gun", "potion_of_gun_friendship" });
            (Gungeon.Game.Items["black_hole_gun"] as Gun).AddCompanionSynergyProcessor("Test Companion Synergy", false, false, Gungeon.Game.Enemies["super_space_turtle"].EnemyGuid);

            //adds a synergy called "Test Dual Wield Synergy" between regular shotgun and winchester and makes them dual wield when the synergy is active.
            SynergyBuilder.CreateSynergy("Test Dual Wield Synergy", new List<string> { "regular_shotgun", "winchester" });
            SynergyBuilder.AddDualWieldSynergyProcessor(Gungeon.Game.Items["regular_shotgun"] as Gun, Gungeon.Game.Items["winchester"] as Gun, "Test Dual Wield Synergy");

            //adds a synergy called "Test Gun Forme Synergy 1" between rube-adyne prototype and platinum bullets and also a synergy called "Test Gun Forme Synergy 2" between rube-adyne prototype and shadow bullets. if rube-adyne prototype is reloaded at
            //  full clip with the first synergy active, it will transform into rube-adyne mk2 like with the megahand synergies. if rube-adyne prototype is reloaded at full clip with the second synergy, it will transform into snakemaker.
            SynergyBuilder.CreateSynergy("Test Gun Forme Synergy 1", new List<string> { "rube_adyne_prototype", "platinum_bullets" });
            SynergyBuilder.CreateSynergy("Test Gun Forme Synergy 2", new List<string> { "rube_adyne_prototype", "shadow_bullets" });
            (Gungeon.Game.Items["rube_adyne_prototype"] as Gun).AddGunFormeSynergyProcessor().AddForme(true, "Test Gun Forme Synergy 1", Gungeon.Game.Items["rube_adyne_mk2"].PickupObjectId).AddForme(true, "Test Gun Forme Synergy 2", 
                Gungeon.Game.Items["snakemaker"].PickupObjectId);

            //adds a synergy called "Test Infinite Ammo Synergy" between cat claw and explosive rounds. cat claw will have infinite ammo and 0 reload time when the synergy is active.
            SynergyBuilder.CreateSynergy("Test Infinite Ammo Synergy", new List<string> { "cat_claw", "explosive_rounds" });
            (Gungeon.Game.Items["rube_adyne_prototype"] as Gun).AddInfiniteAmmoSynergyProcessor("Test Infinite Ammo Synergy", true);

            //adds a synergy called "Test Transform Gun Synergy" between banana and broccoli. banana will transform into it's unused "Fruits and Vegetables" synergy form when the synergy is active.
            SynergyBuilder.CreateSynergy("Test Transform Gun Synergy", new List<string> { "banana", "broccoli" });
            (Gungeon.Game.Items["rube_adyne_prototype"] as Gun).AddTransformGunSynergyProcessor("Test Transform Gun Synergy", (Gungeon.Game.Items["banana+fruits_and_vegetables"] as Gun).PickupObjectId, false, 0);

            //adds a synergy called "Test Hovering Gun Synergy" between the ring of chest friendship and the ring of mimic friendship. while the synergy is active, 3 mimic guns will orbit around the player, aiming at the nearest enemy and shooting
            //  when the owner shoots. the mimic guns will shoot for 2 seconds and then will have a cooldown of 4 seconds. they will make lower case r's "MIMIC" sound when they begin shooting, they will make the "CHEST" sound every shoot and they
            //  will make the unused "WOOD" sound after finishing shooting.
            SynergyBuilder.CreateSynergy("Test Hovering Gun Synergy", new List<string> { "ring_of_chest_friendship", "ring_of_mimic_friendship" });
            (Gungeon.Game.Items["ring_of_chest_friendship"] as PassiveItem).AddHoveringGunSynergyProcessor("Test Hovering Gun Synergy", Gungeon.Game.Items["734"].PickupObjectId, false, new List<int>(), HoveringGunController.HoverPosition.CIRCULATE,
                HoveringGunController.AimType.NEAREST_ENEMY, HoveringGunController.FireType.ON_FIRED_GUN, 4f, 2f, false, "Play_WPN_LowerCaseR_Chest_Mimic_01", "Play_WPN_LowerCaseR_Chest_Chest_01", "Play_WPN_LowerCaseR_Chest_Wood_01", 
                HoveringGunSynergyProcessor.TriggerStyle.CONSTANT, 3, -1f, false, 0f);
        }

        public override void Exit()
        {
        }
    }
}
