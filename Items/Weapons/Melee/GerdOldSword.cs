using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class GerdOldSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Training Sword");
			Tooltip.SetDefault("Having this in your inventory increases melee speed and all damage by 3%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(-1);
		}

		public override void SetDefaults()
		{
			Item.width = 70;
			Item.height = 70;
			Item.rare = 1;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 16;
			Item.crit = 0;
			Item.knockBack = 5f;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.02f;
			player.GetDamage(DamageClass.Melee) += 0.02f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdDagger");
			recipe.AddIngredient(ItemID.GoldBar, 12);
			recipe.AddIngredient(ItemID.Amber, 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "GerdDagger");
			recipe2.AddIngredient(ItemID.PlatinumBar, 12);
			recipe2.AddIngredient(ItemID.Amber, 7);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}