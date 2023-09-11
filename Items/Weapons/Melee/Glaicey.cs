using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class Glaicey : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots swords that inflict Frostburn to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 56;
			Item.height = 56;
			Item.rare = 3;
			Item.useTime = 32;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 30);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 40;
			Item.crit = 10;
			Item.knockBack = 3.5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.Glaicy>();
			Item.shootSpeed = 6f;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(1))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 67);
			}
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 600);
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