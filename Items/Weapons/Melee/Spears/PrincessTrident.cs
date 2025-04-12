using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Spears
{
	public class PrincessTrident : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlantis Princess Trident");
			Tooltip.SetDefault($"'Bathwater pizza'\nGrants the 'Trident Bite' buff to the player when hitting an enemy");

			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;

			// Registers a vertical animation with 7 frames and each one will last 7 ticks
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 7));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.rare = 5;
			Item.value = Item.sellPrice(silver: 240); 
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 5;
			Item.useAnimation = 15;
			Item.reuseDelay = 40;
			Item.UseSound = SoundID.Item7;
			Item.autoReuse = true; 
			Item.damage = 58;
			Item.knockBack = 2.75f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shootSpeed = 14f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.PrincessTrident>();
		}

		public override Color? GetAlpha(Color lightColor) => new Color(125, 255, 255, 55);

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player)
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Trident);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(ItemID.SoulofLight, 16);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}