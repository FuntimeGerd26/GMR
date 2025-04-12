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
	public class AltelierLogicRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 90;
			Item.height = 28;
			Item.rare = 3;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.reuseDelay = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/Shotgun") { Volume = 0f, MaxInstances = 3, PitchVariance = 0.5f };
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 50;
			Item.crit = 7;
			Item.knockBack = 14f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shoot = ProjectileType<AltelierLogicRifleHeldProj>();
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
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
	}

	// Disclaimer: I have no idea how this works, All code using this is possible thanks to Pellucid Mod
	public class AltelierLogicRifleHeldProj : ModProjectile, IDrawable
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
			if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemType<AltelierLogicRifle>())
			{
				bulletType = ContentSamples.ItemsByType[itemSource.AmmoItemIdUsed].shoot;
			}
			else
			{
				Projectile.Kill();
			}
		}

		ref float MuzzleFlashAlpha => ref Projectile.ai[1];
		ref float ChargeFlashAlpha => ref Projectile.ai[2];
		Vector2 MuzzlePosition => Projectile.Center + directionToMouse * 52f;

		private void ShootBullets(int bulletType, int amount, Vector2 from, Vector2 velocity)
		{
			from = Projectile.Center;
			from.Y = from.Y - 8f;
			for (int i = 0; i < amount; i++)
            {
                int p = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    from,
                    velocity,
                    ModContent.ProjectileType<Projectiles.Ranged.MagnumBullet>(),
                    (int)(Projectile.damage * 1.2f),
                    Projectile.knockBack,
                    Projectile.owner);
				Main.projectile[p].penetrate = 3;
				Main.projectile[p].ArmorPenetration = 20;

				SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/Shotgun") { Volume = 1f, MaxInstances = 3, PitchVariance = 0.5f }, Projectile.Center);
				Player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.25f);
			}

			MuzzleFlashAlpha = 1f;
			ChargeFlashAlpha = 1f;
		}

		Player Player => Main.player[Projectile.owner];

		Vector2 directionToMouse;
		Vector2 recoil;

		bool shotBullets;
		int shotspeed;
		public override void AI()
		{
			if (Main.MouseWorld.X < Player.Center.X)
				Player.direction = -1;
			else if (Main.MouseWorld.X > Player.Center.X)
				Player.direction = 1;

			if (Player.dead || !Player.controlUseItem || Player.HeldItem.type != ItemType<AltelierLogicRifle>())
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
					shotspeed = (int)(Player.HeldItem.useTime * 2 - (Player.HeldItem.useTime * attackBuffs));
					if (shotspeed < 4)
						shotspeed = 4;

					ChannelTime += 1;
					if (ChannelTime >= shotspeed)
						shotBullets = false;
				}

				if (!shotBullets)
				{
					shotBullets = true;
					ShootBullets(bulletType, 1, MuzzlePosition, directionToMouse * 18f);
					ChannelTime = 0;

					recoil += new Vector2(Main.rand.NextFloat(1.5f, 3f), Main.rand.NextFloat(0.25f, 0.5f));
				}
			}

			Projectile.rotation = directionToMouse.ToRotation() + recoil.Y * -Player.direction;

			Player.heldProj = Projectile.whoAmI;
			Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

			recoil *= 0.875f / (60f / shotspeed);
			MuzzleFlashAlpha *= 0.7f;
			ChargeFlashAlpha *= 0.935f;
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
			Vector2 normOrigin = new Vector2(16f, 16f) + Vector2.UnitX * recoil.X;

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
				ModContent.Request<Texture2D>($"GMR/Items/Weapons/Ranged/Guns/AltelierLogicRifle_Glow", AssetRequestMode.ImmediateLoad).Value,
				Projectile.Center - Main.screenPosition,
				new Microsoft.Xna.Framework.Rectangle?(rectangle),
				Color.Lerp(Color.Transparent, Color.Yellow, ChargeFlashAlpha),
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
				new Vector2(MuzzlePosition.X, MuzzlePosition.Y - 8f) - Main.screenPosition,
				null,
				Color.Lerp(Color.Orange, Color.Yellow, MuzzleFlashAlpha) * MuzzleFlashAlpha,
				muzzleRot,
				muzzleOrigin,
				MuzzleSize(MuzzleFlashAlpha),
				SpriteEffects.None,
				0
				);
		}
	}
}