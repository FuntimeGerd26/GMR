using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class AluminiumBreastplate : ModItem
	{
		public override void Load()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			Player player = Main.player[0];
			if (!player.Male)
			{
				EquipLoader.AddEquipTexture(Mod, "AluminiumLeggings_LegsFemale", EquipType.Legs, this);
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aluminium Breastplate");
			Tooltip.SetDefault("Increases weapon speed by 4%\nIncreases all weapon damage by 2%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 30);
			Item.maxStack = 1;
			Item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.04f;
			player.GetDamage(DamageClass.Generic) += 0.2f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 10);
			recipe.AddIngredient(ItemID.Silk, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 10);
			recipe2.AddIngredient(ItemID.Silk, 4);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}

		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			Player player = Main.player[0];
			if (!player.Male)
			{
				robes = true;
				equipSlot = EquipLoader.GetEquipSlot(Mod, "AluminiumLeggings_LegsFemale", EquipType.Legs);
			}
		}
	}
}
