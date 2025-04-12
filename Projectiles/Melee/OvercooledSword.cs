using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
    public class OvercooledSword : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Melee/Swords/OvercooledSword";

		// Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 36f;
		protected virtual float HoldoutRangeMax => 46f;

		public override void SetStaticDefaults()
		{
			Projectile.AddElement(1);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear);
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.height = 50;
			Projectile.width = 50;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			int duration = player.itemAnimationMax;

			player.heldProj = Projectile.whoAmI;

			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}

			Projectile.velocity = Vector2.Normalize(Projectile.velocity);

			float halfDuration = duration * 0.5f;
			float progress;

			if (Projectile.timeLeft < halfDuration)
			{
				progress = Projectile.timeLeft / halfDuration;

			}
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

			return false; // Don't execute vanilla AI.
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;
			SpriteEffects effects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.spriteBatch.Draw(texture2D13, Projectile.position + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null,
				Projectile.GetAlpha(lightColor), Projectile.velocity.ToRotation() + MathHelper.ToRadians(Projectile.direction == 1 ? 45f : 135f), origin2, 1f, effects, 0);
			return false;
		}
	}


	public class OvercooledSwordCut : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/SpearTrail";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(1);
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 240;
			Projectile.penetrate = 2;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}

		float decreaseScaleY;
		float velMult = 1f;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.25f, 0.5f, 1f));

			Projectile.rotation = Projectile.velocity.ToRotation();
			velMult *= 0.85f;
			Projectile.alpha += 16;
			decreaseScaleY += 0.1f;
			if (Projectile.alpha >= 255)
				Projectile.Kill();
			Projectile.velocity.Normalize();
			Projectile.velocity *= 32f * velMult;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 120);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			float opacity = Projectile.Opacity;

			Color color26 = new Color(55, 125, 255, 80);

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = Color.Lerp(color26, Color.Transparent, (float)Math.Cos(Projectile.ai[0]) / 3 + 0.3f);
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				float scale = Projectile.scale - (float)Math.Cos(Projectile.ai[0]) / 5;
				scale *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
				Main.spriteBatch.Draw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27 * opacity,
					num165, origin2, new Vector2(Projectile.scale * 0.5f, Projectile.scale * 0.2f * (1.2f - decreaseScaleY)), SpriteEffects.None, 0);
			}
			return false;
		}
	}
}