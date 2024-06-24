using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;
using VampireMonkey.Weapons;

namespace VampireMonkey.Characters
{
    public abstract class CharacterTemplate : ModContent
    {
        public static bool IsSelected(CharacterTemplate character) => VampireMonkey.instance.selectedCharacter == character;
        public override void Register() { }
        public abstract string CharacterName { get; }
        public abstract string CharacterImage { get; }
        public abstract string CharacterDescription { get; }
        public abstract WeaponTemplate StarterWeapon { get; }
        public virtual float RangeBonus { get; } = 1.00f;
        public virtual float PierceBonus { get; } = 1.00f;
        public virtual float DamageBonus { get; } = 1.00f;
        public virtual float AttackSpeedBonus { get; } = 1.00f;
        public virtual float XPBonus { get; } = 1.00f;
        public virtual int ProjectileBonus { get; } = 0;
        public virtual float ProjectileSizeBonus { get; } = 1.00f;
        public abstract void EditTower(Tower tower);
        public abstract void LevelUP(int level);
    }
}
