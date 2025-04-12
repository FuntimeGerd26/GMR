using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic.Others
{
    public class MaskedPlagueDaggers : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.AddElement(2);
            Item.AddElement(3);
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.rare = 2;
            Item.value = Item.buyPrice(silver: 95);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.reuseDelay = 40;
            Item.UseSound = SoundID.Item7;
            Item.autoReuse = true;
            Item.damage = 28;
            Item.knockBack = 6f;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.MaskedPlagueDagger>();
            Item.shootSpeed = 18f;
        }

        public override bool? UseItem(Player player)
        {
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }
            return null;
        }

        float combo;
        public override bool CanUseItem(Player player)
        {
            if (combo >= 1f)
            {
                Item.useTime = 10;
                Item.useAnimation = 20;
                Item.reuseDelay = 35;
                combo = 0f;
            }
            else if (combo <= 0f)
            {
                Item.useTime = 15;
                Item.useAnimation = 15;
                Item.reuseDelay = 25;
                combo += 1f;
            }

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 3;
            float rotation = MathHelper.ToRadians(combo >= 1f ? 5f : Main.rand.NextFloat(7f, 14f));
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1.35f;
                Projectile.NewProjectile(source, position, perturbedSpeed, type, (int)(combo >= 1f ? damage * 0.5f : damage * 0.35f), knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("IronBar", 15);
            recipe.AddIngredient(ItemID.Feather, 20);
            recipe.AddIngredient(ItemID.Bone, 30);
            recipe.AddIngredient(ItemID.Silk, 18);
            recipe.AddRecipeGroup("GMR:AnyGem", 3);
            recipe.AddIngredient(null, "UpgradeCrystal", 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}