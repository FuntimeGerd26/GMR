using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class InfraRedPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Chestplate");
			Tooltip.SetDefault("Increases all damage by 6%\nIncreases critical strike chance by 4\nIncreases max summons and sentries by 2");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 215);
			Item.rare = 4;
			Item.defense = 17;
		}

		public override void UpdateEquip(Player player)
        {
            if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.JackyMaskOn>()))
            {
                player.GetDamage(DamageClass.Generic) += 0.06f;
                player.GetCritChance(DamageClass.Generic) += 4f;
                player.maxMinions += 2;
                player.maxTurrets += 2;
            }
			else
            {
				Item.defense = 0;
				Item.vanity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 14);
			recipe.AddIngredient(null, "InfraRedBar", 18);
			recipe.AddIngredient(null, "ScrapFragment", 22);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(null, "UpgradeCrystal", 65);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}