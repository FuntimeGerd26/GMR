using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Summon.Whips
{
	public class RealitySnap : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordSlash";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reality Slash");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.aiStyle = 0;
			Projectile.light = 1.25f;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 1200; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 9;
			Projectile.localNPCHitCooldown = 40;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.scale = 0.5f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(40, 120, 200, 0);
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.timeLeft -= 120;
			Projectile.damage /= 2;
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.ChillBurn>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Thoughtful>(), 600);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void Kill(int timeleft)
		{
			Projectile.position = Projectile.Center;
			Projectile.width = Projectile.height = 40;
			Projectile.position.X -= (float)(Projectile.width / 2);
			Projectile.position.Y -= (float)(Projectile.height / 2);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = new Color(40, 120, 200, 55);
			color26 = Projectile.GetAlpha(color26);

			SpriteEffects effects = SpriteEffects.None;

			// Trail funny
			float modifier = Projectile.localAI[1] * MathHelper.Pi / 45;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Player player = Main.player[Projectile.owner];
				Texture2D glow = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
				Color color27 = Color.Lerp(color26, Color.Transparent, 1 / 3 + 0.3f);
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				float scale = Projectile.scale - 1 / 5;
				scale *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = Math.Max((int)i - 1, 0);
				Vector2 center = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], (1 - i % 1));
				float smoothtrail = i % 1 * (float)Math.PI / 3.45f;
				bool withinangle = Projectile.rotation > -Math.PI / 2 && Projectile.rotation < Math.PI / 2;
				if (withinangle && player.direction == 1)
					smoothtrail *= -1;
				else if (!withinangle && player.direction == -1)
					smoothtrail *= -1;
				center += Projectile.Size / 2;

				Vector2 offset = (Projectile.Size / 12).RotatedBy(Projectile.oldRot[(int)i] - smoothtrail * (-Projectile.direction));
				Main.spriteBatch.Draw(glow, center - offset - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27, Projectile.rotation, glow.Size() / 2, scale, effects, 0f);
			}

			for (float i = 0; i < 1; i += 0.5f)
			{
				Player player = Main.player[Projectile.owner];
				int max0 = Math.Max((int)i - 1, 0);
				Vector2 center = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], (1 - i % 1));
				float smoothtrail = i % 1 * (float)Math.PI / 3.45f;
				bool withinangle = Projectile.rotation > -Math.PI / 2 && Projectile.rotation < Math.PI / 2;
				if (withinangle && player.direction == 1)
					smoothtrail *= -1;
				else if (!withinangle && player.direction == -1)
					smoothtrail *= -1;
				center += Projectile.Size / 2;

				Vector2 offset = (Projectile.Size / 12).RotatedBy(Projectile.oldRot[(int)i] - smoothtrail * (-Projectile.direction));
				Main.EntitySpriteDraw(texture2D13, center - offset - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, Projectile.rotation, origin2, Projectile.scale, effects, 0);
			}
			return false;
		}
	}
}