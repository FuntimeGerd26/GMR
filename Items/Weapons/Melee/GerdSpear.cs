using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class GerdSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stella Salutis");
			Tooltip.SetDefault($"When hitting enemies with any of the projectiles, grants the player the 'Cutting Edge' buff");

			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.rare = 7;
			Item.value = Item.buyPrice(silver: 300); 
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 7;
			Item.useAnimation = 7;
			Item.UseSound = SoundID.Item7;
			Item.autoReuse = true; 
			Item.damage = 78;
			Item.knockBack = 1.5f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shootSpeed = 24f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.GerdSpear>();
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
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(18));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "Iceberg");
			recipe.AddIngredient(null, "ElementalSpear");
			recipe.AddIngredient(null, "PrincessTrident");
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}