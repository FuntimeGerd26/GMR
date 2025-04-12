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
	public class JackSwordExplode : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordSlash2";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Slash");
			ProjectileID.Sets.TrailCacheLength[Type] = 30;
			ProjectileID.Sets.TrailingMode[Type] = 6;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 240;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.scale = 1.1f;
			Projectile.tileCollide = false;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}

		public override void AI()
		{
			if ((int)Projectile.ai[0] == 0)
			{
				Projectile.ai[0]++;
			}

			if (Projectile.alpha > 40)
			{
				if (Projectile.extraUpdates > 0)
				{
					Projectile.extraUpdates = 0;
				}
				if (Projectile.scale > 1f)
				{
					Projectile.scale -= 0.02f;
					if (Projectile.scale < 1f)
					{
						Projectile.scale = 1f;
					}
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.velocity *= 0.99f;
			int size = 140;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
			if (collding || Projectile.penetrate < 1)
			{
				Projectile.alpha += 8;
				Projectile.velocity *= 0.95f;
			}
			Projectile.alpha += 8;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}

			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.PartiallyCrystallized>(), 60);
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.JackExplosion>()] < 1)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.JackExplosion>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Main.myPlayer);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 63, Projectile.velocity.X * 0.5f,
	            Projectile.velocity.Y * 0.2f, 60, Color.Red, 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 63, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, Color.Red, 2f);
			Main.dust[dustId3].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			if (Projectile.alpha < 200)
			{
				for (int i = 0; i < 30; i++)
				{
					var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, newColor: new Color(255, 55, 55, 0), Scale: 1f);
					d.velocity *= 0.4f;
					d.velocity += Projectile.velocity * 0.5f;
					d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
					d.scale *= Projectile.scale * 0.6f;
					d.fadeIn = d.scale + 0.1f;
					d.noGravity = true;
				}
			}
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 85, 5);

		public override bool PreDraw(ref Color lightColor)
		{
			var texture = TextureAssets.Projectile[Type].Value;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			lightColor = Projectile.GetAlpha(lightColor);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			float opacity = Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.direction == 1)
				spriteEffects = SpriteEffects.FlipVertically;

			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, Projectile.GetAlpha(lightColor) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale, spriteEffects, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, Projectile.GetAlpha(lightColor) * opacity, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

			var swish = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			var swish2 = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordSlash2_Light", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = new Color(225, 55, 55, 255) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;
			float swishRotation = Projectile.rotation;
			for (int i = 0; i < 1; i++)
			{
				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation,
					swishOrigin,
					swishScale, spriteEffects, 0);
				Main.EntitySpriteDraw(
					swish2,
					swishPosition,
					swishFrame,
					new Color(225, 125, 125, 255) * opacity,
					swishRotation,
					swishOrigin,
					swishScale, spriteEffects, 0);
			}
			return false;
		}
	}
}