using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class PrismaticBeam : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordHugeSlash";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 400;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.scale = 1.1f;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Projectile.alpha > 40)
			{
				if (Projectile.extraUpdates > 0)
				{
					Projectile.extraUpdates = 0;
				}
				if (Projectile.scale > 1f)
				{
					Projectile.scale -= 0.001f;
					if (Projectile.scale < 1f)
					{
						Projectile.scale = 1f;
					}
				}
			}

			Projectile.velocity *= 0.98f;
			Projectile.light += -0.05f;
			Projectile.alpha += 7;

			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}

			if (Projectile.penetrate == 1)
			{
				Projectile.damage = 0;
				if (Projectile.timeLeft >= 200)
					Projectile.timeLeft = 200;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			if (Main.rand.NextBool(3))
			player.Heal(3);
		}

		public override void Kill(int timeleft)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 1f,
				Projectile.velocity.Y * 1f, 60, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
			Main.dust[dustId].noGravity = true;
			int dustId2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * -1f,
				Projectile.velocity.Y * -1f, 60, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
			Main.dust[dustId2].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			var opacity = Projectile.Opacity;

			Color color26 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 25) * 1.5f;

			SpriteEffects spriteEffects = SpriteEffects.None;

			var texture = TextureAssets.Projectile[Type].Value;
			var drawPosition = Projectile.Center;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			lightColor = Projectile.GetAlpha(lightColor);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, color26 * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale, SpriteEffects.None, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, color26 * opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordHugeSlash_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
				Projectile.position + drawOffset, frame, Color.White * opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}