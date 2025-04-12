using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Accessories.SoulsContent.Enchantments
{
    public class SandwaveEnchantment : ModItem
    {
        public int Timer;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault($"Every 2 seconds you shoot a knife that homes into enemies" +
                "\nGives the player the 'Sunburnt' and 'Empowered' buffs\nIncreases all damage by 18% and increases damage reduction by 5%" +
                "\n'Glory on it's bare bones'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.DamageType = DamageClass.Generic;
            Item.damage = 40;
            Item.crit = 4;
            Item.knockBack = 4f;
            Item.rare = 7;
            Item.value = Item.sellPrice(silver: 100);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 2);
            player.AddBuff(ModContent.BuffType<Buffs.Buff.Empowered>(), 2);
            player.GetDamage(DamageClass.Generic) += 0.18f;
            player.endurance += 0.05f;

            if (player.GPlayer().EnchantToggles["SandwaveKnife"] && ++Timer % 120 == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.SandwaveKnife>()] < 1)
            {
                for (int i = 0; i < 1; i++)
                {
                    Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, new Vector2(0f, -6f), ModContent.ProjectileType<Projectiles.SandwaveKnife>(), Item.damage, 6f, player.whoAmI);
                }
            }
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "SandwaveHat");
            recipe.AddIngredient(null, "SandwaveVest");
            recipe.AddIngredient(null, "SandwaveLeggings");
            recipe.AddIngredient(null, "DualDesertAxe");
            recipe.AddIngredient(null, "GerdSpear");
            recipe.AddIngredient(null, "DuneSearcher");
            recipe.AddIngredient(null, "HeistGun");
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
    }
}