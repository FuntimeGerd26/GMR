using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class GerdBlade : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_927";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.width = 72;
			Projectile.height = 72;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
			Projectile.localNPCHitCooldown = 10;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			var target = Projectile.FindTargetWithinRange(1000f);
			if (target != null)
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 18f, 0.05f);
			else
				Projectile.velocity *= 0.97f;


			if (Projectile.timeLeft <= 30)
			{
				Projectile.alpha += 8;
				Projectile.velocity *= 0.9f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override Color? GetAlpha(Color lightColor) => new Color(125, 255, 55, 125);

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			player.AddBuff(ModContent.BuffType<Buffs.Buff.Empowered>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 1200);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Devilish>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Glimmering>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.ChillBurn>(), 600);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = new Color(125, 255, 55, 125);

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			// Main Projectile
			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = Color.Lerp(color26, Color.Transparent, (float)Math.Cos(Projectile.ai[0]) / 3 + 0.3f);
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle),
					color27 * Projectile.Opacity, Projectile.rotation, origin2, new Vector2(Projectile.scale * 1.2f, Projectile.scale * 0.5f), spriteEffects, 0);
			}
			return false;
		}
	}
}