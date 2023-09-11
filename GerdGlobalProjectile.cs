using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using ReLogic.Content;
using GMR;

namespace GMR
{
	public class GerdGlobalProjectile : GlobalProjectile
    {
		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			//not doing this causes player array index error during worldgen in some cases maybe??
			if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
				return;

			Player player = Main.player[projectile.owner];

			if (projectile.hostile && projectile.ModProjectile?.Mod is GMR) // Hostile projectiles deal a good damage value, not too little not literally one shot
			{
				projectile.damage /= 2;
				if (Main.expertMode)
					projectile.damage /= Main.masterMode ? 2 : 1;
				if (Main.getGoodWorld)
					projectile.damage /= 2;
			}
		}

		public override void PostAI(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
		}

		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[projectile.owner];
			
			if (player.GPlayer().GambleCrown != null && projectile.friendly && Main.rand.NextBool(100))
				target.takenDamageMultiplier = 20f;
			else
				target.takenDamageMultiplier = 1f;

			if (player.GPlayer().MaskedPlagueCloak != null && projectile.friendly && projectile.DamageType?.CountsAsClass(DamageClass.Magic) == true && Main.rand.NextBool(10))
				player.Heal((int)(player.statLifeMax * 0.01));
		}
	}
}