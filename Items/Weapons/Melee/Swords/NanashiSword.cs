using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class NanashiSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nanashi Sword");
			Tooltip.SetDefault("On hit has a 5% chance to cover the screen with the creation that deals massive damage in all the screen\n'Well, well I'm, I'm gonna make sure, you, you all, I'm gonna... I'mma make sure you... you, I... I'mma do it'");

			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.rare = 5;
			Item.value = Item.sellPrice(silver: 210);
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.damage = 30;
			Item.knockBack = 3f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shootSpeed = 6f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.NanashiSword>();
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
			recipe.AddIngredient(1365); // WoF Trophy
			recipe.AddTile(220); // Solidifier
			recipe.Register();
		}
	}
}