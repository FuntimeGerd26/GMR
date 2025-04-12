using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Bosses
{
	public class JackMarker : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/StopAura";

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.timeLeft = 120;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));

			Projectile.rotation += 0.015f;
			Projectile.velocity.Normalize();
			Projectile.velocity *= 0.1f;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 55, 55);

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;

			Color color26 = new Color(255, 55, 55, 55);
			Main.EntitySpriteDraw(texture2D13, Projectile.position + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color26, Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}