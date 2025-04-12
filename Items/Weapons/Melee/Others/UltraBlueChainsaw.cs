using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Others
{
	public class UltraBlueChainsaw : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra-Blue Chainsaw");
			Tooltip.SetDefault($"'I got nothing man'\nShoots a sword that slows down until stop\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //Count of items to research

			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 2));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			Item.AddElement(2);
		}

        public override void SetDefaults()
        {
            Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.UltraBlueChainsaw>(30);
            Item.useTime /= 2;
            Item.SetWeaponValues(50, 2.5f, 4);
            Item.width = 76;
            Item.height = 74;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(silver: 150);
            Item.rare = 4;
            Item.autoReuse = true;
            Item.reuseDelay = 2;
        }

		public override bool MeleePrefix()
        {
            return true;
        }

        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AdamantiteBar, 22);
			recipe.AddIngredient(ItemID.MythrilBar, 16);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TitaniumBar, 22);
			recipe2.AddIngredient(ItemID.OrichalcumBar, 16);
			recipe2.AddIngredient(ItemID.SoulofNight, 8);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.AdamantiteBar, 22);
			recipe3.AddIngredient(ItemID.OrichalcumBar, 16);
			recipe3.AddIngredient(ItemID.SoulofNight, 8);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.TitaniumBar, 22);
			recipe4.AddIngredient(ItemID.MythrilBar, 16);
			recipe4.AddIngredient(ItemID.SoulofNight, 8);
			recipe4.AddTile(TileID.MythrilAnvil);
			recipe4.Register();
		}
	}
}