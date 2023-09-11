using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class Iceberg : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts 'Frostburn' to enemies");

			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = 3;
			Item.value = Item.buyPrice(silver: 101);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 8;
			Item.useAnimation = 24;
			Item.UseSound = SoundID.Item7;
			Item.autoReuse = true; 
			Item.damage = 26;
			Item.knockBack = 1f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shootSpeed = 18f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.IcebergProj>();
		}

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
			recipe.AddIngredient(ItemID.IceBlock, 30);
			recipe.AddIngredient(ItemID.SnowBlock, 40);
			recipe.AddIngredient(ItemID.Bone, 45);
			recipe.AddRecipeGroup("GMR:AnyGem", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}