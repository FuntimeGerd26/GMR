using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class InfraRedGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Greaves");
			Tooltip.SetDefault("Increases movement speed and jump speed by 7%\nIncreases max mana by 60\nIncreases attack speed by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 5;
			Item.value = Item.sellPrice(silver: 175);
			Item.defense = 14;
		}

		public override void UpdateEquip(Player player)
		{
			if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.JackyMaskOn>()))
			{
				player.moveSpeed += 0.5f;
				player.jumpSpeedBoost += 0.07f;
				player.statManaMax2 += 60;
				player.GetAttackSpeed(DamageClass.Generic) += 0.03f;
			}
			else
			{
				Item.defense = 0;
				Item.vanity = true;
			}
		}
	}
}
