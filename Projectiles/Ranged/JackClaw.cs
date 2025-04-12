using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
	public class JackClaw : ModProjectile
	{
		public override string Texture => "GMR/NPCs/Bosses/Jack/JackArmClaw";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 55, 55);

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));

			Player player = Main.player[Projectile.owner];
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(270f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			var glow = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Jack/JackArmClaw_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
			var offset = Projectile.Size / 2f - Main.screenPosition;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.direction == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Color color = new Color(194, 91, 112, 5);

			Main.EntitySpriteDraw(texture, Projectile.position + offset, null, lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			Main.EntitySpriteDraw(glow, Projectile.position + offset, null, color, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
	}
}