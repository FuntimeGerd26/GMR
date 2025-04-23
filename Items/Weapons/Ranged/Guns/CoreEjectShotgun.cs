using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
    public class CoreEjectShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.AddElement(3);
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 26;
            Item.rare = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.reuseDelay = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(silver: 265);
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item36.WithPitchOffset(-0.25f);
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 20;
            Item.crit = 10;
            Item.knockBack = 6f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>();
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, -4);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.reuseDelay = 80;
                Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShotgunCore>();
                Item.UseSound = SoundID.Item61;
            }
            else
            {
                Item.reuseDelay = 40;
                Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>();
                Item.UseSound = SoundID.Item36.WithPitchOffset(-0.25f);
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position.Y = position.Y - 10;
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectileDirect(source, position, velocity * 0.8f, ModContent.ProjectileType<Projectiles.Ranged.ShotgunCore>(), damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item36.WithPitchOffset(-0.1f), player.position);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    // Rotate the velocity randomly between 5 degrees.
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5f));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 1f - Main.rand.NextFloat(0.5f);

                    int p = Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
                    Main.projectile[p].penetrate += 2;
                    Main.projectile[p].usesLocalNPCImmunity = true;
                    Main.projectile[p].localNPCHitCooldown = 5;
                }
                Projectile.NewProjectileDirect(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "AluminiumGun");
            recipe.AddIngredient(ItemID.SoulofFright, 12);
            recipe.AddIngredient(ItemID.HallowedBar, 14);
            recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}