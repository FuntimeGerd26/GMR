using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class GerdSword : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(255, 255, 255),
			new Color(20, 20, 255),
			new Color(255, 255, 255),
			new Color(20, 125, 255),
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gerd's Spirit Sword");
			Tooltip.SetDefault("Throws the sword handle like a flail towards enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 110;
			Item.height = 110;
			Item.rare = 8;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 315);
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.damage = 180;
			Item.crit = 4;
			Item.knockBack = 1f;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.GerdHandle>();
			Item.shootSpeed = 6f;
			Item.channel = true;
			Item.noMelee = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int numColors = itemNameCycleColors.Length;

			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "ItemName")
				{
					float fade = (Main.GameUpdateCount % 30) / 30f;
					int index = (int)((Main.GameUpdateCount / 30) % numColors);
					int nextIndex = (index + 1) % numColors;

					line2.OverrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[nextIndex], fade);
				}
			}
		}
	}
}