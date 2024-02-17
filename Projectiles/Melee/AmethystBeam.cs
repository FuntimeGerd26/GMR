using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class AmethystBeam : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordHalfSpin";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet's Great Slasher");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(1);
		}

		public override void SetDefaults()
		{
			Projectile.width = 200;
			Projectile.height = 200;
			Projectile.aiStyle = -1;
			Projectile.light = 0.5f;
			Projectile.penetrate = 6;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 300;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 1.7f;
			Projectile.usesLocalNPCImmunity = true;
		}

		private bool DirectionOnSpawn;
		private int Direction;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			// Get direction on spawn so it's not changed
			if (!DirectionOnSpawn)
            {
				Direction = player.direction;
				DirectionOnSpawn = true;
			}

			Lighting.AddLight(Projectile.Center, new Vector3(0.7f, 0.1f, 0.95f));

			if (Projectile.timeLeft >= 140 && Projectile.timeLeft <= 150)
			{
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Direction * 16f));
			}
			else
			{
				Projectile.velocity *= 1.02f;
			}

			if (Projectile.timeLeft <= 30 || Projectile.penetrate == 1)
			{
				Projectile.alpha += 8;
				Projectile.velocity *= 0.96f;
				if (Projectile.alpha >= 255)
					Projectile.Kill();
			}

			if (Projectile.penetrate == 1)
				Projectile.damage = 0;

			Projectile.rotation += 6f * 0.03f * Direction;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Glimmering>(), 900);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			var opacity = Projectile.Opacity;

			Color color26 = new Color(100, 85, 155, 125) * opacity;

			SpriteEffects spriteEffects = Direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			// Main Projectile
			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27,
					Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
				Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"GMR/Projectiles/Melee/SwordHalfSpinLight", AssetRequestMode.ImmediateLoad).Value, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
					new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26,
				Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
			Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"GMR/Projectiles/Melee/SwordHalfSpinLight", AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				new Microsoft.Xna.Framework.Rectangle?(rectangle), new Color(200, 185, 255, 125) * opacity, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
			return false;
		}
	}
}