using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class GerdDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Survival Dagger");
			Tooltip.SetDefault("Having this in your inventory increases melee speed by 2%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 42;
			Item.rare = 1;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 50);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 18;
			Item.crit = 4;
			Item.knockBack = 1f;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.02f;
		}


		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 15);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddRecipeGroup("GMR:AnyGem", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Wood, 15);
			recipe2.AddIngredient(ItemID.LeadBar, 2);
			recipe2.AddRecipeGroup("GMR:AnyGem", 4);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}