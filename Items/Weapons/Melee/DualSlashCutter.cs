using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;

namespace GMR.Items.Weapons.Melee
{
	public class DualSlashCutter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Today is friday in california'\nIncreases health regen when hitting an enemy\nRight-click to switch to gun mode");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 60;
			Item.height = 58;
			Item.rare = 6;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 125);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 90;
			Item.crit = 4;
			Item.knockBack = 5f;
			Item.scale = 1.5f;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 600);
			target.AddBuff(BuffID.Venom, 300);
			player.AddBuff(BuffID.Wrath, 300);
			player.AddBuff(BuffID.RapidHealing, 180);
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.DualGunShooter>(), 1));
		}
	}
}