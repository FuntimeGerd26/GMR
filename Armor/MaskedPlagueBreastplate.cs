using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class MaskedPlagueBreastplate : ModItem
	{
		public override void Load()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}
				EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Breastplate");
			Tooltip.SetDefault("Increases magic weapon speed by 5%\nIncreases magic and ranged weapon damage by 5%\nDecreases mana costs by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 22;
			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 60);
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Magic) += 0.05f;
			player.GetDamage(DamageClass.Magic) += 0.5f;
			player.GetDamage(DamageClass.Ranged) += 0.5f;
			player.manaCost -= 0.03f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 15);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddRecipeGroup("GMR:AnyGem", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
		}
	}
}
