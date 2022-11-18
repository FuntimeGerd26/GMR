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

namespace GMR.Items.Weapons.Melee
{
	public class PsycopathAxe : ModItem
	{
		public bool flip;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Throw an axe that spirals arround you");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 44;
			Item.rare = 2;
			Item.useTime = 120;
			Item.useAnimation = 120;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 150);
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 40;
			Item.crit = 4;
			Item.knockBack = 2f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.PsycopathAxe>();
			Item.shootSpeed = 6f;
		}

		public override string Texture => base.Texture;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			flip = !flip;
			const int max = 1;
			for (int i = 0; i < max; i++)
			{
				Projectile.NewProjectile(source, position, velocity.RotatedBy(2 * Math.PI / max * i), type,
					damage, knockback, player.whoAmI, 0, (player.Center - 200 * Vector2.UnitX - position).Length() * (flip ? 1 : -1));
			}
			return false;
		}
	}
}