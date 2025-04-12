using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Projectiles.Melee
{
	public class KizunaScythe : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_972";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kizuna Scythe");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Main.projFrames[Type] = 4;
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 10;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.95f, 0.25f, 0.3f));

			var target = Projectile.FindTargetWithinRange(180f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 12f, 0.09f);
			}

			Projectile.rotation += 8f * 0.03f;
			Projectile.localAI[0] += 0.5f;

			Projectile.direction = 1;

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.5f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage * 0.95);
			target.AddBuff(BuffID.Electrified, 300);

			SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.Center);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
			Projectile.velocity.Y * 0.5f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void Kill(int timeleft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.Center);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
			Projectile.velocity.Y * 0.5f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 105, 125);

		public override bool PreDraw(ref Color lightColor)
		{
			float opacity = Projectile.Opacity;
			SpriteEffects spriteEffects = SpriteEffects.None;

			#region What the actual fuck

			float num3 = Utils.Remap(0.5f, 0f, 0.6f, 0f, 1f) * Utils.Remap(0.5f, 0.6f, 1f, 0.8f, 0f);
			Color color6 = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
			Vector3 val = color6.ToVector3();
			float fromValue = val.Length() / (float)Math.Sqrt(3.0);
			fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);

			Texture2D swish = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Texture2D swishGlow = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordHalfSpinLight", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			var swishFrame = swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			var swishColor = new Color(125, 0, 25, 20) * opacity;
			var swishColor2 = new Color(255, 85, 125, 125) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;

			var flareColor = new Color(255, 255, 255, 0) * num3 * 0.5f * opacity;
			flareColor.A = (byte)(float)(int)(flareColor.A * (1f - fromValue));
			float swishRotation = Projectile.rotation;

			Color color5 = flareColor * fromValue * 0.5f;
			color5.G = (byte)(color5.G * fromValue);
			color5.B = (byte)(color5.R * (0.25f + fromValue * 0.75f));

			// Drawing Time
			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor * num3,
				swishRotation + MathHelper.ToRadians(-20f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor2 * 1.25f * num3,
				swishRotation + MathHelper.ToRadians(20f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, color5 * 0.15f,
				swishRotation + Projectile.ai[0] * 0.01f, swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor2 * num3 * 0.3f,
				swishRotation, swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor2 * num3 * 0.5f,
				swishRotation, swishOrigin, swishScale * 0.975f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor * num3,
				swishRotation, swishOrigin, swishScale * 0.75f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				swishColor2 * num3, swishRotation, swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				flareColor * num3, swishRotation, swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				flareColor * 0.8f * num3, swishRotation, swishOrigin, swishScale * 0.8f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				flareColor * 0.6f * num3, swishRotation, swishOrigin, swishScale * 0.65f, spriteEffects, 0);
			#endregion

			Texture2D texture2D13 = GMR.Instance.Assets.Request<Texture2D>("Items/Weapons/Melee/Others/KizunaScythe", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 origin2 = texture2D13.Size() / 2f;

			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
			{
				Color color26 = new Color(125, 0, 25, 20) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				Vector2 value4 = Projectile.oldPos[i];
				float num165 = Projectile.oldRot[i];

				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null,
					color27 * opacity, num165, origin2, Projectile.scale, spriteEffects, 0);

				Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
					color26 * opacity, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
			}
			return false;
		}
	}
}