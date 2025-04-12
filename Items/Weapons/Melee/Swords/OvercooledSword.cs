using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class OvercooledSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 56;
			Item.rare = 6;
			Item.value = Item.buyPrice(silver: 325);
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useTime = 7;
			Item.useAnimation = 7;
			Item.UseSound = SoundID.Item7;
			Item.autoReuse = true;
			Item.damage = 35;
			Item.crit = -3;
			Item.knockBack = 3f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.OvercooledSword>();
			Item.shootSpeed = 12f;
		}

		public override bool? UseItem(Player player)
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(14f));
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Melee.OvercooledSword>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Melee.OvercooledSwordCut>(), damage, knockback, player.whoAmI);
			return false;
		}
	}
}