using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Simulation.Towers;
using VampireMonkey;

namespace VampireMonkey.Items
{
    public abstract class ItemTemplate : ModContent
    {
        public int level = 0;
        public override void Register() { }
        public abstract string ItemIcon { get; }
        public abstract string ItemName { get; }
        public abstract VampireMonkey.UpgradeType UpgradeType { get; }
        public abstract float Increase { get; }
        public abstract int MaxLevel { get; }
    }
}
