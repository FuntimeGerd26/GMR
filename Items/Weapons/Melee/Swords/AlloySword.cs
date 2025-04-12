using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR.Projectiles.Melee;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class AlloySword : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.width = 68;
			Item.height = 68;
			Item.rare = 3;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 200);
			Item.DamageType = DamageClass.Melee;
			Item.crit = -3;
			Item.knockBack = 8f;
			Item.shootSpeed = 12f;
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.noMelee = true;
				Item.noUseGraphic = true;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AlloySwordThrow>();
				Item.UseSound = SoundID.Item7;
			}
			else
			{
				Item.noMelee = false;
				Item.noUseGraphic = false;
				Item.shoot = 0;
				Item.UseSound = SoundID.Item1;
			}
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			Vector2 projPos = new Vector2(target.Center.X + Main.rand.Next(-40, 40), target.Center.Y + Main.rand.Next(-40, 40));
			Vector2 velocityToTarget = ((new Vector2(target.Center.X, target.Center.Y)) - projPos) * 0.2f;

			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.AlloyMetalCut>()] < 3)
				Projectile.NewProjectile(player.GetSource_FromThis(), projPos, velocityToTarget, ModContent.ProjectileType<Projectiles.Melee.AlloyMetalCut>(), (int)(Item.damage * 1.1f), 0f, Main.myPlayer);

			for (int i = 0; i < 5; i++)
			{
				Dust dustId = Dust.NewDustDirect(target.position, target.width, target.height, 91, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 60, default(Color), 1f);
				dustId.noGravity = true;
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = position - 10 * Vector2.UnitY;
			if (Main.rand.NextBool(3) && player.altFunctionUse == 2)
			{
				type = ModContent.ProjectileType<AlloySwordThrowMultiplicate>();
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdOldSword");
			recipe.AddIngredient(null, "AlloyDagger");
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(null, "AlloyBox");
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}