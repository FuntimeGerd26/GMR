using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class MagnumFoxChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnum Chestplate");
			if (Main.hardMode)
			{
			Tooltip.SetDefault($"Increases ranged damage and attack speed by 7%\nMelee, magic and summoner damage is decreased by 7%\nIncreased melee and ranged knockback\n[i:{ModContent.ItemType<UI.ItemEffectIcon>()}] Knockback inmunity");
		    }
			else
            {
				Tooltip.SetDefault("Increases all weapon attack speed by 5%\nIncreases all knockback slightly");
			}
	CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 90);
			Item.rare = 4;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			if (Main.hardMode)
            {
				player.GetDamage(DamageClass.Ranged) += 0.07f;
				player.GetDamage(DamageClass.Melee) += -0.07f;
				player.GetDamage(DamageClass.Magic) += -0.07f;
				player.GetDamage(DamageClass.Summon) += -0.07f;
				player.GetAttackSpeed(DamageClass.Ranged) += 0.07f;
				player.GetKnockback(DamageClass.Melee) += 4f;
				player.GetKnockback(DamageClass.Ranged) += 4f;
				player.noKnockback = true;
				Item.defense = 14;
			}
			else
            {
				player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
				player.GetKnockback(DamageClass.Generic) += 2f;
			}				
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ArmorMoldChestplate");
			recipe.AddIngredient(ItemID.TungstenBar, 30);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "ArmorMoldChestplate");
			recipe2.AddIngredient(ItemID.SilverBar, 30);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe2.AddIngredient(ItemID.Ruby, 1);
			recipe2.AddIngredient(ItemID.Silk, 10);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}