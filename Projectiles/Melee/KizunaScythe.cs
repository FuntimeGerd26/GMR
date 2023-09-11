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
		public override string Texture => "GMR/Items/Weapons/Melee/KizunaScythe";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kizuna Scythe");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 10;
			Projectile.timeLeft = 1200;
			Projectile.light = 0.25f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		private bool DirectionOnSpawn;
		private int Direction;
		public override void AI()
		{
			// Get direction on spawn so it's not changed
			if (!DirectionOnSpawn)
			{
				Direction = Projectile.direction;
				DirectionOnSpawn = true;
			}

			Lighting.AddLight(Projectile.Center, new Vector3(0.95f, 0.25f, 0.3f));

			var target = Projectile.FindTargetWithinRange(150f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 8f, 0.09f);
			}

			Projectile.rotation += 18f * 0.03f * Direction;

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.5f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage * 0.95);
			target.AddBuff(BuffID.Electrified, 300);
		}

		public override void Kill(int timeleft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.Center);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
			Projectile.velocity.Y * 0.5f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 105, 125, 0);

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			float opacity = Projectile.Opacity / 2;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;

			SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
			{
				Color color26 = new Color(155, 55, 75, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.Red * opacity, Projectile.rotation, origin2, Projectile.scale * 1.5f, spriteEffects, 0);

				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				Vector2 value4 = Projectile.oldPos[i];
				float num165 = Projectile.oldRot[i];
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale * 1.5f, spriteEffects, 0);
			}

			var swish = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordHalfSpin", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			var swish2 = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordHalfSpinLight", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = new Color(155, 55, 75, 0) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;

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
					swish2,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation,
					swishOrigin,
					swishScale, SpriteEffects.None, 0);


			}
			return false;
		}
	}
}