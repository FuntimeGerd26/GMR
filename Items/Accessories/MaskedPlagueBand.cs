using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Accessories
{
	public class MaskedPlagueBand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Band");
			Tooltip.SetDefault("Gives the player the 'Plague Regen' buff\nIncreases magic and melee damage and attack speed by 4%\nIncreases max mana by 30\n'Someone's gift is now yours'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 58);
			Item.rare = 3;
			Item.accessory = true;
			Item.defense = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.AddBuff(ModContent.BuffType<Buffs.Buff.PlagueRegen>(), 2);
			player.GetDamage(DamageClass.Magic) += 0.04f;
			player.GetDamage(DamageClass.Melee) += 0.04f;
			player.GetAttackSpeed(DamageClass.Magic) += 0.04f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.04f;
			player.statManaMax2 += 30;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BandofStarpower);
			recipe.AddIngredient(ItemID.Shackle);
			recipe.AddRecipeGroup("GMR:AnyGem", 5);
			recipe.AddIngredient(null, "UpgradeCrystal", 40);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}