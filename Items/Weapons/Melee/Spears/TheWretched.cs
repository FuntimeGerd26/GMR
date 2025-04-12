using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Spears
{
	public class TheWretched : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(-1);
		}

		public override void SetDefaults()
		{
			Item.width = 100;
			Item.height = 100;
			Item.rare = 2;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 125);
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 19;
			Item.crit = 4;
			Item.knockBack = 14f;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.TheWretched>();
			Item.shootSpeed = 12f;
			Item.channel = true;
			Item.noMelee = true;
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
			if (combo >= 4f)
			{
				Item.useTime = 7;
				Item.useAnimation = 7;
				combo += 1f;
				return true;
			}
			else if (combo < 4f)
			{
				Item.useTime = 28;
				Item.useAnimation = 28;
				combo += 1f;
				return player.ownedProjectileCounts[Item.shoot] < 1;
			}

			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (combo >= 7f)
				combo = -1f;

			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(9f));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Obsidian, 25);
			recipe.AddIngredient(ItemID.CrimtaneBar, 22);
			recipe.AddIngredient(ItemID.TissueSample, 18);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Obsidian, 25);
			recipe2.AddIngredient(ItemID.DemoniteBar, 22);
			recipe2.AddIngredient(ItemID.ShadowScale, 18);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}