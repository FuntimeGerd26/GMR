using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class SandwaveHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(silver: 125);
			Item.rare = 2;
			Item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Generic) += 6f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<SandwaveVest>() && legs.type == ModContent.ItemType<SandwaveLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases movement speed by 25%\nIncreases crit chance by 6%";
			player.moveSpeed += 0.25f;
			player.GetCritChance(DamageClass.Generic) += 6f;
		}
	}


	[AutoloadEquip(EquipType.Body)]
	public class SandwaveVest : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 135);
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.06f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.06f;
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class SandwaveLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 125);
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.06f;
			player.GetCritChance(DamageClass.Generic) += 6f;
		}
	}
}