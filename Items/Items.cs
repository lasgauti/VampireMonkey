using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Simulation.Towers;
using VampireMonkey.Characters;
using VampireMonkey.Weapons;

namespace VampireMonkey.Items
{
    public class Goggles : ItemTemplate
    {
        public override string ItemName => "Goggles";
        public override string ItemIcon => VanillaSprites.NightVisionGogglesUpgradeIcon;
        public override float Increase => 1.1f;
        public override VampireMonkey.UpgradeType UpgradeType => VampireMonkey.UpgradeType.Range;
        public override int MaxLevel => 5;
    }
    public class JungleDrums : ItemTemplate
    {
        public override string ItemName => "Jungle Drums";
        public override string ItemIcon => VanillaSprites.JungleDrumsUpgradeIcon;
        public override float Increase => 1.1f;
        public override VampireMonkey.UpgradeType UpgradeType => VampireMonkey.UpgradeType.AttackSpeed;
        public override int MaxLevel => 5;
    }
    public class DartsStock : ItemTemplate
    {
        public override string ItemName => "Darts Stock";
        public override string ItemIcon => VanillaSprites.TripleShotUpgradeIcon;
        public override float Increase => 1f;
        public override VampireMonkey.UpgradeType UpgradeType => VampireMonkey.UpgradeType.Projectiles;
        public override int MaxLevel => 3;
    }
    public class MonkeyIndustry : ItemTemplate
    {
        public override string ItemName => "Monkey Industry";
        public override string ItemIcon => VanillaSprites.MonkeyIntelligenceBureauUpgradeIcon;
        public override float Increase => 1.1f;
        public override VampireMonkey.UpgradeType UpgradeType => VampireMonkey.UpgradeType.Xp;
        public override int MaxLevel => 5;
    }
    public class BlackSmith : ItemTemplate
    {
        public override string ItemName => "Black Smith";
        public override string ItemIcon => VanillaSprites.PrimaryExpertiseUpgradeIcon;
        public override float Increase => 1.1f;
        public override VampireMonkey.UpgradeType UpgradeType => VampireMonkey.UpgradeType.Pierce;
        public override int MaxLevel => 5;
    }
    public class SizePotion : ItemTemplate
    {
        public override string ItemName => "Size Potion";
        public override string ItemIcon => VanillaSprites.LargerPotionsUpgradeIcon;
        public override float Increase => 1.1f;
        public override VampireMonkey.UpgradeType UpgradeType => VampireMonkey.UpgradeType.ProjectileSize;
        public override int MaxLevel => 5;
    }
    public class LightningBolt : ItemTemplate
    {
        public override string ItemName => "Lightning Bolt";
        public override string ItemIcon => VanillaSprites.HeartofThunderUpgradeIcon;
        public override float Increase => 1.1f;
        public override VampireMonkey.UpgradeType UpgradeType => VampireMonkey.UpgradeType.Damage;
        public override int MaxLevel => 5;
    }
}
