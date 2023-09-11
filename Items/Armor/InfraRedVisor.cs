using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class InfraRedVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Visor");
			Tooltip.SetDefault("Increases crit chance and attack speed by 2%\nIncreases ranged and magic damage by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 110);
			Item.rare = 4;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.03f;
			player.GetDamage(DamageClass.Ranged) += 0.03f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.02f;
			player.GetCritChance(DamageClass.Generic) += 2f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<InfraRedPlating>() && legs.type == ModContent.ItemType<InfraRedGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases all damage by 4%\n A style of swords can now shoot an extra projectile that goes through walls and deals 50% more damage\nHello World.";
			player.GetDamage(DamageClass.Generic) += 0.04f;
			player.GPlayer().InfraRedSet = Item;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(null, "ScrapFragment", 14);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(null, "UpgradeCrystal", 45);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}