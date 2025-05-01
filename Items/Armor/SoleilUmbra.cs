using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SoleilUmbraHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 155);
            Item.rare = 6;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.11f;
            player.GetDamage(DamageClass.Generic) += 0.05f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SoleilUmbraChestplate>() && legs.type == ModContent.ItemType<SoleilUmbraBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = $"Increases all damage by 12%\nIncreases crit chance by 6%\nIncreases damage reduction by 8%\nIncreases knockback mildly";
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.GetCritChance(DamageClass.Generic) += 6f;
            player.GetKnockback(DamageClass.Generic) += 2f;
            player.endurance += 0.08f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "AncientInfraRedPlating", 20);
            recipe.AddIngredient(null, "InfraRedBar", 34);
            recipe.AddIngredient(null, "PrimePlating", 3);
            recipe.AddIngredient(null, "InfraRedCrystalShard", 8);
            recipe.AddIngredient(null, "UpgradeCrystal", 10);
            recipe.AddIngredient(null, "BossUpgradeCrystal", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }


    [AutoloadEquip(EquipType.Head)]
    public class SoleilUmbraHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 16;
            Item.value = Item.sellPrice(silver: 155);
            Item.rare = 6;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
            player.GetDamage(DamageClass.Melee) += 0.15f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SoleilUmbraChestplate>() && legs.type == ModContent.ItemType<SoleilUmbraBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = $"Increases melee attack speed by 12%\nIncreases melee crit chance by 8%\nHighly increases movement speed";
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
            player.GetCritChance(DamageClass.Melee) += 8f;
            player.moveSpeed += 0.5f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "AncientInfraRedPlating", 18);
            recipe.AddIngredient(null, "InfraRedBar", 28);
            recipe.AddIngredient(null, "PrimePlating", 3);
            recipe.AddIngredient(null, "InfraRedCrystalShard", 5);
            recipe.AddIngredient(null, "UpgradeCrystal", 10);
            recipe.AddIngredient(null, "BossUpgradeCrystal", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Body, EquipType.Back)]
    public class SoleilUmbraChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


            int capeSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Back);
            ArmorIDs.Body.Sets.IncludedCapeBack[Item.bodySlot] = capeSlot;
            ArmorIDs.Body.Sets.IncludedCapeBackFemale[Item.bodySlot] = capeSlot;
            ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 22;
            Item.rare = 6;
            Item.value = Item.sellPrice(silver: 175);
            Item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.14f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.07f;
            player.moveSpeed += 1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "AncientInfraRedPlating", 25);
            recipe.AddIngredient(null, "InfraRedBar", 40);
            recipe.AddIngredient(null, "PrimePlating", 3);
            recipe.AddIngredient(null, "InfraRedCrystalShard", 12);
            recipe.AddIngredient(null, "UpgradeCrystal", 10);
            recipe.AddIngredient(null, "BossUpgradeCrystal", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class SoleilUmbraBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 18;
            Item.rare = 6;
            Item.value = Item.sellPrice(silver: 165);
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost *= 1.1f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.18f;
            player.GetAttackSpeed(DamageClass.Generic) -= 0.08f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "AncientInfraRedPlating", 22);
            recipe.AddIngredient(null, "InfraRedBar", 38);
            recipe.AddIngredient(null, "PrimePlating", 3);
            recipe.AddIngredient(null, "InfraRedCrystalShard", 10);
            recipe.AddIngredient(null, "UpgradeCrystal", 10);
            recipe.AddIngredient(null, "BossUpgradeCrystal", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}