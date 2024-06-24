using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Unity.Jobs;
using UnityEngine;
using VampireMonkey.Characters;

namespace VampireMonkey.Weapons
{
   
    public class Projectiles : MonoBehaviour
    {
        public static AttackModel GetPlasmaAttack()
        {
            var plasmaAttack = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
            plasmaAttack.range *= 2.25f;
            plasmaAttack.weapons[0].rate = 0.3f;
            plasmaAttack.weapons[0].projectile.pierce = 15f;
            plasmaAttack.weapons[0].projectile.GetDamageModel().damage = 4.5f;
            plasmaAttack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
            plasmaAttack.weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.SuperMonkey, 2).GetAttackModel().weapons[0].projectile.display;
            plasmaAttack.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 1.5f;
            plasmaAttack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 2f;
            plasmaAttack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            plasmaAttack.name = "Plasma";
            return plasmaAttack.Duplicate();
        }
        public static AttackModel GetDarkDoomAttack()
        {
            var attack = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
            attack.fireWithoutTarget = true;
            attack.weapons[0].rate = 0.15f;
            attack.weapons[0].projectile.pierce = 50f;
            attack.weapons[0].projectile.GetDamageModel().damage = 2.5f;
            attack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
            attack.weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.SuperMonkey, 5).GetAttackModel().weapons[0].projectile.display;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 0.75f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 4f;
            attack.weapons[0].projectile.scale /= 1.2f;
            attack.weapons[0].projectile.radius *= 2.5f;
            attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.name = "DarkDoom";
            return attack.Duplicate();
        }
        public static AttackModel GetRocketAttack()
        {
            var attack = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().Duplicate();
            attack.weapons[0].rate = 3f;
            attack.weapons[0].projectile.pierce = 1;
            attack.weapons[0].projectile.scale *= 1.5f;
            var explosion = attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>();
            explosion.projectile.radius *= 3;
            explosion.projectile.GetDamageModel().damage = 100;
            explosion.projectile.pierce = 100;
            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 0.5f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 4f;
            attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.range = 999;
            return attack.Duplicate();
        }
        public static AttackModel GetDeathZoneAttack()
        {
            var attack = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().Duplicate();
            attack.fireWithoutTarget = true;
            attack.weapons[0].rate = 0.25f;
            attack.weapons[0].projectile.pierce = 5f;
            attack.weapons[0].projectile.GetDamageModel().damage = 2f;
            attack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
            attack.weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.SuperMonkey, 0, 5).GetAttackModel().weapons[0].projectile.display;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 1.5f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 1.5f;
            attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.range *= 1.75f;
            return attack.Duplicate();
        }
        public static AttackModel GetSeekingExplosionAttack()
        {
            var attack = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().Duplicate();
            attack.weapons[0].rate = 2f;
            var explosion = attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>();
            explosion.projectile.GetDamageModel().damage = 3;
            explosion.projectile.pierce = 15;
            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 6f;
            attack.weapons[0].projectile.maxPierce = 0;
            attack.weapons[0].projectile.pierce = 30;
            attack.weapons[0].projectile.AddBehavior(new ClearHitBloonsModel("ClearHitBloonsModel", 2f));
            attack.weapons[0].projectile.AddBehavior(new TrackTargetModel("TrackTargetModel", 999, true, false, 360, false, 360, false, false));
            attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.range *= 2;
            return attack.Duplicate();
        }
        public static AttackModel GetFireBallAttack()
        {
            var attack = Game.instance.model.GetTowerFromId("WizardMonkey").GetAttackModel().Duplicate();
            attack.range *= 2f;

            attack.weapons[0].rate = 0.8f;
            attack.weapons[0].projectile.pierce = 5f;
            attack.weapons[0].projectile.GetDamageModel().damage = 10f;
            attack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
            attack.weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.WizardMonkey, 0, 1).GetAttackModel(1).weapons[0].projectile.display;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 1.5f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 2.5f;
            attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.weapons[0].projectile.collisionPasses = new int[] { -1, 0 };
            var fire = Game.instance.model.GetTowerFromId("Alchemist").GetDescendant<AddBehaviorToBloonModel>().Duplicate();
            fire.GetBehavior<DamageOverTimeModel>().interval = 0.2f;
            fire.lifespan = 10;
            fire.lifespanFrames = 600;
            fire.GetBehavior<DamageOverTimeModel>().damage = 6;
            fire.overlayType = "Fire";
            attack.weapons[0].projectile.AddBehavior(fire);
            return attack.Duplicate();
        }
        public static AttackModel GetJungleSwarmAttack()
        {
            var attack = Game.instance.model.GetTowerFromId("Druid").GetAttackModel().Duplicate();
            attack.weapons[0].rate = 0.1f;
            attack.weapons[0].projectile.GetDamageModel().damage *= 2f;
            attack.weapons[0].projectile.pierce *= 2.5f;
            attack.weapons[0].projectile.scale *= 2;
            attack.weapons[0].projectile.radius *= 2;
            attack.range *= 2;
            return attack.Duplicate();
        }
    }

    public class Plasma : WeaponTemplate
    {
        public override int MaxLevel => 0;
        public override string WeaponIcon => VanillaSprites.PlasmaBlastUpgradeIcon;
        public override string WeaponName => "Plasma";
        public override int BaseProjectileCount => 5;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Projectiles.GetPlasmaAttack();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { };
        public override float[][] Increase => new float[][] {};
        public override bool Evolution => true;
        public override void EditTower(Tower tower)
        {
            var wpn = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            wpn.range *= 2.25f;
            wpn.weapons[0].rate = 0.3f;
            wpn.weapons[0].projectile.pierce = 15f;
            wpn.weapons[0].projectile.GetDamageModel().damage = 4.5f;
            wpn.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            wpn.weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.SuperMonkey, 2).GetAttackModel().weapons[0].projectile.display;
            wpn.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 1.5f;
            wpn.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 2f;
            wpn.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            wpn.name = WeaponName;
            towerModel.AddBehavior(wpn);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class DarkDoom : WeaponTemplate
    {
        public override int MaxLevel => 0;
        public override string WeaponIcon => VanillaSprites.SuperMonkey555;
        public override string WeaponName => "Dark Doom";
        public override int BaseProjectileCount => 4;
        public override float BaseProjectileDegree => 360;
        public override AttackModel BaseAttackModel => Projectiles.GetDarkDoomAttack();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { };
        public override float[][] Increase => new float[][] { };
        public override bool Evolution => true;
        public override void EditTower(Tower tower)
        {
            
            var attack = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            attack.fireWithoutTarget = true;
            attack.weapons[0].rate = 0.15f;
            attack.weapons[0].projectile.pierce = 50f;
            attack.weapons[0].projectile.GetDamageModel().damage = 2f;
            attack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
            attack.weapons[0].projectile.display = CosmeticHelper.SwapDarkTempleAsset(Game.instance.model.GetTower(TowerType.SuperMonkey, 5).GetAttackModel().weapons[0].projectile.display);
            attack.weapons[0].projectile.scale /= 1.2f;
            attack.weapons[0].projectile.radius *= 2.5f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 0.75f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 4f;
            attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.name = WeaponName;
            attack.weapons[0].ejectX = 0;
            attack.weapons[0].ejectY = 0;
            attack.weapons[0].ejectZ = 0;
            towerModel.AddBehavior(attack);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class Rocket : WeaponTemplate
    {
        public override int MaxLevel => 0;
        public override string WeaponIcon => VanillaSprites.MissileLauncherUpgradeIcon;
        public override string WeaponName => "Rocket";
        public override int BaseProjectileCount => 1;
        public override float BaseProjectileDegree => 60;
        public override AttackModel BaseAttackModel => Projectiles.GetRocketAttack();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { };
        public override float[][] Increase => new float[][] { };
        public override bool Evolution => true;
        public override void EditTower(Tower tower)
        {
            var attack = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            attack.weapons[0].rate = 3f;
            attack.weapons[0].projectile.pierce = 1;
            attack.weapons[0].projectile.scale *= 1.5f;
            
            var explosion = attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>();
            explosion.projectile.GetDamageModel().damage = 100;
            explosion.projectile.pierce = 100;
            explosion.projectile.radius *= 3;
            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attack.weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.scale *= 3.5f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 0.5f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 4f;
          
          attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.name = WeaponName;
            attack.range = 999;
            towerModel.AddBehavior(attack);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class DeathZone : WeaponTemplate
    {
        public override int MaxLevel => 0;
        public override string WeaponIcon => VanillaSprites.TheAntiBloonUpgradeIconAA;
        public override string WeaponName => "Death Zone";
        public override int BaseProjectileCount => 30;
        public override float BaseProjectileDegree => 360;
        public override AttackModel BaseAttackModel => Projectiles.GetDeathZoneAttack();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { };
        public override float[][] Increase => new float[][] { };
        public override bool Evolution => true;
        public override void EditTower(Tower tower)
        {
            var attack = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            attack.range *= 1.75f;
            attack.weapons[0].rate = 0.25f;
            attack.weapons[0].projectile.pierce = 5f;
            attack.weapons[0].projectile.GetDamageModel().damage = 2f;
            attack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
            attack.weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.SuperMonkey, 0, 5).GetAttackModel().weapons[0].projectile.display;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 1.5f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 1.5f;
            attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.name = WeaponName;
            towerModel.AddBehavior(attack);
            tower.UpdateRootModel(towerModel);
        }
    }
    public class SeekingExplosion : WeaponTemplate
    {
        public override int MaxLevel => 0;
        public override string WeaponIcon => VanillaSprites.MoabAssassinUpgradeIcon;
        public override string WeaponName => "Seeking Explosion";
        public override int BaseProjectileCount => 2;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Projectiles.GetSeekingExplosionAttack();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { };
        public override float[][] Increase => new float[][] { };
        public override bool Evolution => true;
        public override void EditTower(Tower tower)
        {
            var attack = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            attack.weapons[0].rate = 2f;

            var explosion = attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>();
            explosion.projectile.GetDamageModel().damage = 3;
            explosion.projectile.pierce = 15;
            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 6f;
            attack.weapons[0].projectile.maxPierce = 0;
            attack.weapons[0].projectile.pierce = 30;
            attack.weapons[0].projectile.AddBehavior(new ClearHitBloonsModel("ClearHitBloonsModel", 2f));
            attack.weapons[0].projectile.AddBehavior(new TrackTargetModel("TrackTargetModel", 999, true, false, 360, false, 360, false, false));
            attack.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("BombShooter-040").GetAttackModel().weapons[0].projectile.display;
          attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.name = WeaponName;
            attack.range *= 2;
            towerModel.AddBehavior(attack);
            tower.UpdateRootModel(towerModel);
        }
    }
    public class FireBall : WeaponTemplate
    {
        public override int MaxLevel => 0;
        public override string WeaponIcon => VanillaSprites.FireballUpgradeIcon;
        public override string WeaponName => "Fire Ball";
        public override int BaseProjectileCount => 5;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Projectiles.GetFireBallAttack();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { };
        public override float[][] Increase => new float[][] { };
        public override bool Evolution => true;
        public override void EditTower(Tower tower)
        {
            var attack = Game.instance.model.GetTowerFromId("WizardMonkey").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            attack.range *= 2f;
            
            attack.weapons[0].rate = 0.8f;
            attack.weapons[0].projectile.pierce = 5f;
            attack.weapons[0].projectile.GetDamageModel().damage = 10f;
            attack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
            attack.weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.WizardMonkey, 0, 1).GetAttackModel(1).weapons[0].projectile.display;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 1.5f;
            attack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 2.5f;
            attack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            attack.weapons[0].projectile.collisionPasses = new int[] { -1, 0 };
            var fire = Game.instance.model.GetTowerFromId("Alchemist").GetDescendant<AddBehaviorToBloonModel>().Duplicate();
            fire.GetBehavior<DamageOverTimeModel>().interval = 0.2f;
            fire.lifespan = 10;
            fire.lifespanFrames = 600;
            fire.GetBehavior<DamageOverTimeModel>().damage = 6;
            fire.overlayType = "Fire";
            attack.weapons[0].projectile.AddBehavior(fire);
            attack.name = WeaponName;
            towerModel.AddBehavior(attack);
            tower.UpdateRootModel(towerModel);
        }
    }
    public class JungleSwarm : WeaponTemplate
    {
        public override int MaxLevel => 0;
        public override string WeaponIcon => VanillaSprites.DruidoftheJungleUpgradeIcon;
        public override string WeaponName => "Jungle Swarm";
        public override int BaseProjectileCount => 30;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Projectiles.GetJungleSwarmAttack();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] {};
        public override bool Evolution => true;
        public override float[][] Increase => new float[][] { };
        public override void EditTower(Tower tower)
        {
            var wpn = Game.instance.model.GetTowerFromId("Druid").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            wpn.name = WeaponName;
            wpn.weapons[0].rate = 0.1f;
            wpn.weapons[0].projectile.GetDamageModel().damage *= 2f;
            wpn.weapons[0].projectile.pierce *= 2.5f;
            wpn.weapons[0].projectile.scale *= 2;
            wpn.weapons[0].projectile.radius *= 2;
            wpn.range *= 2;
            wpn.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            wpn.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            towerModel.AddBehavior(wpn);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class Dart : WeaponTemplate
    {
        public override int MaxLevel => 10;
        public override string WeaponIcon => VanillaSprites.SharpShotsUpgradeIcon;
        public override string WeaponName => "Dart";
        public override int BaseProjectileCount => 1;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { new[] { VampireMonkey.UpgradeType.Projectiles }/*1*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*2*/, new[] { VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Range }/*3*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.MIB }/*4*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce }/*5*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*6*/, new[] { VampireMonkey.UpgradeType.Range }/*7*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce, VampireMonkey.UpgradeType.Projectiles }/*8*/, new[] { VampireMonkey.UpgradeType.Projectiles }/*9*/, new[] { VampireMonkey.UpgradeType.Damage, VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Pierce }/*10*/};
        public override float[][] Increase => new float[][] { new[] { 1f }/*1*/, new[] { 1f, 1.5f }/*2*/, new[] { 1.25f, 1.5f }/*3*/, new[] { 1f, 1.5f }/*4*/, new[] { 1.5f, 1.5f }/*5*/, new[] { 1f, 1.5f }/*6*/, new[] { 1.5f }/*7*/, new[] { 1.5f, 1.5f, 1f }/*8*/, new[] { 1f }/*9*/, new[] { 1.5f, 1.25f, 1.5f }/*10*/};
        public override void EditTower(Tower tower)
        {
            var wpn = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            wpn.name = WeaponName;
            towerModel.AddBehavior(wpn);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class Shuriken : WeaponTemplate
    {
        public override int MaxLevel => 10;
        public override string WeaponIcon => VanillaSprites.NinjaDisciplineUpgradeIcon;
        public override string WeaponName => "Shuriken";
        public override int BaseProjectileCount => 1;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Game.instance.model.GetTowerFromId("NinjaMonkey").GetAttackModel().Duplicate();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { new[] { VampireMonkey.UpgradeType.Projectiles }/*1*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*2*/, new[] { VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Range }/*3*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.MIB }/*4*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce }/*5*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*6*/, new[] { VampireMonkey.UpgradeType.Range }/*7*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce, VampireMonkey.UpgradeType.Projectiles }/*8*/, new[] { VampireMonkey.UpgradeType.Projectiles }/*9*/, new[] { VampireMonkey.UpgradeType.Damage, VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Pierce }/*10*/};
        public override float[][] Increase => new float[][] { new[] { 1f }/*1*/, new[] { 1f, 1.5f }/*2*/, new[] { 1.25f, 1.5f }/*3*/, new[] { 1f, 1.5f }/*4*/, new[] { 1.5f, 1.5f }/*5*/, new[] { 1f, 1.5f }/*6*/, new[] { 1.5f }/*7*/, new[] { 1.5f, 1.5f, 1f }/*8*/, new[] { 1f }/*9*/, new[] { 1.5f, 1.25f, 1.5f }/*10*/};
        public override void EditTower(Tower tower)
        {
            var wpn = Game.instance.model.GetTowerFromId("NinjaMonkey").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            wpn.name = WeaponName;
            towerModel.AddBehavior(wpn);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class Nail : WeaponTemplate
    {
        public override int MaxLevel => 10;
        public override string WeaponIcon => VanillaSprites.PinUpgradeIcon;
        public override string WeaponName => "Nail";
        public override int BaseProjectileCount => 1;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Game.instance.model.GetTowerFromId("EngineerMonkey").GetAttackModel().Duplicate();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { new[] { VampireMonkey.UpgradeType.Projectiles }/*1*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*2*/, new[] { VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Range }/*3*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.MIB }/*4*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce }/*5*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*6*/, new[] { VampireMonkey.UpgradeType.Range }/*7*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce, VampireMonkey.UpgradeType.Projectiles }/*8*/, new[] { VampireMonkey.UpgradeType.Projectiles }/*9*/, new[] { VampireMonkey.UpgradeType.Damage, VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Pierce }/*10*/};
        public override float[][] Increase => new float[][] { new[] { 1f }/*1*/, new[] { 1f, 1.5f }/*2*/, new[] { 1.25f, 1.5f }/*3*/, new[] { 1f, 1.5f }/*4*/, new[] { 1.5f, 1.5f }/*5*/, new[] { 1f, 1.5f }/*6*/, new[] { 1.5f }/*7*/, new[] { 1.5f, 1.5f, 1f }/*8*/, new[] { 1f }/*9*/, new[] { 1.5f, 1.25f, 1.5f }/*10*/};
        public override void EditTower(Tower tower)
        {
            var wpn = Game.instance.model.GetTowerFromId("EngineerMonkey").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            wpn.name = WeaponName;
            towerModel.AddBehavior(wpn);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class Tack : WeaponTemplate
    {
        public override int MaxLevel => 10;
        public override string WeaponIcon => VanillaSprites.MoreTacksUpgradeIcon;
        public override string WeaponName => "Tack";
        public override int BaseProjectileCount => 8;
        public override float BaseProjectileDegree => 360;
        public override AttackModel BaseAttackModel => Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().Duplicate();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { new[] { VampireMonkey.UpgradeType.Projectiles }/*1*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*2*/, new[] { VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Range }/*3*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.MIB }/*4*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce }/*5*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*6*/, new[] { VampireMonkey.UpgradeType.Range }/*7*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce, VampireMonkey.UpgradeType.Projectiles }/*8*/, new[] { VampireMonkey.UpgradeType.Projectiles }/*9*/, new[] { VampireMonkey.UpgradeType.Damage, VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Pierce }/*10*/};
        public override float[][] Increase => new float[][] { new[] { 8f }/*1*/, new[] { 8f, 1.5f }/*2*/, new[] { 1.25f, 1.5f }/*3*/, new[] { 8f, 1.5f }/*4*/, new[] { 1.5f, 1.5f }/*5*/, new[] { 8f, 1.5f }/*6*/, new[] { 1.5f }/*7*/, new[] { 1.5f, 1.5f, 8f }/*8*/, new[] { 8f }/*9*/, new[] { 1.5f, 1.25f, 1.5f }/*10*/};
        public override void EditTower(Tower tower)
        {
            var wpn = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            wpn.name = WeaponName;
            towerModel.AddBehavior(wpn);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class Bomb : WeaponTemplate
    {
        public override int MaxLevel => 10;
        public override string WeaponIcon => VanillaSprites.BiggerBombsUpgradeIcon;
        public override string WeaponName => "Bomb";
        public override int BaseProjectileCount => 1;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().Duplicate();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { new[] { VampireMonkey.UpgradeType.Projectiles }/*1*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*2*/, new[] { VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Range }/*3*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.MIB }/*4*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce }/*5*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*6*/, new[] { VampireMonkey.UpgradeType.Range }/*7*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce, VampireMonkey.UpgradeType.Projectiles }/*8*/, new[] { VampireMonkey.UpgradeType.Projectiles }/*9*/, new[] { VampireMonkey.UpgradeType.Damage, VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Pierce }/*10*/};
        public override float[][] Increase => new float[][] { new[] { 1f }/*1*/, new[] { 1f, 1.5f }/*2*/, new[] { 1.25f, 1.5f }/*3*/, new[] { 1f, 1.5f }/*4*/, new[] { 1.5f, 1.5f }/*5*/, new[] { 1f, 1.5f }/*6*/, new[] { 1.5f }/*7*/, new[] { 1.5f, 1.5f, 1f }/*8*/, new[] { 8f }/*9*/, new[] { 1.5f, 1.25f, 1.5f }/*10*/};
        public override void EditTower(Tower tower)
        {
            var wpn = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            wpn.name = WeaponName;
            towerModel.AddBehavior(wpn);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class Magic : WeaponTemplate
    {
        public override int MaxLevel => 10;
        public override string WeaponIcon => VanillaSprites.IntenseMagicUpgradeIcon;
        public override string WeaponName => "Magic";
        public override int BaseProjectileCount => 1;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Game.instance.model.GetTowerFromId("WizardMonkey").GetAttackModel().Duplicate();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { new[] { VampireMonkey.UpgradeType.Projectiles }/*1*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*2*/, new[] { VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Range }/*3*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.MIB }/*4*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce }/*5*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*6*/, new[] { VampireMonkey.UpgradeType.Range }/*7*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce, VampireMonkey.UpgradeType.Projectiles }/*8*/, new[] { VampireMonkey.UpgradeType.Projectiles }/*9*/, new[] { VampireMonkey.UpgradeType.Damage, VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Pierce }/*10*/};
        public override float[][] Increase => new float[][] { new[] { 1f }/*1*/, new[] { 1f, 1.5f }/*2*/, new[] { 1.25f, 1.5f }/*3*/, new[] { 1f, 1.5f }/*4*/, new[] { 1.5f, 1.5f }/*5*/, new[] { 1f, 1.5f }/*6*/, new[] { 1.5f }/*7*/, new[] { 1.5f, 1.5f, 1f }/*8*/, new[] { 1f }/*9*/, new[] { 1.5f, 1.25f, 1.5f }/*10*/};
        public override void EditTower(Tower tower)
        {
            var wpn = Game.instance.model.GetTowerFromId("WizardMonkey").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            wpn.name = WeaponName;
            towerModel.AddBehavior(wpn);
            tower.UpdateRootModel(towerModel);

        }
    }
    public class Thorn : WeaponTemplate
    {
        public override int MaxLevel => 10;
        public override string WeaponIcon => VanillaSprites.ThornSwarmUpgradeIcon;
        public override string WeaponName => "Thorn";
        public override int BaseProjectileCount => 5;
        public override float BaseProjectileDegree => 45;
        public override AttackModel BaseAttackModel => Game.instance.model.GetTowerFromId("Druid").GetAttackModel().Duplicate();
        public override VampireMonkey.UpgradeType[][] Upgrades => new VampireMonkey.UpgradeType[][] { new[] { VampireMonkey.UpgradeType.Projectiles }/*1*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*2*/, new[] { VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Range }/*3*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.MIB }/*4*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce }/*5*/, new[] { VampireMonkey.UpgradeType.Projectiles, VampireMonkey.UpgradeType.Damage }/*6*/, new[] { VampireMonkey.UpgradeType.Range }/*7*/, new[] { VampireMonkey.UpgradeType.ProjectileSize, VampireMonkey.UpgradeType.Pierce, VampireMonkey.UpgradeType.Projectiles }/*8*/, new[] { VampireMonkey.UpgradeType.Projectiles }/*9*/, new[] { VampireMonkey.UpgradeType.Damage, VampireMonkey.UpgradeType.AttackSpeed, VampireMonkey.UpgradeType.Pierce }/*10*/};
        public override float[][] Increase => new float[][] { new[] { 5f }/*1*/, new[] { 1f, 1.5f }/*2*/, new[] { 1.25f, 1.5f }/*3*/, new[] { 5f, 1.5f }/*4*/, new[] { 1.5f, 1.5f }/*5*/, new[] { 5f, 1.5f }/*6*/, new[] { 1.5f }/*7*/, new[] { 1.5f, 1.5f, 5f }/*8*/, new[] { 5f }/*9*/, new[] { 1.5f, 1.25f, 1.5f }/*10*/};
        public override void EditTower(Tower tower)
        {
            var wpn = Game.instance.model.GetTowerFromId("Druid").GetAttackModel().Duplicate();
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            wpn.name = WeaponName;
            towerModel.AddBehavior(wpn);
            tower.UpdateRootModel(towerModel);

        }
    }
}
