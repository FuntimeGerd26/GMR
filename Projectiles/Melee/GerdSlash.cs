using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class GerdSlash : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordHalfSpin";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbra Del Slash");
			ProjectileID.Sets.TrailCacheLength[Type] = 2;
			ProjectileID.Sets.TrailingMode[Type] = 6;
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
			Projectile.localNPCHitCooldown = 30;
			Projectile.scale = 1.4f;
			Projectile.tileCollide = false;
		}

		float velMult = 1f;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.35f, 1f, 0.1f));

			if ((int)Projectile.ai[0] == 0)
			{
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				Projectile.ai[0]++;
			}
			Projectile.velocity.Normalize();
			Projectile.velocity *= 12f * velMult;

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
			Projectile.velocity *= 0.99f;
			Projectile.rotation += 18f * 0.03f * Projectile.direction;
			int size = 140;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
			if (collding)
			{
				Projectile.alpha += 8;
				velMult *= 0.88f;
			}
			Projectile.alpha += 8;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			Projectile.damage = (int)(Projectile.damage * 0.95);
			player.AddBuff(ModContent.BuffType<Buffs.Buff.Empowered>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 1200);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Devilish>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Glimmering>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.ChillBurn>(), 600);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 75, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, Color.White, 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, -oldVelocity.X, 0.75f);
			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, -oldVelocity.Y, 0.75f);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			float numberProjectiles = 7;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 projPos = new Vector2(Projectile.Center.X + Main.rand.Next(-Projectile.width / 2, Projectile.width / 2), Projectile.Center.Y + Main.rand.Next(-Projectile.height / 2, Projectile.height / 2));

				Vector2 perturbedSpeed = (Projectile.Center - projPos) * 0.1f;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), projPos, perturbedSpeed, ModContent.ProjectileType<Projectiles.Melee.GerdBlade>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Main.myPlayer);
			}

			SoundEngine.PlaySound(SoundID.Item105, Projectile.Center);

			for (int i = 0; i < 10; i++)
			{
				Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 75, Projectile.velocity.X, Projectile.velocity.Y, 60, default(Color), 1.5f);
				dustId.noGravity = true;
				Dust dustId2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 75, -Projectile.velocity.X, -Projectile.velocity.Y, 60, default(Color), 1.5f);
				dustId2.noGravity = true;
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
			float opacity = Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, new Color(125, 255, 55, 0) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale * 0.5f, SpriteEffects.None, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, Projectile.GetAlpha(lightColor) * opacity, Projectile.rotation, origin, Projectile.scale * 0.75f, SpriteEffects.None, 0);

			var swish = TextureAssets.Projectile[Type].Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = new Color(125, 255, 55, 0) * opacity;
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


				for (int j = 0; j < 3; j++)
				{
					var flarePosition = (swishRotation + MathHelper.ToRadians(180f) + 0.6f * (j - 1)).ToRotationVector2() * flareOffset;
					float flareIntensity = Math.Max(flareDirectionDistance - Vector2.Distance(flareDirectionNormal, flarePosition), 0f) / flareDirectionDistance;
					Main.EntitySpriteDraw(
						flare,
						swishPosition + flarePosition,
						null,
						swishColor * flareIntensity * 3f * 0.4f,
						0f,
						flareOrigin,
						new Vector2(swishScale * 0.7f, swishScale * 2f) * flareIntensity, SpriteEffects.None, 0);

					Main.EntitySpriteDraw(
						flare,
						swishPosition + flarePosition,
						null,
						swishColor * flareIntensity * 3f * 0.4f,
						MathHelper.PiOver2,
						flareOrigin,
						new Vector2(swishScale * 0.8f, swishScale * 2.5f) * flareIntensity, SpriteEffects.None, 0);
				}
			}
			return false;
		}
	}
}