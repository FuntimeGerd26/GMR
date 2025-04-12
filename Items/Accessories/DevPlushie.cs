using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Accessories
{
    public class DevPlushie : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 72;
            Item.value = Item.sellPrice(silver: 140);
            Item.rare = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GPlayer().EnchantToggles["MultipleProjectile"])
            {
                player.GPlayer().DevPlush = Item;
            }
            player.GPlayer().DevInmune = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 12);
            recipe.AddIngredient(ItemID.Silk, 14);
            recipe.AddIngredient(ItemID.SoulofSight, 8);
            recipe.AddIngredient(ItemID.CrossNecklace);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}