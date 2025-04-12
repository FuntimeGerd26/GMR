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
	public class SunWrathSlash : ModProjectile
	{
		public override string Texture => "GMR/Empty";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thousand Sun Wrath Slash");
			ProjectileID.Sets.TrailCacheLength[Type] = 15;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.scale = 2.1f;
			Projectile.tileCollide = false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(85, 185, 25, 25);
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
				if (Projectile.scale > 2f)
				{
					Projectile.scale -= 0.02f;
					if (Projectile.scale < 2f)
					{
						Projectile.scale = 2f;
					}
				}
			}

			var target = Projectile.FindTargetWithinRange(500f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 18f, 0.009f);
			}

			Projectile.rotation += 18f * 0.03f * Projectile.direction;
			Projectile.velocity *= 0.99f;
			Projectile.alpha += 8;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage * 0.95);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Projectile.velocity * 0.75f,
				ModContent.ProjectileType<Projectiles.SmallIchorExplosion>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
			target.AddBuff(BuffID.Ichor, 600);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, Color.Yellow, 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			if (Projectile.alpha < 200)
			{
				for (int i = 0; i < 30; i++)
				{
					var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, newColor: Color.Yellow, Scale: 2f);
					d.velocity *= 0.4f;
					d.velocity += Projectile.velocity * 0.5f;
					d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
					d.scale *= Projectile.scale * 0.6f;
					d.fadeIn = d.scale + 0.1f;
					d.noGravity = true;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var texture = TextureAssets.Projectile[Type].Value;
			var drawPosition = Projectile.Center;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			lightColor = Projectile.GetAlpha(lightColor);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			var opacity = Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, new Color(255, 155, 40, 25) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale, SpriteEffects.None, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, new Color(255, 155, 40, 25) * opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

			var swish = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordHalfSpin", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = new Color(255, 155, 40, 25) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale;
			var swishPosition = Projectile.position + drawOffset;

			Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
			var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
			var flareOrigin = flare.Size() / 2f;
			float flareOffset = (swish.Width / 2f - 4f);
			var flareDirectionNormal = Vector2.Normalize(Projectile.velocity) * flareOffset;
			float flareDirectionDistance = 200f;
			for (int i = 0; i < 2; i++)
			{
				float swishRotation = Projectile.rotation + MathHelper.Pi * i;
				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation,
					swishOrigin,
					swishScale, SpriteEffects.None, 0);

				swish = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordHalfSpinLight", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation,
					swishOrigin,
					swishScale, SpriteEffects.None, 0);

				var circle = GMR.Instance.Assets.Request<Texture2D>("Assets/Images/TwirlThing", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				Main.EntitySpriteDraw(
					circle,
					Projectile.Center - Main.screenPosition,
					null,
					swishColor,
					swishRotation,
					circle.Size() / 2,
					swishScale * 0.4f, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}