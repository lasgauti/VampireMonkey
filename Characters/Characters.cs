using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Unity;
using VampireMonkey.Weapons;

namespace VampireMonkey.Characters
{
    public class DartMonkey : CharacterTemplate
    {
        public override string CharacterName => "Dart Monkey";
        public override string CharacterImage => VanillaSprites.DartMonkey000;
        public override string CharacterDescription => "Gain 8% Bonus Damage Every 5 Level";
        public override float PierceBonus => 1.5f;
        public override WeaponTemplate StarterWeapon => new Dart();
        public override void EditTower(Tower tower)
        {

        }
        public override void LevelUP(int level)
        {
            if (level % 5 == 0)
            {
                VampireMonkey.instance.damageBonus += 0.08f;
            }
        }
    }
    public class NinjaMonkey : CharacterTemplate
    {
        public override string CharacterName => "Ninja Monkey";
        public override string CharacterImage => VanillaSprites.NInjaMonkey000;
        public override string CharacterDescription => "Gain 3% Bonus Attack Speed Every 3 Level";
        public override float AttackSpeedBonus => 1.1f;
        public override float ProjectileSizeBonus => 1.15f;
        public override WeaponTemplate StarterWeapon => new Shuriken();
        public override void EditTower(Tower tower)
        {
         
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            towerModel.display = Game.instance.model.GetTower(TowerType.NinjaMonkey).display;
            tower.UpdateRootModel(towerModel);
        }
        public override void LevelUP(int level)
        {
            if (level % 3 == 0)
            {
                VampireMonkey.instance.attackSpeedBonus += 0.03f;
             
            }
        }
    }
    public class EngineerMonkey : CharacterTemplate
    {
        public override string CharacterName => "Engineer Monkey";
        public override string CharacterImage => VanillaSprites.EngineerMonkey000;
        public override string CharacterDescription => "Gain 1 Projectile Every 15 Levels";
        public override int ProjectileBonus => 1;
        public override WeaponTemplate StarterWeapon => new Nail();
        public override void EditTower(Tower tower)
        {

            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            towerModel.display = Game.instance.model.GetTower(TowerType.EngineerMonkey).display;
            tower.UpdateRootModel(towerModel);
        }
        public override void LevelUP(int level)
        {
            if (level % 15 == 0)
            {
                VampireMonkey.instance.projectileBonus += 1;
            }
        }
    }
    public class TackShooter : CharacterTemplate
    {
        public override string CharacterName => "Tack Shooter";
        public override string CharacterImage => VanillaSprites.TackShooter000;
        public override string CharacterDescription => "Gain 5% Range Every 4 Levels";
        public override float RangeBonus => 1.5f;
        public override WeaponTemplate StarterWeapon => new Tack();
        public override void EditTower(Tower tower)
        {

            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            towerModel.display = Game.instance.model.GetTower(TowerType.TackShooter).display;
            tower.UpdateRootModel(towerModel);
        }
        public override void LevelUP(int level)
        {
            if (level % 4 == 0)
            {
                VampireMonkey.instance.rangeBonus += 0.05f;
            }
        }
    }
    public class BombShooter : CharacterTemplate
    {
        public override string CharacterName => "Bomb Shooter";
        public override string CharacterImage => VanillaSprites.BombShooter000;
        public override string CharacterDescription => "Gain 1% Projectile Size Every Level";
        public override float ProjectileSizeBonus => 1.2f;
        public override float XPBonus => 1.3f;
        public override WeaponTemplate StarterWeapon => new Bomb();
        public override void EditTower(Tower tower)
        {

            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            towerModel.display = Game.instance.model.GetTower(TowerType.BombShooter).display;
            tower.UpdateRootModel(towerModel);
        }
        public override void LevelUP(int level)
        {
               
            VampireMonkey.instance.projectileSizeBonus += 0.01f;
        }
    }
    public class WizardMonkey : CharacterTemplate
    {
        public override string CharacterName => "Wizard Monkey";
        public override string CharacterImage => VanillaSprites.Wizard000;
        public override string CharacterDescription => "Gain 10% Pierce Every 8 Levels";
        public override float DamageBonus => 1.3f;
        public override WeaponTemplate StarterWeapon => new Magic();
        public override void EditTower(Tower tower)
        {

            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            towerModel.display = Game.instance.model.GetTower(TowerType.WizardMonkey).display;
            tower.UpdateRootModel(towerModel);
        }
        public override void LevelUP(int level)
        {

            if (level % 8 == 0)
            {
                VampireMonkey.instance.pierceBonus += 0.1f;
            }
        }
    }
    public class Druid : CharacterTemplate
    {
        public override string CharacterName => "Druid";
        public override string CharacterImage => VanillaSprites.Druid000;
        public override string CharacterDescription => "Gain 5% Xp Bonus Every Level";
        public override float DamageBonus => 1.05f;
        public override float PierceBonus => 1.05f;
        public override float AttackSpeedBonus => 1.05f;
        public override float XPBonus => 1.15f;
        public override float ProjectileSizeBonus => 1.05f;
        public override float RangeBonus => 1.05f;
        public override WeaponTemplate StarterWeapon => new Thorn();
        public override void EditTower(Tower tower)
        {

            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            towerModel.display = Game.instance.model.GetTower(TowerType.Druid).display;
            tower.UpdateRootModel(towerModel);
        }
        public override void LevelUP(int level)
        { 
            VampireMonkey.instance.XPBonus += 0.05f;
            
        }
    }
    public class BananaFarm : CharacterTemplate
    {
        public override string CharacterName => "Banana Farm";
        public override string CharacterImage => VanillaSprites.BananaFarm000;
        public override string CharacterDescription => "Gain 1% Monkey Bonus Every Level";
        public override float MoneyBonus => 1.15f;
        public override WeaponTemplate StarterWeapon => new Banana();
        public override void EditTower(Tower tower)
        {

            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            towerModel.display = Game.instance.model.GetTower(TowerType.BananaFarm).display;
            tower.UpdateRootModel(towerModel);
        }
        public override void LevelUP(int level)
        {
            VampireMonkey.instance.moneyBonus += 0.01f;

        }
    }
  
    public class SpikeFactory : CharacterTemplate
    {
        public override string CharacterName => "Spike Factory";
        public override string CharacterImage => VanillaSprites.SpikeFactory000;
        public override string CharacterDescription => "Gain 5% Pierce and 5% Damage Every 9 Levels";
        public override float DamageBonus => 1.1f;
        public override float PierceBonus => 1.1f;
        public override WeaponTemplate StarterWeapon => new Spike();
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            towerModel.display = Game.instance.model.GetTower(TowerType.SpikeFactory).display;
            tower.UpdateRootModel(towerModel);
        }
        public override void LevelUP(int level)
        {
            if (level % 9 == 0)
            {
                VampireMonkey.instance.pierceBonus += 0.05f;
                VampireMonkey.instance.damageBonus += 0.05f;
            }

        }
    }
    public class Quincy : CharacterTemplate
    {
        public override string CharacterName => "Quincy";
        public override string CharacterImage => VanillaSprites.QuincyIcon;
        public override string CharacterDescription => "Gain 1.5% Attack Speed, Range And Pierce Every 2 Levels. He Aso Never Miss!";
        public override WeaponTemplate StarterWeapon => new Arrow();
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            towerModel.display = Game.instance.model.GetTower(TowerType.Quincy).display;
            tower.UpdateRootModel(towerModel);
        }
        public override void LevelUP(int level)
        {
            if (level % 3 == 0)
            {
                VampireMonkey.instance.attackSpeedBonus += 0.015f;
                VampireMonkey.instance.rangeBonus += 0.015f;
                VampireMonkey.instance.pierceBonus += 0.015f;
            }

        }
    }

}

