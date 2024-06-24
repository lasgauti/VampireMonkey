using MelonLoader;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using UnityEngine;
using System.Linq;
using Il2CppAssets.Scripts.Simulation.Objects;
using BTD_Mod_Helper.Api.Towers;
using Il2CppNinjaKiwi.Common;
using Il2CppAssets.Scripts.Models.TowerSets;
using VampireMonkey;
using VampireMonkey.Characters;
using BTD_Mod_Helper.Api;
using Il2CppTMPro;
using Il2CppSteamNative;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Simulation.Display;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2Cpp;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using System.Threading;
using Il2CppAssets.Scripts.Data.Gameplay.Mods;
using UnityEngine.UIElements;
using Il2CppAssets.Scripts.Unity;
using VampireMonkey.Weapons;
using System.Collections.Generic;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Unity.Towers.Upgrades;
using Il2CppAssets.Scripts.Data.Quests;
using Il2CppFacepunch.Steamworks;
using VampireMonkey.Items;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppSystem.Security.Cryptography;
using static Il2CppFacepunch.Steamworks.Inventory;
using Il2CppSystem.Numerics;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using VampireMonkey.Evolutions;
using FuzzySharp.Utils;

[assembly: MelonInfo(typeof(VampireMonkey.VampireMonkey), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace VampireMonkey;

public class VampireMonkey : BloonsTD6Mod
{

    public static VampireMonkey instance;
    public bool hasSelectedCharacter = false;
    public Tower tower;
    public float Xp;
    public float XpMax;
    public CharacterTemplate? activeCharacter = new DartMonkey();
    public float rangeBonus;
    public float pierceBonus;
    public float damageBonus;
    public float attackSpeedBonus;
    public float XPBonus;
    public int projectileBonus;
    public float projectileSizeBonus;
    public bool selectingCharacter;
    public int itemLimit = 6;
    public int weaponLimit = 6;
    public List<WeaponTemplate> weapons = new List<WeaponTemplate>();
    public List<ItemTemplate> items = new List<ItemTemplate>();
    public int level = 0;
    float Cooldown;
    public enum UpgradeType
    {
        Damage,
        Pierce,
        AttackSpeed,
        Range,
        ProjectileSize,
        Projectiles,
        Xp,
        MIB,
    }

    public CharacterTemplate selectedCharacter
    {
        get { return activeCharacter; }
        set { activeCharacter = value; }
    }
    public class VampireMonkeyTower : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Primary;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 500;
        public override string DisplayName => "Vampire Monkey";
        public override string Name => "VampireMonkey";
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;
        public override string Description => "Choose between different characters to affect the gameplay differently. Gain XP when he pop a Bloon";
        public override string Portrait => "AncientMonkeyIcon";
        public override string Icon => "AncientMonkeyIcon";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.range = 0;
            attackModel.weapons[0].rate = 99999;
            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan = 0.01f;
        }
    }
    public override void OnApplicationStart()
    {
        instance = this;
    }
    public override void OnNewGameModel(GameModel result)
    {
        foreach (var tower in result.towerSet.ToList())
        {
            if (tower.name.Contains("VampireMonkey-VampireMonkey"))
            {
                tower.GetShopTowerDetails().towerCount = 1;
            }
        }
    }
    public void Reset()
    {
        hasSelectedCharacter = false;
        Xp = 0;
        XpMax = 5;
        weapons.Clear();
        items.Clear();
        selectingCharacter = false;
        level = 0;

        foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
        {
            weapon.level = 0;
            if (weapon.Evolution)
            {
                weapon.canBeSelected = false;
            }
            else
            {
                weapon.canBeSelected = true;
            }
        }
        foreach (var item in ModContent.GetContent<ItemTemplate>())
        {
            item.level = 0;
        }
    }
    public override void OnGameModelLoaded(GameModel model)
    {
        Reset();
        
    }
    public override void OnTowerSold(Tower tower, float amount)
    {
        if (tower.towerModel.name.Contains("VampireMonkey-VampireMonkey"))
        {
            Reset();
            instance.tower = null;

        }
    }
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (tower.towerModel.name.Contains("VampireMonkey-VampireMonkey"))
        {
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            MenuUi.CreateCharacterMenu(rect, tower);
            Reset();
            instance.tower = tower;
        }

    }
    public override void OnFixedUpdate()
    {
        
        if (InGame.instance == null || InGame.instance.bridge == null) { return; }
        if (Cooldown <= 0)
        {
            if (tower != null)
            {
                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
                foreach (var weapon in weapons)
                {
                    var damage = 1f;
                    var pierce = 1f;
                    var range = 1f;
                    var projectileSize = 1f;
                    var projectiles = 0;
                    var attackSpeed = 1f;
                    var mib = false;
                    for (var i = 0; i < weapon.level; i++)
                    {
                        var stats = weapon.Upgrades[i];
                       
                        for (var y = 0; y < stats.Count(); y++)
                        {
                            var stat = stats[y];
                            if (stat == UpgradeType.Damage)
                            {
                                damage += (weapon.Increase[i][y] - 1);
                            }
                            if (stat == UpgradeType.Pierce)
                            {
                                pierce += (weapon.Increase[i][y] - 1);
                            }
                            if (stat == UpgradeType.Range)
                            {
                                range += (weapon.Increase[i][y] - 1);
                            }
                            if (stat == UpgradeType.ProjectileSize)
                            {
                                projectileSize += (weapon.Increase[i][y] - 1);
                            }
                            if (stat == UpgradeType.Projectiles)
                            {
                                projectiles += (int)weapon.Increase[i][y];
                            }
                            if (stat == UpgradeType.AttackSpeed)
                            {
                                attackSpeed += (weapon.Increase[i][y] - 1);
                            }
                            if (stat == UpgradeType.MIB)
                            {
                                mib = true;
                            }
                        }
                    }

                    foreach (var weapons in ModContent.GetContent<WeaponTemplate>()) 
                    {
                        ModHelper.Log<VampireMonkey>(weapons.Name + " " + weapons.canBeSelected);
                    }
                    var attackModel = towerModel.GetAttackModel(weapon.WeaponName);
                    if (attackModel != null)
                    {
                        attackModel.range = weapon.BaseAttackModel.range * rangeBonus * range;
                        attackModel.weapons[0].rate = weapon.BaseAttackModel.weapons[0].rate / (attackSpeedBonus + (attackSpeed - 1));
                        attackModel.weapons[0].projectile.scale = weapon.BaseAttackModel.weapons[0].projectile.scale * projectileSizeBonus * projectileSize;
                        attackModel.weapons[0].projectile.radius = weapon.BaseAttackModel.weapons[0].projectile.radius * projectileSizeBonus * projectileSize;
                        attackModel.weapons[0].projectile.pierce = weapon.BaseAttackModel.weapons[0].projectile.pierce * pierceBonus * pierce;
                        if (attackModel.weapons[0].projectile.HasBehavior<DamageModel>())
                        {
                            attackModel.weapons[0].projectile.GetDamageModel().damage = weapon.BaseAttackModel.weapons[0].projectile.GetDamageModel().damage * damageBonus * damage;
                            if (mib)
                            {
                                attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            }
                        }
                        attackModel.weapons[0].emission = new ArcEmissionModel("Emission", weapon.BaseProjectileCount + projectileBonus + projectiles, 0, weapon.BaseProjectileDegree, null, false, false);
                        if (mib)
                        {
                            attackModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                        }
                        if (attackModel.weapons[0].projectile.HasBehavior<CreateProjectileOnContactModel>())
                        {
                            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = weapon.BaseAttackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce * pierceBonus * pierce;
                            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = weapon.BaseAttackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage * damageBonus * damage;
                            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius = weapon.BaseAttackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius * projectileSize * projectileSizeBonus;
                            if (mib)
                            {
                                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            }
                            attackModel.weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.scale = weapon.BaseAttackModel.weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.scale * projectileSize * projectileSizeBonus;
                        }
                    }
                }
                tower.UpdateRootModel(towerModel);
            }
            Cooldown += 1;
        }
        Cooldown -= Time.deltaTime;
    }

    public void CharacterSelected(CharacterTemplate character, Tower tower)
    {
        instance.selectedCharacter = character;
        character.EditTower(tower);
        character.StarterWeapon.EditTower(tower);

        weapons.Add(character.StarterWeapon);

        InGame game = InGame.instance;
        RectTransform rect = game.uiRect;
        hasSelectedCharacter = true;
        MenuUi.CreateXpBar(rect, instance.tower);
        projectileBonus = character.ProjectileBonus;
        projectileSizeBonus = character.ProjectileSizeBonus;
        XPBonus = character.XPBonus;
        damageBonus = character.DamageBonus;
        pierceBonus = character.PierceBonus;
        attackSpeedBonus = character.AttackSpeedBonus;
        rangeBonus = character.RangeBonus;
    }
    public void WeaponSelected(WeaponTemplate weapon, Tower tower)
    {
        weapon.EditTower(tower);
        weapons.Add(weapon);
        selectingCharacter = false;
    }
    public void EvolutionSelected(WeaponTemplate weapon, WeaponTemplate requiredWeapon, Tower tower)
    {
        var weaponGot = weapon;
        foreach (var weap in instance.weapons.ToList())
        {
            if (weap.WeaponName == requiredWeapon.WeaponName)
            {
              
                weaponGot = weap;
                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
                var attack = towerModel.GetAttackModel(weap.WeaponName);
                if (attack != null)
                {
                    towerModel.RemoveBehavior(attack);
                }
                tower.UpdateRootModel(towerModel);
                weapon.EditTower(tower);
                weapons.Add(weapon);
            }
        }
        instance.weapons.Remove(weaponGot);  
       
        selectingCharacter = false;
    }
    public void WeaponUpgrade(WeaponTemplate weapon, Tower tower)
    {
        weapon.level += 1;
        selectingCharacter = false;
    }
    public void ItemSelected(ItemTemplate item, Tower tower)
    {
        item.level = 1;
        items.Add(item);
        var stat = item.UpgradeType;
        if (stat == UpgradeType.Damage)
        {
            damageBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.Pierce)
        {
            pierceBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.Range)
        {
            rangeBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.ProjectileSize)
        {
            projectileSizeBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.Projectiles)
        {
            projectileBonus += (int)item.Increase ;
        }
        if (stat == UpgradeType.AttackSpeed)
        {
            attackSpeedBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.Xp)
        {
            XPBonus += (item.Increase - 1);
        }
        selectingCharacter = false;
    }
    public void ItemUpgrade(ItemTemplate item, Tower tower)
    {
        item.level += 1;
        var stat = item.UpgradeType;
        if (stat == UpgradeType.Damage)
        {
            damageBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.Pierce)
        {
            pierceBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.Range)
        {
            rangeBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.ProjectileSize)
        {
            projectileSizeBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.Projectiles)
        {
            projectileBonus += (int)item.Increase;
        }
        if (stat == UpgradeType.AttackSpeed)
        {
            attackSpeedBonus += (item.Increase - 1);
        }
        if (stat == UpgradeType.Xp)
        {
            XPBonus += (item.Increase - 1);
        }
        selectingCharacter = false;
    }
    [RegisterTypeInIl2Cpp(false)]
    public class MenuUi : MonoBehaviour
    {
        public static MenuUi ui;
        public static ModHelperPanel XpBar;

        public ModHelperInputField input;
        public static ModHelperPanel CreateCharacter(CharacterTemplate character, Tower tower)
        {
            var panel = ModHelperPanel.Create(new Info("characterTemplate " + character.CharacterName, 0, 0, 850, 1800), VanillaSprites.BlueInsertPanel);
            var image = panel.AddImage(new Info("image", 0, -50, 425, 425), character.CharacterImage);

            var name = panel.AddText(new Info("ChallengeName", 0, 860, 1500, 80), character.CharacterName, 80, TextAlignmentOptions.Center);

            var rangeImg = panel.AddImage(new Info("rangeImg", -350, 750, 60, 60), VanillaSprites.MapBuffIconRange);
            var rangeText = panel.AddText(new Info("rangeText", 470, 750, 1500, 60), character.RangeBonus + "X Range", 60, TextAlignmentOptions.MidlineLeft);

            var pierceImg = panel.AddImage(new Info("pierceImg", -350, 680, 60, 60), VanillaSprites.MapBuffIconPierce);
            var pierceText = panel.AddText(new Info("pierceText", 470, 680, 1500, 60), character.PierceBonus + "X Pierce", 60, TextAlignmentOptions.MidlineLeft);

            var damageImg = panel.AddImage(new Info("damageImg", -350, 610, 60, 60), VanillaSprites.MapBuffIconDamage);
            var damageText = panel.AddText(new Info("damageText", 470, 610, 1500, 60), character.DamageBonus + "X Damage", 60, TextAlignmentOptions.MidlineLeft);

            var attackSpeedImg = panel.AddImage(new Info("attackSpeedImg", -350, 540, 60, 60), VanillaSprites.MapBuffIconVillage2xx);
            var attackSpeedText= panel.AddText(new Info("attackSpeedText", 470, 540, 1500, 60), character.AttackSpeedBonus + "X Attack Speed", 60, TextAlignmentOptions.MidlineLeft);

            var projectileImg = panel.AddImage(new Info("projectileImg", -350, 470, 60, 60), VanillaSprites.TripleShotUpgradeIcon);
            var projectileText = panel.AddText(new Info("projectileText", 470, 470, 1500, 60), "+" + character.ProjectileBonus + " Projectiles", 60, TextAlignmentOptions.MidlineLeft);

            var projectileSizeImg = panel.AddImage(new Info("projectileSizeImg", -350, 400, 60, 60), VanillaSprites.BiggerBombsUpgradeIcon);
            var projectileSizeText = panel.AddText(new Info("projectileSizeText", 470, 400, 1500, 60), character.ProjectileSizeBonus + "X Projectiles Size", 60, TextAlignmentOptions.MidlineLeft);

            var XPImg = panel.AddImage(new Info("XPImg", -350, 330, 60, 60), VanillaSprites.ThriveIcon);
            var XPText = panel.AddText(new Info("XPText", 470, 330, 1500, 60), character.XPBonus + "X XP Bonus", 60, TextAlignmentOptions.MidlineLeft);

            var description = panel.AddText(new Info("description", 0, -512, 850, 300), character.CharacterDescription, 60, TextAlignmentOptions.Center);

            var select = panel.AddButton(new Info("select", -100, -820, 500, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
            {
                VampireMonkey.instance.CharacterSelected(character, tower);
                MenuUi.ui.CloseMenu();
            }));
            var selectText = select.AddText(new Info("selectText", 0, 0, 700, 160), "Select", 80);

            var weaponPanel = panel.AddPanel(new Info("weaponPanel", 275, -820, 125, 125), VanillaSprites.GreyInsertPanel);
            var weaponImg = weaponPanel.AddImage(new Info("weaponImg", 0, 0, 125, 125), character.StarterWeapon.WeaponIcon);

            return panel;
        }
        public static void LoadCharactersPanel(ModHelperScrollPanel panel, Tower tower)
        {
            panel.ScrollContent.transform.DestroyAllChildren();
            foreach (var character in ModContent.GetContent<CharacterTemplate>())
            {
                panel.AddScrollContent(CreateCharacter(character, tower));
            }
        }
        public void CloseMenu()
        {
            Destroy(gameObject);
        }
        public static void CreateCharacterMenu(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("scrollPanel", 1100, 800, 2700, 1850, new UnityEngine.Vector2()),VanillaSprites.Black);
            MenuUi characterUI = panel.AddComponent<MenuUi>();
            ui = characterUI;

            ModHelperScrollPanel scrollPanel = panel.AddScrollPanel(new Info("scrollPanel", 2200, 1300, 2700, 1850, new UnityEngine.Vector2()), RectTransform.Axis.Horizontal, VanillaSprites.BrownInsertPanel, 50, 50);

            var infoPanel = panel.AddPanel(new Info("infoPanel", 2200, 2400, 2700, 200, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            var infoText = infoPanel.AddText(new Info("infoText", 0, 0, 2700, 140), "Select A Character", 140);
            LoadCharactersPanel(scrollPanel, tower);
        }
        public static void CreateXpBar(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("panel", 200, 1225, 200, 2400, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            MenuUi characterUI = panel.AddComponent<MenuUi>();

            var percent = instance.Xp * 100 / instance.XpMax;
            var size = 2370 * percent / 100;
            ModHelperPanel xpbar = panel.AddPanel(new Info("Panel",0, ( size - size / 2) - 1185, 160, size), VanillaSprites.MainBGPanelBlue);
            XpBar = xpbar;

        }
        public static void CreateLevelUpMenu(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("panel", 2200, 1500, 2500, 1850, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            MenuUi characterUI = panel.AddComponent<MenuUi>();
            ui = characterUI;
            ModHelperText levelUpReward = panel.AddText(new Info("levelUpReward", 0, 800, 2500, 180), "Level Up Reward", 100);
            Il2CppSystem.Random rnd = new Il2CppSystem.Random();
            float x = 412.5f;

            List<WeaponTemplate> choosableWeapon = new List<WeaponTemplate>();

            foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
            {
                var hasWeapon = false;
                foreach (var ownedWeapon in instance.weapons.ToList())
                {
                    if (weapon.WeaponName == ownedWeapon.WeaponName)
                    {
                        hasWeapon = true;
                    }
                }
                if (weapon.canBeSelected == false)
                {
                    hasWeapon = true;
                }
                if (hasWeapon == false)
                {
                    choosableWeapon.Add(weapon);
                }
            }
            

            List<WeaponTemplate> canUpgradeWeapon = new List<WeaponTemplate>();
            foreach (var ownedWeapon in instance.weapons.ToList())
            {
                if (ownedWeapon.level < ownedWeapon.MaxLevel)
                {
                    canUpgradeWeapon.Add(ownedWeapon);
                }

            }
            List<ItemTemplate> choosableItem = new List<ItemTemplate>();

            foreach (var item in ModContent.GetContent<ItemTemplate>())
            {
                var hasItem = false;
                foreach (var ownedItem in instance.items.ToList())
                {
                    if (item.ItemName == ownedItem.ItemName)
                    {
                        hasItem = true;
                    }
                }
                if (hasItem == false)
                {
                    choosableItem.Add(item);
                }
            }
            if (instance.items.Count >= instance.itemLimit)
            {
                choosableItem.Clear();
            }
            if (instance.weapons.Count >= instance.weaponLimit)
            {
                choosableWeapon.Clear();
            }
            List<ItemTemplate> canUpgradeItem = new List<ItemTemplate>();
            foreach (var ownedItem in instance.items.ToList())
            {
                if (ownedItem.level < ownedItem.MaxLevel)
                {
                    canUpgradeItem.Add(ownedItem);
                }
            }
            List<EvolutionTemplate> evolutions = new List<EvolutionTemplate>();
            foreach (var evolution in ModContent.GetContent<EvolutionTemplate>())
            {
                var hasWeapon = false;
                var hasItem = false;
                foreach (var weapon in instance.weapons.ToList())
                {
                    if (weapon.WeaponName == evolution.WeaponRequired.WeaponName)
                    {
                        if (weapon.level == weapon.MaxLevel)
                        {
                            hasWeapon = true;
                        }
                    }
                }
                foreach (var item in instance.items.ToList())
                {
                    if (item.ItemName == evolution.ItemRequired.ItemName)
                    {
                        if (item.level == item.MaxLevel)
                        {
                            hasItem = true;
                        }
                    }
                }
                if (hasItem && hasWeapon)
                {
                    evolutions.Add(evolution);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                var Selected = false;
                var action = rnd.Next(1, 5);
                if (evolutions.Count >= 1 )
                {
                    Selected = true;
                    var num = rnd.Next(0, evolutions.Count);
                    var evolution = evolutions[num];
                    var weapon1 = evolution.EvolutionWeapon;
                    var weapon2 = evolution.WeaponRequired;
                    ModHelperPanel evolutionPanel = panel.AddPanel(new Info("evolutionPanel", x, 800, 650, 1450, new UnityEngine.Vector2()), VanillaSprites.MainBgPanelHematite);
                    ModHelperText evoName = evolutionPanel.AddText(new Info("evoName", 0, 650, 800, 180), evolution.EvolutionWeapon.WeaponName, 80);
                    ModHelperImage image = evolutionPanel.AddImage(new Info("image", 0, 100, 400, 400), evolution.EvolutionWeapon.WeaponIcon);
                    ModHelperButton selectEvolution = evolutionPanel.AddButton(new Info("selectEvolution", 0, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() =>
                    {
                        foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
                        {
                            if (weapon.WeaponName == evolution.WeaponRequired.WeaponName)
                            {
                                weapon.canBeSelected = false;
                            }
                        }
                        instance.EvolutionSelected(weapon1,weapon2, tower);
                        MenuUi.ui.CloseMenu(); 
                        UpdateXp(rect, tower);
                    }));
                    ModHelperText selectEvoText = selectEvolution.AddText(new Info("selectEvoText", 0, 0, 700, 160), "Select", 70);
                    ModHelperText evoDesc = evolutionPanel.AddText(new Info("evoDesc", 0, -175, 800, 180), evolution.WeaponRequired.WeaponName + "'S Evolution", 70);

                    ModHelperText itemRequired = evolutionPanel.AddText(new Info("itemRequired", 0, -325, 800, 180), "Item Required: " + evolution.ItemRequired.Name, 60);

                }
                else
                {
                    if (action == 1)
                    {
                        if (choosableWeapon.Count > 0)
                        {
                            Selected = true;
                            var num = rnd.Next(0, choosableWeapon.Count);
                            var weapon = choosableWeapon[num];
                            ModHelperPanel weaponPanel = panel.AddPanel(new Info("weaponPanel", x, 800, 650, 1450, new UnityEngine.Vector2()), VanillaSprites.GreyInsertPanel);
                            ModHelperText weaponName = weaponPanel.AddText(new Info("weaponName", 0, 650, 800, 180), weapon.WeaponName, 80);
                            ModHelperImage image = weaponPanel.AddImage(new Info("image", 0, 0, 400, 400), weapon.WeaponIcon);
                            ModHelperButton selectWeapon = weaponPanel.AddButton(new Info("selectWeapon", 0, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() =>
                            {
                                instance.WeaponSelected(weapon, tower);
                                MenuUi.ui.CloseMenu();
                                UpdateXp(rect, tower);
                            }));
                            ModHelperText selectWeaponText = selectWeapon.AddText(new Info("selectWeaponText", 0, 0, 700, 160), "Select", 70);
                        }
                        else
                        {
                            action = rnd.Next(2, 5);
                        }
                    }

                    if (action == 2)
                    {
                        if (canUpgradeWeapon.Count > 0)
                        {
                            Selected = true;
                            var num = rnd.Next(0, canUpgradeWeapon.Count);
                            var weapon = canUpgradeWeapon[num];
                            var desc = "";
                            ModHelperPanel weaponPanel = panel.AddPanel(new Info("weaponPanel", x, 800, 650, 1450, new UnityEngine.Vector2()), VanillaSprites.BlueInsertPanel);
                            ModHelperText weaponName = weaponPanel.AddText(new Info("weaponName", 0, 650, 800, 180), weapon.WeaponName, 80);
                            ModHelperImage image = weaponPanel.AddImage(new Info("image", 0, 150, 400, 400), weapon.WeaponIcon);
                            ModHelperButton selectWeapon = weaponPanel.AddButton(new Info("selectWeapon", 0, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() =>
                            {
                                instance.WeaponUpgrade(weapon, tower);
                                MenuUi.ui.CloseMenu();
                                
                                UpdateXp(rect, tower);
                            }));
                            ModHelperText weaponDesc = weaponPanel.AddText(new Info("weaponDesc", 0, -150, 800, 180), "Level: " + weapon.level + " -> " + (weapon.level + 1) + "(Max " + weapon.MaxLevel + ")", 60);
                            for (var y = 0; y < weapon.Upgrades[weapon.level].Count(); y++)
                            {
                                if (weapon.Upgrades[weapon.level][y] == VampireMonkey.UpgradeType.Projectiles)
                                {
                                    desc = desc + "+" + weapon.Increase[weapon.level][y] + " Projectiles\n";
                                }
                                else if (weapon.Upgrades[weapon.level][y] == VampireMonkey.UpgradeType.MIB)
                                {
                                    desc = desc + "+MIB\n";
                                }
                                else
                                {
                                    desc = desc + "+" + Mathf.Round((weapon.Increase[weapon.level][y] - 1) * 100) + "% " + weapon.Upgrades[weapon.level][y] + "\n";
                                }
                            }

                            ModHelperText weaponStat = weaponPanel.AddText(new Info("weaponStat", 0, -350, 800, 250), desc, 60, TextAlignmentOptions.Top);

                            ModHelperText selectWeaponText = selectWeapon.AddText(new Info("selectWeaponText", 0, 0, 700, 160), "Upgrade", 70);
                        }
                        else
                        {
                            action = rnd.Next(3, 5);
                        }

                    }
                    if (action == 3)
                    {
                        if (choosableItem.Count > 0)
                        {
                            Selected = true;
                            var num = rnd.Next(0, choosableItem.Count);
                            var item = choosableItem[num];
                            ModHelperPanel itemPanel = panel.AddPanel(new Info("itemPanel", x, 800, 650, 1450, new UnityEngine.Vector2()), VanillaSprites.GreyInsertPanel);
                            ModHelperText itemName = itemPanel.AddText(new Info("itemName", 0, 650, 800, 180), item.ItemName, 80);
                            ModHelperImage image = itemPanel.AddImage(new Info("image", 0, 0, 400, 400), item.ItemIcon);
                            ModHelperButton selectItem = itemPanel.AddButton(new Info("selectItem", 0, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() =>
                            {
                                instance.ItemSelected(item, tower);
                                MenuUi.ui.CloseMenu();
                                UpdateXp(rect, tower);
                            }));
                            ModHelperText selectItemText = selectItem.AddText(new Info("selectItemText", 0, 0, 700, 160), "Select", 70);
                            if (item.UpgradeType == UpgradeType.Projectiles)
                            {
                                ModHelperText itemStat = itemPanel.AddText(new Info("itemStat", 0, -250, 800, 180), "+" + item.Increase + "(All) " + item.UpgradeType, 60);
                            }
                            else
                            {
                                ModHelperText itemStat = itemPanel.AddText(new Info("itemStat", 0, -250, 800, 180), "+" + Mathf.Round((item.Increase - 1) * 100) + "%(All) " + item.UpgradeType, 60);
                            }

                        }
                        else
                        {
                            action = 4;
                        }
                    }
                    if (action == 4)
                    {
                        if (canUpgradeItem.Count > 0)
                        {
                            Selected = true;
                            var num = rnd.Next(0, canUpgradeItem.Count);
                            var item = canUpgradeItem[num];
                            ModHelperPanel itemPanel = panel.AddPanel(new Info("itemPanel", x, 800, 650, 1450, new UnityEngine.Vector2()), VanillaSprites.BlueInsertPanel);
                            ModHelperText itemName = itemPanel.AddText(new Info("itemName", 0, 650, 800, 180), item.ItemName, 80);
                            ModHelperImage image = itemPanel.AddImage(new Info("image", 0, 150, 400, 400), item.ItemIcon);
                            ModHelperButton selectItem = itemPanel.AddButton(new Info("selectItem", 0, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() =>
                            {
                                instance.ItemUpgrade(item, tower);
                                MenuUi.ui.CloseMenu();
                                UpdateXp(rect, tower);
                            }));
                            ModHelperText itemDesc = itemPanel.AddText(new Info("itemDesc", 0, -150, 800, 180), "Level: " + item.level + " -> " + (item.level + 1) + "(Max " + item.MaxLevel + ")", 60);
                            if (item.UpgradeType == UpgradeType.Projectiles)
                            {
                                ModHelperText itemStat = itemPanel.AddText(new Info("itemStat", 0, -250, 800, 180), "+" + item.Increase + " Projectile", 60);
                            }
                            else
                            {
                                ModHelperText itemStat = itemPanel.AddText(new Info("itemStat", 0, -250, 800, 180), "+" + Mathf.Round((item.Increase - 1) * 100) + "% " + item.UpgradeType, 60);
                            }

                            ModHelperText selectItemText = selectItem.AddText(new Info("selectItemText", 0, 0, 700, 160), "Upgrade", 70);
                        }
                        else
                        {
                            action = 3;
                        }

                    }

                    if (action == 3 && Selected == false)
                    {
                        if (choosableItem.Count > 0)
                        {
                            Selected = true;
                            var num = rnd.Next(0, choosableItem.Count);
                            var item = choosableItem[num];
                            ModHelperPanel itemPanel = panel.AddPanel(new Info("itemPanel", x, 800, 650, 1450, new UnityEngine.Vector2()), VanillaSprites.GreyInsertPanel);
                            ModHelperText itemName = itemPanel.AddText(new Info("itemName", 0, 650, 800, 180), item.ItemName, 80);
                            ModHelperImage image = itemPanel.AddImage(new Info("image", 0, 0, 400, 400), item.ItemIcon);
                            ModHelperButton selectItem = itemPanel.AddButton(new Info("selectItem", 0, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() =>
                            {
                                instance.ItemSelected(item, tower);
                                MenuUi.ui.CloseMenu();
                                UpdateXp(rect, tower);
                            }));
                            ModHelperText selectItemText = selectItem.AddText(new Info("selectItemText", 0, 0, 700, 160), "Select", 70);
                            if (item.UpgradeType == UpgradeType.Projectiles)
                            {
                                ModHelperText itemStat = itemPanel.AddText(new Info("itemStat", 0, -250, 800, 180), "+" + item.Increase + "(All) " + item.UpgradeType, 60);
                            }
                            else
                            {
                                ModHelperText itemStat = itemPanel.AddText(new Info("itemStat", 0, -250, 800, 180), "+" + Mathf.Round((item.Increase - 1) * 100) + "%(All) " + item.UpgradeType, 60);
                            }
                        }
                        else
                        {
                            action = 2;
                        }
                    }
                    if (action == 2 && Selected == false)
                    {
                        if (canUpgradeWeapon.Count > 0)
                        {
                            Selected = true;
                            var num = rnd.Next(0, canUpgradeWeapon.Count);
                            var weapon = canUpgradeWeapon[num];
                            var desc = "";
                            ModHelperPanel weaponPanel = panel.AddPanel(new Info("weaponPanel", x, 800, 650, 1450, new UnityEngine.Vector2()), VanillaSprites.BlueInsertPanel);
                            ModHelperText weaponName = weaponPanel.AddText(new Info("weaponName", 0, 650, 800, 180), weapon.WeaponName, 80);
                            ModHelperImage image = weaponPanel.AddImage(new Info("image", 0, 150, 400, 400), weapon.WeaponIcon);
                            ModHelperButton selectWeapon = weaponPanel.AddButton(new Info("selectWeapon", 0, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() =>
                            {
                                instance.WeaponUpgrade(weapon, tower);
                                MenuUi.ui.CloseMenu();
                                UpdateXp(rect, tower);
                            }));
                            ModHelperText weaponDesc = weaponPanel.AddText(new Info("weaponDesc", 0, -150, 800, 180), "Level: " + weapon.level + " -> " + (weapon.level + 1) + "(Max " + weapon.MaxLevel + ")", 60);
                            for (var y = 0; y < weapon.Upgrades[weapon.level].Count(); y++)
                            {
                                if (weapon.Upgrades[weapon.level][y] == VampireMonkey.UpgradeType.Projectiles)
                                {
                                    desc = desc + "+" + weapon.Increase[weapon.level][y] + " Projectiles\n";
                                }
                                else if (weapon.Upgrades[weapon.level][y] == VampireMonkey.UpgradeType.MIB)
                                {
                                    desc = desc + "+MIB\n";
                                }
                                else
                                {
                                    desc = desc + "+" + Mathf.Round((weapon.Increase[weapon.level][y] - 1) * 100) + "% " + weapon.Upgrades[weapon.level][y] + "\n";
                                }
                            }

                            ModHelperText weaponStat = weaponPanel.AddText(new Info("weaponStat", 0, -350, 800, 250), desc, 60, TextAlignmentOptions.Top);
                            ModHelperText selectWeaponText = selectWeapon.AddText(new Info("selectWeaponText", 0, 0, 700, 160), "Upgrade", 70);
                        }
                        else
                        {
                            action = 1;
                        }

                    }
                    if (action == 1 && Selected == false)
                    {
                        if (choosableWeapon.Count > 0)
                        {
                            Selected = true;
                            var num = rnd.Next(0, choosableWeapon.Count);
                            var weapon = choosableWeapon[num];
                            ModHelperPanel weaponPanel = panel.AddPanel(new Info("weaponPanel", x, 800, 650, 1450, new UnityEngine.Vector2()), VanillaSprites.GreyInsertPanel);
                            ModHelperText weaponName = weaponPanel.AddText(new Info("weaponName", 0, 650, 800, 180), weapon.WeaponName, 80);
                            ModHelperImage image = weaponPanel.AddImage(new Info("image", 0, 0, 400, 400), weapon.WeaponIcon);
                            ModHelperButton selectWeapon = weaponPanel.AddButton(new Info("selectWeapon", 0, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() =>
                            {
                                instance.WeaponSelected(weapon, tower);
                                MenuUi.ui.CloseMenu();
                                UpdateXp(rect, tower);
                            }));
                            ModHelperText selectWeaponText = selectWeapon.AddText(new Info("selectWeaponText", 0, 0, 700, 160), "Select", 70);
                        }
                    }
                }
             
                x += 833.33f;
                if(Selected == false)
                {
                    instance.selectingCharacter = false;
                    MenuUi.ui.CloseMenu();
                }
            }
            x = 160;
            ModHelperPanel ownedWeaponPanel = panel.AddPanel(new Info("ownedWeaponPanel", 1250, -150, 2500, 200, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            for (int i = 0; i < instance.weapons.Count; i++)
            {
                WeaponTemplate weapon = instance.weapons[i];
                ModHelperPanel ownedWeaponEmpty = panel.AddPanel(new Info("ownedWeaponEmpty", x, -150, 170, 170, new UnityEngine.Vector2()), VanillaSprites.GreyInsertPanel);
                ModHelperImage image = ownedWeaponEmpty.AddImage(new Info("image", 0, 0, 170, 170), weapon.WeaponIcon);
                x += 435f;
            }
            for (int i = 0; i < instance.weaponLimit - instance.weapons.Count; i++)
            {
                ModHelperPanel ownedWeaponEmpty = panel.AddPanel(new Info("ownedWeaponEmpty", x, -150, 170, 170, new UnityEngine.Vector2()), VanillaSprites.GreyInsertPanel);
                x += 435f;
            }
            x = 160;
            ModHelperPanel ownedItemPanel = panel.AddPanel(new Info("ownedItemPanel", 1250, -425, 2500, 200, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            for (int i = 0; i < instance.items.Count; i++)
            {
                ItemTemplate item = instance.items[i];
                ModHelperPanel ownedItemEmpty = panel.AddPanel(new Info("ownedItemEmpty", x, -425, 170, 170, new UnityEngine.Vector2()), VanillaSprites.GreyInsertPanel);
                ModHelperImage image = ownedItemEmpty.AddImage(new Info("image", 0, 0, 170, 170), item.ItemIcon);
                x += 435f;
            }
            for (int i = 0; i < instance.itemLimit - instance.items.Count; i++)
            {
                ModHelperPanel ownedItemEmpty = panel.AddPanel(new Info("ownedItemEmpty", x, -425, 170, 170, new UnityEngine.Vector2()), VanillaSprites.GreyInsertPanel);
                x += 435f;
            }
            instance.level += 1;
            instance.selectedCharacter.LevelUP(instance.level);
        }
        public static void UpdateXp(RectTransform rect, Tower tower)
        {
            var percent = instance.Xp * 100 / instance.XpMax;
            var size = 2370 * percent / 100;
            if (size > 2370)
            {
                size = 2370;
            }
            XpBar.SetInfo(new Info("Panel", 0, (size - size / 2) - 1185, 160, size));
         
            if (instance.Xp >= instance.XpMax && !instance.selectingCharacter)
            {
                Thread.Sleep(50);
                instance.selectingCharacter = true;
                CreateLevelUpMenu(rect, tower);
                instance.Xp -= instance.XpMax;
                instance.XpMax += (5 + instance.XpMax / 10f);
            }
           
        }
    }

    [HarmonyPatch(typeof(Bloon), nameof(Bloon.Damage))]
    internal static class Bloon_Damage
    {
        [HarmonyPrefix]
        private static void Prefix(Bloon __instance, float totalAmount, Projectile projectile, bool distributeToChildren, bool overrideDistributeBlocker, bool createEffect, Tower tower, BloonProperties immuneBloonProperties = BloonProperties.None, bool canDestroyProjectile = true, bool ignoreNonTargetable = false, bool blockSpawnChildren = false, bool ignoreInvunerable = false)
        {
            if (__instance != null && tower != null && tower.towerModel.name.Contains("VampireMonkey") && __instance.bloonModel.IsRegrowBloon() == false)
            {
                instance.Xp += 0.25f * totalAmount * instance.XPBonus;
                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                MenuUi.UpdateXp(rect, tower);
            }
        }
    }

}