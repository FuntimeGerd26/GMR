using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;

namespace GMR.Items.Misc.Consumable
{
	public class DGPCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Desire Crate");
			Tooltip.SetDefault("'Through careful choosing, you have been chosen to participate in the Desire Grand Prix'\nRight-Click to open");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.rare = 3;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(silver: 120);
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			if (Main.rand.NextBool(3))
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.GerdHead>()));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.GerdBody>()));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.GerdLegs>()));
			}

			if (Main.hardMode)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.BoostFoxMask>()));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.BoostFoxChestplate>()));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.BoostFoxBoots>()));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.BullChainsaw>()));
			}
			else
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.MagnumFoxMask>()));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.MagnumFoxChestplate>()));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.MagnumFoxBoots>()));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.MagnumShooter>()));
			}
		}
	}
}