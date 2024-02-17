using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using GMR.Items.Weapons.Melee;
using GMR.Items.Weapons.Ranged;
using GMR.Items.Weapons.Magic;
using GMR.Items.Accessories;

namespace GMR.Items.Misc.Consumable
{
	public class MagmaEyeTreasureBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.rare = 12;
			Item.maxStack = 9999;
			Item.expert = true;
			Item.consumable = true;
			Item.value = Item.sellPrice(silver: 100);
			Item.buyPrice(silver: 500);
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ItemID.Hellstone, 1, 20, 38));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExolBlade>(), 10));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DGPCrate>(), 20));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<IllusionOfLove>(), 20));

			int[] drops = { ModContent.ItemType<BrokenMagmaticSword>(), ModContent.ItemType<Magmathrower>(), ModContent.ItemType<MagmaKnife>(), ModContent.ItemType<MagmaStaff>() };
			itemLoot.Add(ItemDropRule.OneFromOptions(1, drops));

			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.BossUpgradeCrystal>()));
		}
	}
}