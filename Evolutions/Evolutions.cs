using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Unity.PlatformLogins;
using VampireMonkey.Items;
using VampireMonkey.Weapons;

namespace VampireMonkey.Evolutions
{
    public class PlasmaEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new Plasma();
        public override ItemTemplate ItemRequired => new LightningBolt();
        public override WeaponTemplate WeaponRequired => new Dart();
    }
    public class DarkDoomEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new DarkDoom();
        public override ItemTemplate ItemRequired => new BlackSmith();
        public override WeaponTemplate WeaponRequired => new Shuriken();
    }
    public class RocketEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new Rocket();
        public override ItemTemplate ItemRequired => new SizePotion();
        public override WeaponTemplate WeaponRequired => new Nail();
    }
    public class DeathZoneEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new DeathZone();
        public override ItemTemplate ItemRequired => new DartsStock();
        public override WeaponTemplate WeaponRequired => new Tack();
    }
    public class SeekingExplosionEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new SeekingExplosion();
        public override ItemTemplate ItemRequired => new Goggles();
        public override WeaponTemplate WeaponRequired => new Bomb();
    }
    public class FireBallEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new FireBall();
        public override ItemTemplate ItemRequired => new MonkeyIndustry();
        public override WeaponTemplate WeaponRequired => new Magic();
    }
    public class JungleSwarmEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new JungleSwarm();
        public override ItemTemplate ItemRequired => new JungleDrums();
        public override WeaponTemplate WeaponRequired => new Thorn();
    }
    public class FactoryEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new Factory();
        public override ItemTemplate ItemRequired => new Fertilizer();
        public override WeaponTemplate WeaponRequired => new Banana();
    }
    public class MinesEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new Mines();
        public override ItemTemplate ItemRequired => new LightningBolt();
        public override WeaponTemplate WeaponRequired => new Spike();
    }
    public class FlameArrowEvolution : EvolutionTemplate
    {
        public override WeaponTemplate EvolutionWeapon => new FlameArrow();
        public override ItemTemplate ItemRequired => new BlackSmith();
        public override WeaponTemplate WeaponRequired => new Arrow();
    }
}
