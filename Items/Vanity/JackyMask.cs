using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR.Items.Armor; //Allows to get files from the 'Armor' folder

namespace GMR.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class JackyMask : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shape Shifter Mask");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 75);
			Item.vanity = true;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<InfraRedPlating>() && legs.type == ModContent.ItemType<InfraRedGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"[i:{ModContent.ItemType<UI.ItemEffectBoosted>()}] Your armor has no effects now";
			player.AddBuff(ModContent.BuffType<Buffs.Debuffs.JackyMaskOn>(), 2);

		}
	}
}
