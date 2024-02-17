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
	public class AngelOfJudgementThrow : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_972";

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
			Projectile.AddElement(1);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 130;
			Projectile.height = 130;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 3;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.tileCollide = false;
			Projectile.scale = 1.85f;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.usesLocalNPCImmunity = true;
		}

		float VelMult = 1f;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.5f, 0.5f, 1f));
			Player player = Main.player[Projectile.owner];

			Projectile.rotation += 18f * 0.03f * Projectile.direction;
			int size = 129;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), 2, 2);
			if (collding || Projectile.penetrate <= 1)
			{
				Projectile.alpha += 16;
				Projectile.scale -= 0.01f;
				VelMult *= 0.80f;
			}
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
			Projectile.frame = 0;

			Projectile.velocity.Normalize();
			Projectile.velocity *= 36f * VelMult;

			Projectile.localAI[0]++;
			if (Projectile.localAI[0] == 30)
			{
				Projectile.velocity *= -1.35f;
			}

			Projectile.idStaticNPCHitCooldown = 10 - (int)(10 * (player.GetAttackSpeed(DamageClass.Melee) + player.GetAttackSpeed(DamageClass.Generic)));
			if (Projectile.idStaticNPCHitCooldown < 5)
				Projectile.idStaticNPCHitCooldown = 5;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage * 0.75);
			if (Projectile.alpha < 200)
			{
				for (int i = 0; i < 30; i++)
				{
					var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68, newColor: Color.White, Scale: 1f);
					d.velocity *= 0.4f;
					d.velocity += Projectile.velocity * 0.5f;
					d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
					d.scale *= Projectile.scale * 0.6f;
					d.fadeIn = d.scale + 0.1f;
					d.noGravity = true;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Projectile.alpha < 200)
			{
				for (int i = 0; i < 30; i++)
				{
					var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68, newColor: Color.White, Scale: 1f);
					d.velocity *= 0.4f;
					d.velocity += Projectile.velocity * 0.5f;
					d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
					d.scale *= Projectile.scale * 0.6f;
					d.fadeIn = d.scale + 0.1f;
					d.noGravity = true;
				}
			}
		}

		// This code is a fucking Amalgamation
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D textureSword = GMR.Instance.Assets.Request<Texture2D>("Items/Weapons/Melee/AngelOfJudgement", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
			Vector2 swordOrigin = new Vector2(textureSword.Width * 0.5f, textureSword.Height * 0.5f);
			Vector2 drawPos = (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
			var opacity = Projectile.Opacity;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.direction == -1)
				spriteEffects = SpriteEffects.FlipVertically;
			Color color = new Color(85, 185, 225, 10) * opacity;
			
			Main.EntitySpriteDraw(textureSword, drawPos, null, color, Projectile.rotation + MathHelper.ToRadians(45f), swordOrigin, 1f, spriteEffects, 0);

			float num3 = Utils.Remap(Projectile.localAI[0] / 60, 0f, 0.6f, 0f, 1f) * Utils.Remap(Projectile.localAI[0] / 60, 0.6f, 1f, 0.8f, 0f);
			Color color6 = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
			Vector3 val = color6.ToVector3();
			float fromValue = val.Length() / (float)Math.Sqrt(3.0);
			fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);

			Texture2D swish = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Texture2D swishGlow = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordHalfSpinLight", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			var swishFrame = swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			var swishColor = new Color(55, 155, 195, 40) * opacity;
			var swishColor2 = new Color(85, 185, 225, 125) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;

			var flareColor = new Color(255, 255, 255, 125) * num3 * 0.5f * opacity;
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
				swishColor2 * 0.8f * num3, swishRotation, swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				flareColor * 0.5f * num3, swishRotation, swishOrigin, swishScale * 0.8f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				flareColor * 0.4f * num3, swishRotation, swishOrigin, swishScale * 0.65f, spriteEffects, 0);

			// Why am i doing this like this
			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor * num3 * 0.5f,
				swishRotation + MathHelper.ToRadians(-140f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor2 * 1.25f * num3 * 0.5f,
				swishRotation + MathHelper.ToRadians(-100f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, color5 * 0.15f * 0.6f,
				swishRotation + Projectile.ai[0] * 0.01f + MathHelper.ToRadians(-120f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor2 * num3 * 0.3f * 0.5f,
				swishRotation + MathHelper.ToRadians(-120f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor2 * num3 * 0.5f * 0.5f,
				swishRotation + MathHelper.ToRadians(-120f * Projectile.direction), swishOrigin, swishScale * 0.975f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor * num3 * 0.5f,
				swishRotation + MathHelper.ToRadians(-120f * Projectile.direction), swishOrigin, swishScale * 0.75f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				swishColor2 * num3 * 0.5f, swishRotation + MathHelper.ToRadians(-120f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				swishColor2 * 0.8f * num3 * 0.5f, swishRotation + MathHelper.ToRadians(-120f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				flareColor * 0.5f * num3 * 0.5f, swishRotation + MathHelper.ToRadians(-120f * Projectile.direction), swishOrigin, swishScale * 0.8f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				flareColor * 0.4f * num3 * 0.5f, swishRotation + MathHelper.ToRadians(-120f * Projectile.direction), swishOrigin, swishScale * 0.65f, spriteEffects, 0);


			// Seriously why am i doing this like this
			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor * num3 * 0.35f,
				swishRotation + MathHelper.ToRadians(-260f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor2 * 1.25f * num3 * 0.35f,
				swishRotation + MathHelper.ToRadians(-220f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, color5 * 0.15f * 0.8f,
				swishRotation + Projectile.ai[0] * 0.01f + MathHelper.ToRadians(-240f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor2 * num3 * 0.3f * 0.35f,
				swishRotation + MathHelper.ToRadians(-240f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor2 * num3 * 0.5f * 0.35f,
				swishRotation + MathHelper.ToRadians(-240f * Projectile.direction), swishOrigin, swishScale * 0.975f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor * num3 * 0.35f,
				swishRotation + MathHelper.ToRadians(-240f * Projectile.direction), swishOrigin, swishScale * 0.75f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				swishColor2 * num3 * 0.35f, swishRotation + MathHelper.ToRadians(-240f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				swishColor2 * 0.8f * num3 * 0.35f, swishRotation + MathHelper.ToRadians(-240f * Projectile.direction), swishOrigin, swishScale, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				flareColor * 0.5f * num3 * 0.35f, swishRotation + MathHelper.ToRadians(-240f * Projectile.direction), swishOrigin, swishScale * 0.8f, spriteEffects, 0);

			Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
				flareColor * 0.4f * num3 * 0.35f, swishRotation + MathHelper.ToRadians(-240f * Projectile.direction), swishOrigin, swishScale * 0.65f, spriteEffects, 0);
			return false;
		}
	}
}