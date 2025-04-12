using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Accessories.SoulsContent.Enchantments.Forces
{
    public class ForceOfCurses : ModItem
    {
        public bool flip;
        public int Timer;

        public static int[] Enchants => new int[]
        {
            ModContent.ItemType<SandwaveEnchantment>(),
            ModContent.ItemType<IcePrincessEnchantment>(),
            ModContent.ItemType<JackEnchantment>(),
            ModContent.ItemType<BoostEnchantment>(),
            ModContent.ItemType<ArkEnchantment>()
        };

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases max minions and sentries by 3, increases armor penetration, attack speed and knockback by 20%, increases movement speed and damage by 35% and increases jump speed and wing time by 20%" +
                "\nIncreases mana by 40 and increases damage reduction by 5%" +
                $"\nEvery 2 seconds you shoot a knife that homes into enemies and gives the player the 'Sunburnt' and 'Empowered' buffs" +
                $"\nIncreases invincibility frames by 2 seconds and weapons have a chance to shoot 3 projectile that deal 75% damage and shoot an aditional special projectile" +
                $"\nMelee and ranged attack inflict frostburn" +
                $"\nA style of swords can now shoot an extra projectile that goes through walls and deals 50% more damage" +
                $"\nMelee weapons that shoot projectiles now shoot an aditional fire ball" +
                "\nMakes lightnings fall from the sky when taking damage and using any weapon that's not ranged will shoot a fireball that explodes dealing damage on a large area" +
                "\nGreatly increases movement speed and allows to walk on water and lava" +
                $"\nGrants the 'Blood Overflow' buff, when under 75% of health: increased armor penetration, crit chance, damage by 14% and decreased mana cost by 10%" +
                "\nSummons 8 orbiting BL Books and Psyco Axes around the player and increases damage taken by 10%" +
                "\nMelee weapons shoot an Alloyblood Dagger that inflicts 'Devilish' to enemies, hitting enemies with this dagger has a chance to drop Alloyblood Cans above you which heal 15% life\n'The end is never the end is never the end'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.damage = 200;
            Item.DamageType = DamageClass.Generic;
            Item.crit = 14;
            Item.knockBack = 18f;
            Item.rare = 11;
            Item.value = Item.sellPrice(silver: 420);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Boost
            if (player.GPlayer().EnchantToggles["BoostSet"])
                player.GPlayer().BoostSet = Item;
            player.GPlayer().Thunderblade = Item;
            if (player.GPlayer().EnchantToggles["BoostFireball"])
            {
                player.GPlayer().NajaCharm = Item;
            }
            player.waterWalk = true;
            player.fireWalk = true;
            player.wingTimeMax += player.wingTimeMax / 5;
            player.runAcceleration += 0.15f;
            player.maxRunSpeed += 0.75f;
            player.manaCost -= 0.1f;

            // Ice Princess
            if (player.GPlayer().EnchantToggles["IcePrincessShuriken"])
                player.GPlayer().IcePrincessEnch = Item;
            player.frostArmor = true;
            player.statManaMax += 40;


            // Infra-Red
            player.GPlayer().InfraRedSet = Item;
            player.jumpSpeedBoost += 0.20f;

            // Sandwave
            player.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 2);
            player.AddBuff(ModContent.BuffType<Buffs.Buff.Empowered>(), 2);
            player.endurance += 0.05f;
            if (player.GPlayer().EnchantToggles["SandwaveKnife"] && ++Timer % 120 == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.SandwaveKnife>()] < 1)
            {
                for (int i = 0; i < 1; i++)
                {
                    Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, new Vector2(0f, -6f), ModContent.ProjectileType<Projectiles.SandwaveKnife>(), Item.damage, 6f, player.whoAmI);
                }
            }

            // Ark
            player.AddBuff(ModContent.BuffType<Buffs.Buff.BloodFountain>(), 2);
            player.GetDamage(DamageClass.Generic) += 0.35f;
            player.moveSpeed += 0.35f;
            player.maxMinions += 3;
            player.maxTurrets += 3;
            player.GetKnockback(DamageClass.Generic) += 0.20f;
            player.GetArmorPenetration(DamageClass.Generic) += 20f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.20f;
            if (player.GPlayer().EnchantToggles["AlloybloodDagger"])
            {
                player.GPlayer().AlloybloodEnch = Item;
            }

            if (player.statLife < (int)(player.statLifeMax * 0.25f))
            {
                player.AddBuff(ModContent.BuffType<Buffs.Buff.ArkBuffBoost>(), 2);
            }
            else
                player.AddBuff(ModContent.BuffType<Buffs.Buff.ArkBuff>(), 2);

            if (player.GPlayer().EnchantToggles["AlloybloodOrbitingProjectiles"])
            {
                player.GPlayer().BLBook = true;
                flip = !flip;
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.BLFujoshi>()] < 1)
                {
                    const int max = 6;
                    Vector2 velocity = new Vector2(0f, -2f);
                    for (int i = 0; i < max; i++)
                    {
                        Vector2 perturbedSpeed = velocity.RotatedBy(2 * Math.PI / max * i);
                        Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Melee.BLFujoshi>(), Item.damage, 14f,
                            player.whoAmI, 0, (player.Center - 100 * Vector2.UnitX - player.position).Length() * (flip ? 1 : 1));
                    }
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.BLPsycopathAxe>()] < 1)
                {
                    const int max2 = 2;
                    Vector2 velocity = new Vector2(0f, -6f);
                    for (int y = 0; y < max2; y++)
                    {
                        Vector2 perturbedSpeed2 = velocity.RotatedBy(2 * Math.PI / max2 * y);
                        Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed2, ModContent.ProjectileType<Projectiles.Melee.BLPsycopathAxe>(), Item.damage + 10, 4f,
                            player.whoAmI, 0, (player.Center - 200 * Vector2.UnitX - player.position).Length() * (flip ? 1 : 1));
                    }
                }
            }
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            foreach (int ench in Enchants)
                recipe.AddIngredient(ench);

            recipe.AddTile(TileID.LunarCraftingStation);
            //recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
    }
}