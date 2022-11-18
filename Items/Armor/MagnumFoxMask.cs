using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MagnumFoxMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnum Mask");
			if (Main.hardMode)
            {
				Tooltip.SetDefault("Increases ranged damage and attack speed by 5%\nIncreased all critical strike chance by 5%\nIncreased melee and ranged armor penetration by 5%");
			}
			else
            {
				Tooltip.SetDefault("Increases all damage by 10%");
			}
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 85);
			Item.rare = 4;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			if (Main.hardMode)
            {
				player.GetDamage(DamageClass.Ranged) += 0.05f;
				player.GetAttackSpeed(DamageClass.Ranged) += 0.05f;
				player.GetCritChance(DamageClass.Generic) += 5;
				player.GetArmorPenetration(DamageClass.Melee) += 5f;
				player.GetArmorPenetration(DamageClass.Ranged) += 5f;
				Item.defense = 10;
			}
			else
            {
				player.GetDamage(DamageClass.Generic) += 10f;
            }				
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<MagnumFoxChestplate>() && legs.type == ModContent.ItemType<MagnumFoxBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases base ranged damage by 8 and endurance by 2%\nIncreases max health by 5%\n[i:{ModContent.ItemType<UI.ItemEffectBoosted>()}] The armor is buffed in hardmode";
			player.endurance += 0.02f;
			player.GetDamage(DamageClass.Ranged).Base += 8f;
			player.statLifeMax2 += player.statLifeMax / 20;
			player.GPlayer().MagnumSet = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ArmorMoldMask");
			recipe.AddIngredient(ItemID.TungstenBar, 25);
			recipe.AddIngredient(ItemID.Goggles);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(ItemID.Ruby, 2);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "ArmorMoldMask");
			recipe2.AddIngredient(ItemID.SilverBar, 25);
			recipe2.AddIngredient(ItemID.Goggles);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe2.AddIngredient(ItemID.Ruby, 2);
			recipe2.AddIngredient(ItemID.Silk, 10);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}