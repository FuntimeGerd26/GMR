using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ArerniasVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 170);
			Item.rare = 7;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.endurance += 0.02f;
			player.GetDamage(DamageClass.Generic) += 0.07f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ArerniasVest>() && legs.type == ModContent.ItemType<ArerniasLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases all damage by 7%\nIncreases damage reduction by 2%";
			player.endurance += 0.02f;
			player.GetDamage(DamageClass.Generic) += 0.07f;
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class ArerniasVest : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 170);
			Item.defense = 14;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.14f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.07f;
			player.manaCost -= 0.25f;
			player.statLifeMax2 += (int)(player.statLifeMax * 0.07f);
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class ArerniasLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 170);
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.1f;
			player.runAcceleration += 0.05f;
			player.maxRunSpeed += 0.25f;
		}
	}
}