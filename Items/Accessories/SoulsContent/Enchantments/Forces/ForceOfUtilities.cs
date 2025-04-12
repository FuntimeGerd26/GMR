using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Accessories.SoulsContent.Enchantments.Forces
{
    public class ForceOfUtilities : ModItem
    {
        public static int[] Enchants => new int[]
   {
            ModContent.ItemType<MagnumEnchantment>(),
            ModContent.ItemType<MaskedPlagueEnchantment>(),
            ModContent.ItemType<AmethystGolemEnchantment>(),
            ModContent.ItemType<AluminiumEnchantment>(),
            ModContent.ItemType<ArmoredBullEnchantment>()
   };

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault($"Increases damage, knockback, armor penetration, attack speed, max health and movement speed by 15%\nIncreases damage reduction by 5% and max minions by 2" +
                $"\nIncreases the speed of ranged velocity and ranged weapons have a chance to shoot a rocket" +
                $"\nUsing Magic or Summon weapons will shoot a homing projectile" +
                $"\nWhen damaged you release crystal shards" +
                $"\nRanged weapons will shoot a projectile that will home into enemies and all weapons have a chance to shoot an Aluminium Shuriken" +
                "\nBuffs the Zombie Breaker greatly, increases the damage of ALL projectile shooting weapons by 5%" +
                "\n'A handy tool that will help us later'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.rare = 11;
            Item.value = Item.sellPrice(silver: 340);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.15f;
            player.GetKnockback(DamageClass.Generic) += 0.15f;
            player.GetArmorPenetration(DamageClass.Generic) += 0.15f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
            player.endurance += 0.05f;
            player.moveSpeed += 0.15f;
            player.statLifeMax2 += player.statLifeMax / 10 + player.statLifeMax / 20;
            player.maxMinions += 2;

            // Magnum
            player.GPlayer().MagnumSet = true;
            if (player.GPlayer().EnchantToggles["ArmRocket"])
                player.GPlayer().ChargedArm = Item;

            // Masked Plague
            if (player.GPlayer().EnchantToggles["MaskedPlagueCloak"])
                player.GPlayer().MaskedPlagueCloak = Item;

            // Amethyst Golem
            player.GPlayer().AmethystSet = true;

            // Aluminium
            if (player.GPlayer().EnchantToggles["AluminiumShuriken"])
            {
                player.GPlayer().AlumArmor = Item;
                player.GPlayer().AluminiumEnch = Item;
            }

            // Armored Bull
            player.GPlayer().BullSet = true;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            foreach (int ench in Enchants)
                recipe.AddIngredient(ench);

            recipe.AddTile(TileID.LunarCraftingStation);
            //recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
    }
}