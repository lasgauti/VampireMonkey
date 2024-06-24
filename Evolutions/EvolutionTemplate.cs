using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using VampireMonkey.Items;
using VampireMonkey.Weapons;

namespace VampireMonkey.Evolutions
{
    public abstract class EvolutionTemplate : ModContent
    {
        public override void Register() { }
        public abstract WeaponTemplate WeaponRequired { get; }
        public abstract ItemTemplate ItemRequired { get; }
        public abstract WeaponTemplate EvolutionWeapon { get; }
    }
}
