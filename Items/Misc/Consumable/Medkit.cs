using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Consumable
{
	public class Medkit : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases health regeneration for 20 seconds");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.rare = 1;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(silver: 20);
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.consumable = true;
			Item.healLife = Main.LocalPlayer.statLifeMax2 / 20; // While we change the actual healing value in GetHealLife, Item.healLife still needs to be higher than 0 for the item to be considered a healing item
			Item.potion = true; // Makes it so this item applies potion sickness on use and allows it to be used with quick heal
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			// Find the tooltip line that corresponds to 'Heals ... life'
			TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "HealLife");

			if (line != null)
			{
				// Change the text to 'Heals max/2 (max/4 when quick healing) life'
				line.Text = Language.GetTextValue("CommonItemTooltip.RestoresLife", $"{Main.LocalPlayer.statLifeMax2 / 20}");
			}
		}

		public override void GetHealLife(Player player, bool quickHeal, ref int healValue)
		{
			// Make the item heal 5% of the player's health
			healValue = player.statLifeMax2 / 20;
		}

		public override bool? UseItem(Player player)
		{
			player.AddBuff(ModContent.BuffType<Buffs.Buff.MedkitRegen>(), 1200);
			return true;
		}
	}
}