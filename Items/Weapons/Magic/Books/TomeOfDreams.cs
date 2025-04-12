using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;

namespace GMR.Items.Weapons.Magic.Books
{
	public class TomeOfDreams : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(255, 255, 255),
			new Color(225, 200, 255),
			new Color(0, 0, 0),
		};

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots a homing heart that inflicts 'Glimmering' to enemies" +
				$"\n Right-Click will give you 'Violet's Blessing' for a minute and shoot a faster and stronger heart, uses 50 mana\n'It is written in.. Spanish?'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(-1);
		}

		public override void SetDefaults()
        {
			Item.width = 30;
			Item.height = 30;
			Item.rare = 2;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 100);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 20;
			Item.crit = 14;
			Item.knockBack = 2.5f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.VioletHeart>();
			Item.shootSpeed = 6f;
			Item.mana = 8;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.damage = 30;
				player.AddBuff(ModContent.BuffType<Buffs.Buff.VioletBuff>(), 3600);
				Item.mana = 50;
				Item.shootSpeed = 18f;
			}
			else
			{
				Item.damage = 20;
				Item.mana = 8;
				Item.shootSpeed = 6f;
			}

			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int numColors = itemNameCycleColors.Length;

			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "ItemName")
				{
					float fade = (Main.GameUpdateCount % 60) / 60f;
					int index = (int)((Main.GameUpdateCount / 60) % numColors);
					int nextIndex = (index + 1) % numColors;

					line2.OverrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[nextIndex], fade);
				}
			}
		}
	}
}