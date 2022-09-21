using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;

namespace GMR.Items.Accessories
{
	public class BLBook : ModItem
	{
		public bool flip;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BL Book");
			Tooltip.SetDefault("Summons 4 orbiting BL Books around the player\nIncreases all damage by 20%\nIncreases damage taken by 10%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.DamageType = ModContent.GetInstance<Classless>();
			Item.damage = 40;
			Item.crit = 4;
			Item.knockBack = 8f;
			Item.value = Item.sellPrice(silver: 100);
			Item.rare = 2;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.2f;
			player.AddBuff(ModContent.BuffType<Buffs.Minions.BLBook>(), 2);
			player.GPlayer().BLBook = true;
			flip = !flip;
			if (player.HasBuff(ModContent.BuffType<Buffs.Minions.BLBook>()) && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.BLBook>()] < 1)
			{ 
				const int max = 4;
				Vector2 velocity = new Vector2(0f, -2f);
				for (int i = 0; i < max; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(2 * Math.PI / max * i);
					Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Melee.BLBook>(), 40, 8f, player.whoAmI, 0, (player.Center - 80 * Vector2.UnitX - player.position).Length() * (flip ? 1 : -1));
				}
			}
		}
	}
}