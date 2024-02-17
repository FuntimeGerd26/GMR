using GMR;
using System;
using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace GMR.Items.Weapons
{
	public class SilentGloves : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(125, 125, 125),
			new Color(55, 55, 55),
		};

		public int GloveMode;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($" Right-Click to change attack types: Sword, Katana, Rifle\nUses bullets on Rifle mode\n'O sorrow'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.AddElement(1);
            Item.AddElement(2);
        }

		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.DamageType = DamageClass.Generic;
			Item.width = 42;
			Item.height = 42;
			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 200);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
            if (player.altFunctionUse == 2)
            {
                if (GloveMode < 2)
                    GloveMode++;
                else
                    GloveMode = 0;

                Rectangle displayPoint = new Rectangle(player.Hitbox.Center.X, player.Hitbox.Center.Y - player.height / 4, 2, 2);
                if (GloveMode == 0) // Cool Swing
                {
                    CombatText.NewText(displayPoint, Color.Black, "Sword");
                }
                else if (GloveMode == 1) // Katana Swing
                {
                    CombatText.NewText(displayPoint, Color.Black, "Katana");
                }
                else if (GloveMode == 2) // Gun Shoot 
                {
                    CombatText.NewText(displayPoint, Color.Black, "Rifle");
                }
                return false;
            }
            else if (GloveMode == 0) // Cool Swing
            {
                Item.DamageType = DamageClass.Melee;
                Item.UseSound = SoundID.Item1;
                Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.SilentSword>(36);
                Item.useTime /= 3;
                Item.SetWeaponValues(42, 1f, 10);
                Item.useStyle = ItemUseStyleID.Swing;
                Item.reuseDelay = 2;
                Item.useAmmo = 0;
            }
            else if (GloveMode == 1) // Katana Swing
            {
                Item.DamageType = DamageClass.Melee;
                Item.UseSound = SoundID.Item1;
                Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.SilentKatana>(16);
                Item.useTime /= 3;
                Item.SetWeaponValues(26, 1f, 10);
                Item.useStyle = ItemUseStyleID.Swing;
                Item.reuseDelay = 2;
                Item.useAmmo = 0;
            }
            else if (GloveMode == 2) // Gun Shoot 
            {
                Item.damage = 24;
                Item.DamageType = DamageClass.Ranged;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.crit = 10;
                Item.knockBack = 4f;

                Item.shoot = ModContent.ProjectileType<SilentGun>();
                Item.shootSpeed = 12f;
                Item.UseSound = SoundID.Item11;

                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.useAmmo = AmmoID.Bullet;
                return player.ownedProjectileCounts[Item.shoot] <= 0;
            }
            return true;
		}

		public override bool CanShoot(Player player)
		{
    		return player.ownedProjectileCounts[Item.shoot] <= 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (GloveMode == 2)
			{
				Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 1f);
				return false;
			}
			else
				return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int numColors = itemNameCycleColors.Length;

			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "ItemName")
				{
					float fade = (Main.GameUpdateCount % 60) / 60f;
					int index = (int)((Main.GameUpdateCount / 60) % numColors);
					int nextIndex = (index + 1) % numColors;

					line2.OverrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[nextIndex], fade);
				}
			}
		}

        // Disclaimer: I have no idea how this works, All code using this is possible thanks to Pellucid Mod
        public class SilentGun : ModProjectile, IDrawable
        {
            public override string Texture => "GMR/Items/Weapons/SilentGun";
            public int ChannelTime;

            public override void SetDefaults()
            {
                Projectile.width = 8;
                Projectile.height = 8;
                Projectile.aiStyle = -1;
                Projectile.penetrate = -1;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.friendly = false;
                Projectile.hostile = false;
                Projectile.timeLeft = 999;
                if (Projectile.extraUpdates > 1 || Projectile.extraUpdates < 1)
                    Projectile.extraUpdates = 1;
            }

            int bulletType;
            public override void OnSpawn(IEntitySource source)
            {
                if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemType<SilentGloves>())
                {
                    bulletType = ContentSamples.ItemsByType[itemSource.AmmoItemIdUsed].shoot;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            ref float MuzzleFlashAlpha => ref Projectile.ai[1];
            Vector2 MuzzlePosition => Projectile.Center + directionToMouse * 65f;

            private void ShootBullets(int bulletType, int amount, Vector2 from, Vector2 velocity)
            {
                for (int i = 0; i < amount; i++)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        from,
                        velocity.RotatedByRandom(MathHelper.PiOver4 * 0.05f) * Main.rand.NextFloat(0.85f, 1.25f),
                        bulletType,
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner);
                    SoundEngine.PlaySound(SoundID.Item41, Projectile.Center);
                }

                MuzzleFlashAlpha = 1f;
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

                if (!Player.controlUseItem || Player.HeldItem.type != ItemType<SilentGloves>())
                {
                    Projectile.Kill();
                    return;
                }

                if (Main.myPlayer == Player.whoAmI)
                {
                    Vector2 shoulderPosition = Player.ShoulderPosition();
                    directionToMouse = Player.ShoulderDirectionToMouse(ref shoulderPosition, 4f);
                    Projectile.Center = shoulderPosition;

                    float attackBuffs = (Player.GetAttackSpeed(DamageClass.Ranged) + Player.GetAttackSpeed(DamageClass.Generic) + Player.GetAttackSpeed(DamageClass.Melee)) * 0.5f;
                    if (attackBuffs < 1f)
                        attackBuffs = 1f;

                    if (Player.controlUseItem)
                    {
                        shotspeed = (int)((Player.HeldItem.useTime) / attackBuffs);
                        if (shotspeed < 1)
                            shotspeed = 1;

                        if (++ChannelTime % shotspeed == 0)
                            shotBullets = false;
                    }

                    if (!shotBullets)
                    {
                        shotBullets = true;

                        int bulletCount = 1;
                        ShootBullets(bulletType, bulletCount, MuzzlePosition, directionToMouse * 12f);

                        recoil += new Vector2(4f, Main.rand.NextFloat(0, 0.1f));
                    }
                }

                Projectile.rotation = directionToMouse.ToRotation() + recoil.Y * -Player.direction;

                Player.heldProj = Projectile.whoAmI;
                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

                recoil *= 0.475f;
                MuzzleFlashAlpha *= 0.8f;
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
                Vector2 normOrigin = new Vector2(20f, 12f) + Vector2.UnitX * recoil.X;

                Main.EntitySpriteDraw(
                    texture,
                    Projectile.Center - Main.screenPosition,
                    null,
                    lightColor,
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
                Vector2 muzzleOrigin = muzzleFlash.Size() * 0.5f + Vector2.UnitX * 40;
                float muzzleRot = directionToMouse.ToRotation() + MathHelper.Pi;

                static Vector2 MuzzleSize(float flashAlpha) => 0.6f * new Vector2(1, 1 + MathF.Pow(flashAlpha, 2) * 0.4f) * flashAlpha;

                Main.EntitySpriteDraw(
                    muzzleFlash,
                    MuzzlePosition - Main.screenPosition,
                    null,
                    Color.Lerp(Color.White, Color.Yellow, MuzzleFlashAlpha) * MuzzleFlashAlpha,
                    muzzleRot,
                    muzzleOrigin,
                    MuzzleSize(MuzzleFlashAlpha),
                    SpriteEffects.None,
                    0
                    );
            }
        }
    }
}