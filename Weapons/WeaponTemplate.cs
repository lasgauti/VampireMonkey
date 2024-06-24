using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace VampireMonkey.Weapons
{
    public abstract class WeaponTemplate : ModContent
    {
        public int level = 0;
        public bool canBeSelected = true;
        public override void Register() { }
        public abstract string WeaponIcon { get; }
        public abstract string WeaponName { get; }
        public abstract int BaseProjectileCount { get; }
        public abstract float BaseProjectileDegree { get; }
        public abstract AttackModel BaseAttackModel { get; }
        public abstract VampireMonkey.UpgradeType[][] Upgrades {get;}
        public abstract float[][] Increase { get; }
        public abstract int MaxLevel { get; }
        public abstract void EditTower(Tower tower);
        public virtual bool Evolution { get; } = false;
       
    }
}
