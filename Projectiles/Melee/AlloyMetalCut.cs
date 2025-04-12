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
	public class AlloyMetalCut : ModProjectile
	{
		public override string Texture => "GMR/Empty";

		public override void SetStaticDefaults()
		{
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 120;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}
		float decreaseScaleY;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.5f, 1.25f, 0.5f));

			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.velocity *= 0.98f;
			Projectile.alpha += 8;
			decreaseScaleY += 0.1f;
			if (Projectile.alpha >= 255)
				Projectile.Kill();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = TextureAssets.Extra[98].Value;

			Main.spriteBatch.Draw(texture2D13, Projectile.position + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, Color.White * Projectile.Opacity,
				Projectile.rotation, texture2D13.Size() / 2f, new Vector2(Projectile.scale * 4f, Projectile.scale * 0.25f * (1f - decreaseScaleY)), SpriteEffects.None, 0);
			return false;
		}
	}
}