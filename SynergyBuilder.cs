using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Gungeon;

namespace SynergyAPI
{
    /// <summary>
    /// The core class of SynergyAPI that allows you to build synergies.
    /// </summary>
    public static class SynergyBuilder
    {
        /// <summary>
        /// Inits SynergyAPI.
        /// </summary>
        public static void Init()
        {
            m_synergyTable = typeof(StringTableManager).GetField("m_synergyTable", BindingFlags.NonPublic | BindingFlags.Static);
            synergies = new List<AdvancedSynergyEntry>();
            Strings = new AdvancedStringDB();
        }

        /// <summary>
        /// Unloads SynergyAPI.
        /// </summary>
        public static void Unload()
        {
            m_synergyTable = null;
            Strings?.Unload();
            Strings = null;
            synergies?.Clear();
            synergies = null;
        }

        /// <summary>
        /// Creates a new synergy.
        /// </summary>
        /// <param name="name">The name of the synergy.</param>
        /// <param name="mandatoryItems">Items that are always required for the completion of the synergy.</param>
        /// <param name="optionalItems">"Filler items" that will be needed to fill empty spaces in list of synergy-completing items.</param>
        /// <param name="activeWhenGunsUnequipped">If true, the synergy will still be active when the player is not holding the guns required for it's completion.</param>
        /// <param name="statModifiers">Stat modifiers that will be applied to the player when the synergy is active.</param>
        /// <param name="ignoreLichsEyeBullets">If true, Lich's Eye Bullets will not be able to activate the synergy.</param>
        /// <param name="numberObjectsRequired">Number of items required for the synergy's completion.</param>
        /// <param name="suppressVfx">If true, the synergy arrow VFX will not appear when the synergy is completed.</param>
        /// <param name="requiresAtLeastOneGunAndOneItem">If true, the player will have to have at least one item AND gun from either/both <paramref name="mandatoryIds"/> and <paramref name="optionalIds"/>.</param>
        /// <param name="bonusSynergies">List of "bonus synergies" for the synergy. Bonus synergies are used by base game items to detect if a synergy is active, but for modded synergies you don't need them.</param>
        /// <returns>The built synergy</returns>
        public static AdvancedSynergyEntry CreateSynergy(string name, List<PickupObject> mandatoryItems, List<PickupObject> optionalItems = default, bool activeWhenGunsUnequipped = true, List<StatModifier> statModifiers = default, bool ignoreLichsEyeBullets = false,
            int numberObjectsRequired = 2, bool suppressVfx = false, bool requiresAtLeastOneGunAndOneItem = false, List<CustomSynergyType> bonusSynergies = default)
        {
            List<int> manditemids = new List<int>();
            List<int> optitemids = new List<int>();
            foreach (PickupObject po in mandatoryItems)
            {
                if (po is Gun || po is PassiveItem || po is PlayerItem)
                {
                    manditemids.Add(po.PickupObjectId);
                }
            }
            foreach (PickupObject po in optionalItems)
            {
                if (po is Gun || po is PassiveItem || po is PlayerItem)
                {
                    optitemids.Add(po.PickupObjectId);
                }
            }
            return CreateSynergy(name, manditemids, optitemids, activeWhenGunsUnequipped, statModifiers, ignoreLichsEyeBullets, numberObjectsRequired, suppressVfx, requiresAtLeastOneGunAndOneItem, bonusSynergies);
        }

        /// <summary>
        /// Creates a new synergy.
        /// </summary>
        /// <param name="name">The name of the synergy.</param>
        /// <param name="mandatoryIds">Console ids of items that are always required for the completion of the synergy.</param>
        /// <param name="optionalIds">Console ids of "filler items" that will be needed to fill empty spaces in list of synergy-completing items.</param>
        /// <param name="activeWhenGunsUnequipped">If true, the synergy will still be active when the player is not holding the guns required for it's completion.</param>
        /// <param name="statModifiers">Stat modifiers that will be applied to the player when the synergy is active.</param>
        /// <param name="ignoreLichsEyeBullets">If true, Lich's Eye Bullets will not be able to activate the synergy.</param>
        /// <param name="numberObjectsRequired">Number of items required for the synergy's completion.</param>
        /// <param name="suppressVfx">If true, the synergy arrow VFX will not appear when the synergy is completed.</param>
        /// <param name="requiresAtLeastOneGunAndOneItem">If true, the player will have to have at least one item AND gun from either/both <paramref name="mandatoryIds"/> and <paramref name="optionalIds"/>.</param>
        /// <param name="bonusSynergies">List of "bonus synergies" for the synergy. Bonus synergies are used by base game items to detect if a synergy is active, but for modded synergies you don't need them.</param>
        /// <returns>The built synergy</returns>
        public static AdvancedSynergyEntry CreateSynergy(string name, List<string> mandatoryIds, List<string> optionalIds = default, bool activeWhenGunsUnequipped = true, List<StatModifier> statModifiers = default, bool ignoreLichsEyeBullets = false,
            int numberObjectsRequired = 2, bool suppressVfx = false, bool requiresAtLeastOneGunAndOneItem = false, List<CustomSynergyType> bonusSynergies = default)
        {
            List<int> manditemids = new List<int>();
            List<int> optitemids = new List<int>();
            foreach (string str in mandatoryIds)
            {
                PickupObject po = Game.Items[str];
                if (po is Gun || po is PassiveItem || po is PlayerItem)
                {
                    manditemids.Add(po.PickupObjectId);
                }
            }
            foreach (string str in optionalIds)
            {
                PickupObject po = Game.Items[str];
                if (po is Gun || po is PassiveItem || po is PlayerItem)
                {
                    optitemids.Add(po.PickupObjectId);
                }
            }
            return CreateSynergy(name, manditemids, optitemids, activeWhenGunsUnequipped, statModifiers, ignoreLichsEyeBullets, numberObjectsRequired, suppressVfx, requiresAtLeastOneGunAndOneItem, bonusSynergies);
        }

        /// <summary>
        /// Creates a new synergy.
        /// </summary>
        /// <param name="name">The name of the synergy.</param>
        /// <param name="mandatoryIds">Ids of items that are always required for the completion of the synergy.</param>
        /// <param name="optionalIds">Ids of "filler items" that will be needed to fill empty spaces in list of synergy-completing items.</param>
        /// <param name="activeWhenGunsUnequipped">If true, the synergy will still be active when the player is not holding the guns required for it's completion.</param>
        /// <param name="statModifiers">Stat modifiers that will be applied to the player when the synergy is active.</param>
        /// <param name="ignoreLichsEyeBullets">If true, Lich's Eye Bullets will not be able to activate the synergy.</param>
        /// <param name="numberObjectsRequired">Number of items required for the synergy's completion.</param>
        /// <param name="suppressVfx">If true, the synergy arrow VFX will not appear when the synergy is completed.</param>
        /// <param name="requiresAtLeastOneGunAndOneItem">If true, the player will have to have at least one item AND gun from either/both <paramref name="mandatoryIds"/> and <paramref name="optionalIds"/>.</param>
        /// <param name="bonusSynergies">List of "bonus synergies" for the synergy. Bonus synergies are used by base game items to detect if a synergy is active, but for modded synergies you don't need them.</param>
        /// <returns>The built synergy</returns>
        public static AdvancedSynergyEntry CreateSynergy(string name, List<int> mandatoryIds, List<int> optionalIds = default, bool activeWhenGunsUnequipped = true, List<StatModifier> statModifiers = default, bool ignoreLichsEyeBullets = false,
            int numberObjectsRequired = 2, bool suppressVfx = false, bool requiresAtLeastOneGunAndOneItem = false, List<CustomSynergyType> bonusSynergies = default)
        {
            if(Strings == null || synergies == null)
            {
                Init();
            }
            AdvancedSynergyEntry entry = new AdvancedSynergyEntry();
            string key = "#" + name.ToUpper().Replace(" ", "_").Replace("'", "").Replace(",", "").Replace(".", "");
            entry.NameKey = key;
            Strings.Synergies.Set(key, name);
            if (mandatoryIds != null)
            {
                foreach (int id in mandatoryIds)
                {
                    PickupObject po = PickupObjectDatabase.GetById(id);
                    if (po is Gun)
                    {
                        entry.MandatoryGunIDs.Add(id);
                    }
                    else if (po is PassiveItem || po is PlayerItem)
                    {
                        entry.MandatoryItemIDs.Add(id);
                    }
                }
            }
            if (optionalIds != null)
            {
                foreach (int id in optionalIds)
                {
                    PickupObject po = PickupObjectDatabase.GetById(id);
                    if (po is Gun)
                    {
                        entry.OptionalGunIDs.Add(id);
                    }
                    else if(po is PassiveItem || po is PlayerItem)
                    {
                        entry.OptionalItemIDs.Add(id);
                    }
                }
            }
            entry.ActiveWhenGunUnequipped = activeWhenGunsUnequipped;
            entry.statModifiers = new List<StatModifier>();
            if (statModifiers != null)
            {
                foreach (StatModifier mod in statModifiers)
                {
                    if (mod != null)
                    {
                        entry.statModifiers.Add(mod);
                    }
                }
            }
            entry.IgnoreLichEyeBullets = ignoreLichsEyeBullets;
            entry.NumberObjectsRequired = numberObjectsRequired;
            entry.SuppressVFX = suppressVfx;
            entry.RequiresAtLeastOneGunAndOneItem = requiresAtLeastOneGunAndOneItem;
            entry.bonusSynergies = new List<CustomSynergyType>();
            if (bonusSynergies != null)
            {
                foreach (CustomSynergyType type in bonusSynergies)
                {
                    entry.bonusSynergies.Add(type);
                }
            }
            synergies.Add(entry);
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { entry }).ToArray();
            return entry;
        }

        /// <summary>
        /// Changes <paramref name="original"/> to a name that can be used for checking synergies with <see cref="PlayerHasActiveSynergy(PlayerController, string)"/>. This is done automatically at the start of <see cref="PlayerHasActiveSynergy(PlayerController, string)"/>, so you don't need to do it manually
        /// </summary>
        /// <param name="original">The original synergy name.</param>
        /// <returns>The changed <paramref name="original"/> that can be used for checking synergies with <see cref="PlayerHasActiveSynergy(PlayerController, string)"/>.</returns>
        public static string FixSynergyAPISynergyName(string original)
        {
            string key = "#" + original.ToUpper().Replace(" ", "_").Replace("'", "").Replace(",", "").Replace(".", "");
            foreach(var s in synergies)
            {
                if(s.NameKey == key)
                {
                    return key;
                }
            }
            return original;
        }

        public static bool SynergyIsMatchingAndActive(this AdvancedSynergyEntry synergy, string nameToCheck)
        {
            if (synergy.NameKey == nameToCheck)
            {
                if (synergy.SynergyIsActive(GameManager.Instance.PrimaryPlayer, GameManager.Instance.SecondaryPlayer))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if <paramref name="player"/> has an active synergy with a name of <paramref name="synergyNameToCheck"/>. It's recommended to use this only with synergies added by this SynergyAPI, for base game synergies use <see cref="PlayerController.HasActiveBonusSynergy(CustomSynergyType, bool)"/>
        /// </summary>
        /// <param name="player">The player that will get checked.</param>
        /// <param name="synergyNameToCheck">The name of the synergy that will be checked.</param>
        /// <returns><see langword="true"/> if any matching active synergies were found, <see langword="false"/> otherwise.</returns>
        public static bool PlayerHasActiveSynergy(this PlayerController player, string synergyNameToCheck)
        {
            string actualSynergyName = FixSynergyAPISynergyName(synergyNameToCheck);
            foreach (int index in player.ActiveExtraSynergies)
            {
                AdvancedSynergyEntry synergy = GameManager.Instance.SynergyManager.synergies[index];
                if (synergy.SynergyIsMatchingAndActive(actualSynergyName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if anyone has an active synergy with the name of <paramref name="synergy"/>. It's recommended to use this only with synergies added by this SynergyAPI, for base game synergies use <see cref="PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType, out int)"/>
        /// </summary>
        /// <param name="synergy">The name of the synergy that will be checked.</param>
        /// <param name="count">The final count of matching synergies found.</param>
        /// <returns><see langword="true"/> if any matching active synergies were found, <see langword="false"/> otherwise.</returns>
        public static bool AnyoneHasActiveSynergy(string synergy, out int count)
        {
            string actualSynergy = FixSynergyAPISynergyName(synergy);
            count = 0;
            for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
            {
                if (!GameManager.Instance.AllPlayers[i].IsGhost)
                {
                    count += GameManager.Instance.AllPlayers[i].CountActiveBonusSynergies(actualSynergy);
                }
            }
            return count > 0;
        }

        /// <summary>
        /// Checks if anyone has an active synergy with the name of <paramref name="synergy"/>. It's recommended to use this only with synergies added by this SynergyAPI, for base game synergies use <see cref="PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType, out int)"/>
        /// </summary>
        /// <param name="synergy">The name of the synergy that will be checked.</param>
        /// <returns><see langword="true"/> if any matching active synergies were found, <see langword="false"/> otherwise.</returns>
        public static bool AnyoneHasActiveSynergy(string synergy)
        {
            return AnyoneHasActiveSynergy(synergy, out _);
        }

        /// <summary>
        /// Counts the active synergies of all players with the name of <paramref name="synergy"/>. It's recommended to use this only with synergies added by this SynergyAPI.
        /// </summary>
        /// <param name="synergy">The name of the synergies that will be counted.</param>
        /// <returns>The count of <paramref name="player"/>'s synergies that are named <paramref name="synergy"/>.</returns>
        public static int CountAllActiveMatchingSynergies(string synergy)
        {
            AnyoneHasActiveSynergy(synergy, out var num);
            return num;
        }

        /// <summary>
        /// Counts <paramref name="player"/>'s active synergies with the name of <paramref name="synergy"/>. It's recommended to use this only with synergies added by this SynergyAPI, for base game synergies use <see cref="PlayerController.CountActiveBonusSynergies(CustomSynergyType)"/>
        /// </summary>
        /// <param name="player">The player whose matching synergies will be counted.</param>
        /// <param name="synergy">The name of the synergies that will be counted.</param>
        /// <returns>The count of <paramref name="player"/>'s synergies that are named <paramref name="synergy"/>.</returns>
        public static int CountActiveBonusSynergies(this PlayerController player, string synergy)
        {
            if (player == null)
            {
                return 0;
            }
            string actualSynergy = FixSynergyAPISynergyName(synergy);
            int num = 0;
            foreach (int index in player.ActiveExtraSynergies)
            {
                AdvancedSynergyEntry entry = GameManager.Instance.SynergyManager.synergies[index];
                if (entry.SynergyIsMatchingAndActive(actualSynergy))
                {
                    num++;
                }
            }
            return num;
        }

        /// <summary>
        /// Synergies that were added by SynergyAPI;
        /// </summary>
        public static List<AdvancedSynergyEntry> synergies;
        /// <summary>
        /// <see cref="AdvancedStringDB"/> used by SynergyAPI;
        /// </summary>
        public static AdvancedStringDB Strings;
        /// <summary>
        /// <see cref="FieldInfo"/> for the synergy table.
        /// </summary>
        public static FieldInfo m_synergyTable;
    }
}
