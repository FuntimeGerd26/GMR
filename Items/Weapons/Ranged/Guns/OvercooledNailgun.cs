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
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;
using GMR;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class OvercooledNailgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Over-Cooled Nailgun");
			Tooltip.SetDefault("Slowly overcools and slows down due to the barrels getting clogged with ice\nCan randomly unclog releasing small bursts of projectiles");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 72;
			Item.height = 32;
			Item.rare = 6;
			Item.useTime = 1;
			Item.useAnimation = 1;
			Item.reuseDelay = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 235);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot1") { Volume = 0.25f, MaxInstances = 3, };
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 32;
			Item.crit = 0;
			Item.knockBack = 0.25f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<OvercooledNailgunHeldProj>();
			Item.shootSpeed = 8f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(
				source,
				position,
				velocity,
				Item.shoot,
				damage,
				knockback,
				player.whoAmI
				);

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Megashark);
			recipe.AddIngredient(null, "InfraRedBar", 14);
			recipe.AddIngredient(ItemID.SoulofNight, 35);
			recipe.AddIngredient(ItemID.FrostCore);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}


	// Disclaimer: I have no idea how this works, All code using this is possible thanks to Pellucid Mod
	public class OvercooledNailgunHeldProj : ModProjectile, IDrawable
	{
		public int ChannelTime;
		public override string Texture => base.Texture.Replace("HeldProj", string.Empty);
		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 999;
			if (Projectile.extraUpdates > 0 || Projectile.extraUpdates < 0)
				Projectile.extraUpdates = 0;
		}

		int bulletType;
		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemType<OvercooledNailgun>())
			{
				bulletType = ContentSamples.ItemsByType[itemSource.AmmoItemIdUsed].shoot;
			}
			else
			{
				Projectile.Kill();
			}
		}

		ref float MuzzleFlashAlpha => ref Projectile.ai[1];
		Vector2 MuzzlePosition => Projectile.Center + directionToMouse * 42f;

		private void ShootBullets(int bulletType, int amount, Vector2 from, Vector2 velocity)
		{
			from.Y = from.Y - 4;
			for (int i = 0; i < amount; i++)
			{
				Projectile.NewProjectile(
					Projectile.GetSource_FromThis(),
					from,
					velocity.RotatedByRandom(MathHelper.ToRadians(6f)),
					ModContent.ProjectileType<Projectiles.Ranged.OvercooledBullet>(),
					Projectile.damage,
					Projectile.knockBack,
					Projectile.owner);

				SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot1") { Volume = 0.25f, }, Projectile.Center);
				Player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.15f);
			}

			if (overcool < 60f)
				overcool += 0.15f;
			MuzzleFlashAlpha = 1f;
		}

		Player Player => Main.player[Projectile.owner];

		Vector2 directionToMouse;
		Vector2 recoil;

		bool shotBullets;
		float overcool;
		int shotspeed;
		public override void AI()
		{
			if (Main.MouseWorld.X < Player.Center.X)
				Player.direction = -1;
			else if (Main.MouseWorld.X > Player.Center.X)
				Player.direction = 1;

			if (Player.dead || !Player.controlUseItem || Player.HeldItem.type != ItemType<OvercooledNailgun>())
			{
				Projectile.Kill();
				return;
			}

			if (Main.myPlayer == Player.whoAmI)
			{
				Vector2 shoulderPosition = Player.ShoulderPosition();
				directionToMouse = Player.ShoulderDirectionToMouse(ref shoulderPosition, 4f);
				Projectile.Center = shoulderPosition;

				float attackBuffs = (Player.GetAttackSpeed(DamageClass.Ranged) + Player.GetAttackSpeed(DamageClass.Generic)) * 0.5f;
				if (attackBuffs < 1f)
					attackBuffs = 1f;

				if (Player.controlUseItem)
				{
					shotspeed = (int)(Player.HeldItem.useTime * 2 - (Player.HeldItem.useTime * attackBuffs) + overcool);
					if (shotspeed < 1)
						shotspeed = 1;

					ChannelTime += 1;
					if (ChannelTime % shotspeed == 0)
						shotBullets = false;
				}

				if (!shotBullets)
				{
					shotBullets = true;
					ShootBullets(bulletType, 1, MuzzlePosition, directionToMouse * 18f);

					recoil += new Vector2(Main.rand.NextFloat(0.5f, 2f), Main.rand.NextFloat(0, 0.05f));
				}
			}

			Projectile.rotation = directionToMouse.ToRotation() + recoil.Y * -Player.direction;

			Player.heldProj = Projectile.whoAmI;
			Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

			recoil *= 0.875f;
			MuzzleFlashAlpha *= 0.7f;
		}

		public override bool ShouldUpdatePosition() => false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(directionToMouse);
			writer.WriteVector2(recoil);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			directionToMouse = reader.ReadVector2();
			recoil = reader.ReadVector2();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 normOrigin = new Vector2(-7f, 14f) + Vector2.UnitX * recoil.X;

			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
			int y3 = num156 * Projectile.frame;
			Rectangle rectangle = new Rectangle(0, y3, texture.Width, num156);

			Main.EntitySpriteDraw(
				texture,
				Projectile.Center - Main.screenPosition,
				new Microsoft.Xna.Framework.Rectangle?(rectangle),
				lightColor,
				Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
				Player.direction == -1 ? new Vector2(texture.Width - normOrigin.X, normOrigin.Y) : normOrigin,
				Projectile.scale,
				Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
				);
			Main.EntitySpriteDraw(
				ModContent.Request<Texture2D>($"GMR/Items/Weapons/Ranged/Guns/OvercooledNailgun_Glow", AssetRequestMode.ImmediateLoad).Value,
				Projectile.Center - Main.screenPosition,
				new Microsoft.Xna.Framework.Rectangle?(rectangle),
				Color.Lerp(Color.Transparent, Color.White, MuzzleFlashAlpha) * (6f - (float)(shotspeed / 10)),
				Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
				Player.direction == -1 ? new Vector2(texture.Width - normOrigin.X, normOrigin.Y) : normOrigin,
				Projectile.scale,
				Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
				);
			return false;
		}

		DrawLayer IDrawable.DrawLayer => DrawLayer.BeforeProjectiles;
		public void Draw(Color lightColor)
		{
			Texture2D muzzleFlash = Request<Texture2D>("GMR/Assets/Images/MuzzleFlash", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 muzzleOrigin = muzzleFlash.Size() * 0.5f + Vector2.UnitX * 68;
			float muzzleRot = directionToMouse.ToRotation() + MathHelper.Pi;

			static Vector2 MuzzleSize(float flashAlpha) => 0.6f * new Vector2(1, 1 + MathF.Pow(flashAlpha, 2) * 0.4f) * flashAlpha;

			Main.EntitySpriteDraw(
				muzzleFlash,
				MuzzlePosition - Main.screenPosition,
				null,
				Color.Lerp(Color.Blue, Color.Cyan, MuzzleFlashAlpha) * MuzzleFlashAlpha,
				muzzleRot,
				muzzleOrigin,
				MuzzleSize(MuzzleFlashAlpha),
				SpriteEffects.None,
				0
				);
		}
	}
}