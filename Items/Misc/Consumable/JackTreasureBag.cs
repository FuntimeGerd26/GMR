using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using GMR.Items.Accessories;

#region Mod Items

using GMR.Items.Misc;
using GMR.Items.Misc.Materials;
using GMR.Items.Misc.Consumable;
using GMR.Items.Weapons.Melee;
using GMR.Items.Weapons.Melee.Swords;
using GMR.Items.Weapons.Melee.Spears;
using GMR.Items.Weapons.Melee.Others;
using GMR.Items.Weapons.Ranged;
using GMR.Items.Weapons.Ranged.Bows;
using GMR.Items.Weapons.Ranged.Guns;
using GMR.Items.Weapons.Ranged.Others;
using GMR.Items.Weapons.Ranged.Railcannons;
using GMR.Items.Weapons.Magic;
using GMR.Items.Weapons.Magic.Books;
using GMR.Items.Weapons.Magic.Staffs;
using GMR.Items.Weapons.Magic.Others;

#endregion

namespace GMR.Items.Misc.Consumable
{
	public class JackTreasureBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		bool isEternity;
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
			if (GMR.Eternity() == true)
				isEternity = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.AncientInfraRedPlating>(), 1, 9, 25));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.InfraRedCrystalShard>(), 2, 4, 12));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientCore>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DGPCrate>(), 4));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<JackRailcannon>(), 10));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<IllusionOfLove>(), 20));

			int[] drops = { ModContent.ItemType<JackSword>(), ModContent.ItemType<AncientHarpoon>(), ModContent.ItemType<AncientRifle>(), };
			itemLoot.Add(ItemDropRule.OneFromOptions(1, drops));

			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.JuiceBox>(), 5, 1, 3));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.BossUpgradeCrystal>(), 1));

			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EternityJackGlider>()));
		}
	}
}