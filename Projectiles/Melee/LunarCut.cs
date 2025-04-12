using System;
using System.IO;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace GMR.Projectiles.Melee
{
	public class LunarCut : ModProjectile, IDrawable
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordSlashSmall";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
			Projectile.AddElement(1);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 130;
			Projectile.height = 130;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 10;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.85f, 0.25f, 1f));

			Projectile.velocity *= 0.985f;
			if (Projectile.timeLeft <= 180)
			{
				Projectile.alpha += 16;
			}

			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.Center.X + Main.rand.Next(-target.width / 2, target.width / 2), target.Center.Y + Main.rand.Next(-target.height / 2, target.height / 2)),
				Projectile.velocity * 0f, ModContent.ProjectileType<LunarShine>(), 10, Projectile.knockBack, Main.myPlayer);
			SoundEngine.PlaySound(SoundID.Item30.WithPitchOffset(Main.rand.NextFloat(-0.5f, 0f)).WithVolumeScale(0.25f), Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;
			var opacity = Projectile.Opacity;

			SpriteEffects spriteEffects = SpriteEffects.None;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color26 = new Color(175, 55, 255, 55) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);

				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null,
					color27 * opacity, Projectile.rotation, origin2, new Vector2(Projectile.scale * 1.15f, Projectile.scale * 1.6f), spriteEffects, 0);

				Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"GMR/Projectiles/Melee/SwordSlashSmall_Light", AssetRequestMode.ImmediateLoad).Value,
					value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
					null, color27 * opacity, Projectile.rotation, origin2, new Vector2(Projectile.scale * 1.15f, Projectile.scale * 1.6f), spriteEffects, 0);
			}
			return false;
		}

		public float extraRot;
		DrawLayer IDrawable.DrawLayer => DrawLayer.AfterProjectiles;
		public void Draw(Color lightColor)
		{
			Texture2D flash = Request<Texture2D>("GMR/Assets/Images/Flash01", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 flashOrigin = flash.Size() / 2f;
			extraRot += 0.3f * 8f;

			Main.EntitySpriteDraw(
				flash,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				null,
				new Color(255, 135, 255) * Projectile.Opacity,
				Projectile.rotation + MathHelper.ToRadians(extraRot),
				flashOrigin,
				Projectile.scale * 0.75f,
				SpriteEffects.None,
				0
				);

			Main.EntitySpriteDraw(
				flash,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				null,
				new Color(255, 135, 255) * Projectile.Opacity,
				Projectile.rotation + MathHelper.ToRadians(90f + extraRot),
				flashOrigin,
				Projectile.scale * 0.45f,
				SpriteEffects.None,
				0
				);
		}
	}

	public class LunarShine : ModProjectile, IDrawable
	{
		public override string Texture => "GMR/Assets/Images/Circle";

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.timeLeft = 600;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 9;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 300;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.85f, 0.25f, 1f));

			Projectile.alpha += 2;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}

		public float ScaleUp;
		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;
			var opacity = Projectile.Opacity;

			Color color26 = new Color(175, 55, 255) * opacity;
			if (ScaleUp < 3f)
			 ScaleUp += 0.025f;

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
				color26, Projectile.rotation, origin2, Projectile.scale * 0.15f + ScaleUp, SpriteEffects.None, 0);
			return false;
		}

		DrawLayer IDrawable.DrawLayer => DrawLayer.AfterProjectiles;
		public void Draw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D flash = Request<Texture2D>("GMR/Assets/Images/Flash01", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 flashOrigin = flash.Size() / 2f;

			Main.EntitySpriteDraw(flash, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 135, 255) * Projectile.Opacity,
				Projectile.rotation, flashOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(flash, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 135, 255) * Projectile.Opacity,
				Projectile.rotation + MathHelper.ToRadians(90f), flashOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0);
		}
	}
}