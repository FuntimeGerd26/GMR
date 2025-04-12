using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace GMR.Items.Accessories.SoulsContent.Enchantments
{
    public class BoostEnchantment : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault($"Melee weapons that shoot projectiles now shoot an aditional fire ball\nIncreases max mana by 10%" +
                $"\nReduces mana cost by 5%\nGreatly increases movement speed and allows to walk on water and lava\nIncreases attack speed by 15% and increases wing time by 10%" +
                $"\nMakes lightnings fall from the sky when taking damage and using any weapon that's not ranged will shoot a fireball that explodes dealing damage on a large area" +
                "\n[c/FF5555:Boost Time!]");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.rare = 8;
            Item.value = Item.sellPrice(silver: 240);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.manaCost -= 0.05f;
            player.statManaMax2 += player.statManaMax / 10;
            player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
            player.moveSpeed += 0.25f;
            player.wingTimeMax += player.wingTimeMax / 10;
            player.runAcceleration += 0.15f;
            player.maxRunSpeed += 0.75f;
            if (player.GPlayer().EnchantToggles["BoostSet"])
                player.GPlayer().BoostSet = Item;
            player.GPlayer().Thunderblade = Item;
            if (player.GPlayer().EnchantToggles["BoostFireball"])
            {
                player.GPlayer().NajaCharm = Item;
            }
            player.waterWalk = true;
            player.fireWalk = true;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "BoostFoxMask");
            recipe.AddIngredient(null, "BoostFoxChestplate");
            recipe.AddIngredient(null, "BoostFoxBoots");
            recipe.AddIngredient(null, "Heresy");
            recipe.AddIngredient(null, "NajaBladeCharm");
            recipe.AddIngredient(null, "ShiningKuwagata");
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
    }
}