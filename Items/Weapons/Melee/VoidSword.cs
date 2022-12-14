using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class VoidSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots projectiles which inflict Frostburn and Weak to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 48;
			Item.rare = 5;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 70);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 44;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.scale = 1.5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.VoidScythe>();
			Item.shootSpeed = 6f;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			// 60 frames = 1 second
			target.AddBuff(BuffID.Frostburn, 900);
			target.AddBuff(BuffID.Weak, 600);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "Glaicey");
			recipe.AddIngredient(ItemID.Bone, 40);
			recipe.AddIngredient(ItemID.Hellstone, 25);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}