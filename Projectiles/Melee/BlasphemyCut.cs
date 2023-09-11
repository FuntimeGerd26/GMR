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
	public class BlasphemyCut : ModProjectile
	{
		public override string Texture => "GMR/Assets/Images/TwirlThing";

		public override void SetDefaults()
		{
			Projectile.width = 120;
			Projectile.height = 120;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 300;
			Projectile.light = 0.75f;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.localNPCHitCooldown = 15;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			Projectile.velocity *= 0.96f;

			if (Projectile.timeLeft <= 150)
			{
				Projectile.light += -0.05f;
				Projectile.alpha += 16;
			}

			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}

			if (Projectile.penetrate == 1)
			{
				Projectile.damage = 0;
				Projectile.alpha += 8;
			}

			Projectile.rotation = -Projectile.direction * Projectile.velocity.Length() * 4f;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(125, 5, 55) * Projectile.Opacity;

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			var opacity = Projectile.Opacity;

			Color color26 = new Color(125, 5, 55) * opacity;

			SpriteEffects spriteEffects = Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"GMR/Assets/Images/TwirlDust", AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
				color26, Projectile.rotation, origin2, Projectile.scale * 0.25f, spriteEffects, 0);
			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
				new Color(225, 105, 155) * opacity, Projectile.rotation, origin2, Projectile.scale * 0.25f, spriteEffects, 0);
			return false;
		}
	}
}