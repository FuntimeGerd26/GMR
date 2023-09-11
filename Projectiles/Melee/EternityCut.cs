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
	public class EternityCut : ModProjectile, IDrawable
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordSlashSmall";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = 10;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.6f, 0.05f));


			if (Projectile.timeLeft <= 150)
			{
				Projectile.alpha += 16;
			}

			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0f,
				Projectile.velocity.Y * 0f, 60, default(Color), 1f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0f,
				Projectile.velocity.Y * 0f, 60, default(Color), 0.75f);
			Main.dust[dustId3].noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.Center.X + Main.rand.Next(-target.width / 2, target.width / 2), target.Center.Y + Main.rand.Next(-target.height / 2, target.height / 2)),
				Projectile.velocity * 0f, ModContent.ProjectileType<Projectiles.Melee.EternalShine>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.Center.X + Main.rand.Next(-target.width / 2, target.width / 2), target.Center.Y + Main.rand.Next(-target.height / 2, target.height / 2)),
				Projectile.velocity * 0f, ModContent.ProjectileType<Projectiles.SmallExplotion>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0f,
				Projectile.velocity.Y * 0f, 60, default(Color), 1f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0f,
				Projectile.velocity.Y * 0f, 60, default(Color), 0.75f);
			Main.dust[dustId3].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = new Color(255, 120, 25, 155);

			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
			{
				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				Vector2 value4 = Projectile.oldPos[i];
				float num165 = Projectile.rotation;
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
					new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, SpriteEffects.None, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"GMR/Projectiles/Melee/SwordSlashSmall_Light", AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public float extraRot;
		DrawLayer IDrawable.DrawLayer => DrawLayer.AfterProjectiles;
		public void Draw(Color lightColor)
		{
			Texture2D flash = Request<Texture2D>("GMR/Assets/Images/Flash01", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 flashOrigin = flash.Size() / 2f;
			extraRot += 0.1f * Projectile.velocity.Length();

			Main.EntitySpriteDraw(
				flash,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				null,
				new Color(255, 120, 25) * Projectile.Opacity,
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
				new Color(255, 120, 25) * Projectile.Opacity,
				Projectile.rotation + MathHelper.ToRadians(90f + extraRot),
				flashOrigin,
				Projectile.scale * 0.45f,
				SpriteEffects.None,
				0
				);
		}
	}

	public class EternalShine : ModProjectile, IDrawable
	{
		public override string Texture => "GMR/Assets/Images/Circle";

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 9;
			Projectile.localNPCHitCooldown = 100;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.05f, 0.55f));

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

			Color color26 = new Color(255, 120, 25) * opacity;
			ScaleUp += 0.035f;

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
				color26, Projectile.rotation, origin2, Projectile.scale * 0.1f + ScaleUp, SpriteEffects.None, 0);
			return false;
		}

		DrawLayer IDrawable.DrawLayer => DrawLayer.AfterProjectiles;
		public void Draw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D flash = Request<Texture2D>("GMR/Assets/Images/Flash01", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 flashOrigin = flash.Size() / 2f;
			Color color26 = new Color(255, 120, 25) * Projectile.Opacity;

			Main.EntitySpriteDraw(flash, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color26,
				Projectile.rotation, flashOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(flash, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color26,
				Projectile.rotation + MathHelper.ToRadians(90f), flashOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0);
		}
	}
}