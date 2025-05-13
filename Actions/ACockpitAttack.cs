using System.Collections.Generic;

namespace VionheartSweetroll.Actions;

public class ACockpitAttack : AAttack
{
    public override void Begin(G g, State s, Combat c)
    {
		Ship ship = targetPlayer ? s.ship : c.otherShip;
		Ship ship2 = targetPlayer ? c.otherShip : s.ship;
		if (ship == null || ship2 == null || ship.hull <= 0 || (fromDroneX.HasValue && !c.stuff.ContainsKey(fromDroneX.Value)))
		{
			return;
		}
		int? num = GetFromXCockpit(s, c);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        RaycastResult raycastResult = fromDroneX.HasValue ? CombatUtils.RaycastGlobal(c, ship, fromDrone: true, fromDroneX.Value) : (num.HasValue ? CombatUtils.RaycastFromShipLocal(s, c, num.Value, targetPlayer) : null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        bool flag = ship2.Get(Status.libra) > 0 && !fromDroneX.HasValue;
		if (!targetPlayer && !fromDroneX.HasValue && g.state.ship.GetPartTypeCount(PType.cannon) > 1 && !multiCannonVolley)
		{
			c.QueueImmediate(new AVolleyAttackFromAllCannons
			{
				attack = Mutil.DeepCopy(this)
			});
			timer = 0.0;
		}
		else
		{
			if (raycastResult != null && ApplyAutododge(c, ship, raycastResult))
			{
				return;
			}
			if (!targetPlayer && !fromDroneX.HasValue)
			{
				foreach (Artifact item in s.EnumerateAllArtifacts())
				{
					if (item.OnPlayerAttackMakeItPierce(s, c) == true)
					{
						piercing = true;
						item.Pulse();
					}
				}
				foreach (Artifact item2 in s.EnumerateAllArtifacts())
				{
					if (item2.ModifyAttacksToStun(s, c) == true)
					{
						stunEnemy = true;
						item2.Pulse();
					}
				}
			}
			if (!targetPlayer && fromDroneX.HasValue)
			{
				foreach (Artifact item3 in s.EnumerateAllArtifacts())
				{
					if (item3.OnDroneAttackEnemyMakeItPierce(s, c) == true)
					{
						piercing = true;
						item3.Pulse();
					}
				}
			}
			if (!stunEnemy && s.ship.Get(Status.stunCharge) > 0 && !targetPlayer && !fromDroneX.HasValue)
			{
				s.ship.Set(Status.stunCharge, s.ship.Get(Status.stunCharge) - 1);
				stunEnemy = true;
			}
			if (!fromDroneX.HasValue && !targetPlayer && !multiCannonVolley)
			{
				c.QueueImmediate(new AJupiterShoot
				{
					attackCopy = Mutil.DeepCopy(this)
				});
			}
			if (raycastResult == null)
			{
				timer = 0.0;
				if (flag)
				{
					DoLibraEffect(c, ship2);
				}
				{
					foreach (Artifact item4 in s.EnumerateAllArtifacts())
					{
						item4.OnPlayerAttack(s, c);
					}
					return;
				}
			}
			if (raycastResult.hitDrone)
			{
				bool flag2 = c.stuff[raycastResult.worldX].Invincible();
				foreach (Artifact item5 in s.EnumerateAllArtifacts())
				{
					if (item5.ModifyDroneInvincibility(s, c, c.stuff[raycastResult.worldX]) == true)
					{
						flag2 = true;
						item5.Pulse();
					}
				}
				if (c.stuff[raycastResult.worldX].bubbleShield && !piercing)
				{
					c.stuff[raycastResult.worldX].bubbleShield = false;
				}
				else if (flag2)
				{
					c.QueueImmediate(c.stuff[raycastResult.worldX].GetActionsOnShotWhileInvincible(s, c, !targetPlayer, damage));
				}
				else
				{
					c.DestroyDroneAt(s, raycastResult.worldX, !targetPlayer);
				}
			}
			timer = (fast ? 0.2 : 0.4);
			DamageDone dmg = new DamageDone();
			if (raycastResult.hitShip)
			{
				dmg = (isBeam ? new DamageDone
				{
					hitHull = true,
					hitShield = false,
					poppedShield = false
				} : ship.NormalDamage(s, c, damage, raycastResult.worldX, piercing));
				Part? partAtWorldX = ship.GetPartAtWorldX(raycastResult.worldX);
				if (partAtWorldX != null && partAtWorldX.stunModifier == PStunMod.stunnable)
				{
					stunEnemy = true;
				}
				if (!isBeam && (ship.Get(Status.payback) > 0 || ship.Get(Status.tempPayback) > 0) && paybackCounter < 100)
				{
					c.QueueImmediate(new AAttack
					{
						paybackCounter = paybackCounter + 1,
						damage = Card.GetActualDamage(s, ship.Get(Status.payback) + ship.Get(Status.tempPayback), !targetPlayer),
						targetPlayer = !targetPlayer,
						fast = true,
						storyFromPayback = true
					});
				}
				if (moveEnemy != 0)
				{
					c.QueueImmediate(new AMove
					{
						dir = moveEnemy,
						targetPlayer = targetPlayer
					});
				}
				if (status.HasValue)
				{
					c.QueueImmediate(new AStatus
					{
						status = status.Value,
						statusAmount = statusAmount,
						targetPlayer = targetPlayer
					});
				}
				if (givesEnergy.HasValue && targetPlayer)
				{
					c.QueueImmediate(new AEnergy
					{
						changeAmount = 1
					});
				}
				if (cardOnHit != null)
				{
					c.QueueImmediate(new AAddCard
					{
						card = Mutil.DeepCopy(cardOnHit),
						destination = destination
					});
				}
				if (stunEnemy && !targetPlayer)
				{
					c.QueueImmediate(new AStunPart
					{
						worldX = raycastResult.worldX
					});
				}
				if (weaken)
				{
					c.QueueImmediate(new AWeaken
					{
						worldX = raycastResult.worldX,
						targetPlayer = targetPlayer
					});
				}
				if (brittle)
				{
					c.QueueImmediate(new ABrittle
					{
						worldX = raycastResult.worldX,
						targetPlayer = targetPlayer
					});
				}
				if (ship.Get(Status.reflexiveCoating) >= 1)
				{
					c.QueueImmediate(new AArmor
					{
						worldX = raycastResult.worldX,
						targetPlayer = targetPlayer,
						justTheActiveOverride = true
					});
				}
				if (armorize)
				{
					c.QueueImmediate(new AArmor
					{
						worldX = raycastResult.worldX,
						targetPlayer = targetPlayer
					});
				}
			}
			if (flag)
			{
				DoLibraEffect(c, ship2);
			}
			if (!isBeam && !targetPlayer && !fromDroneX.HasValue)
			{
				Input.Rumble(0.5);
			}
			if (!isBeam)
			{
				if (targetPlayer)
				{
					if (!raycastResult.hitShip && !raycastResult.hitDrone)
					{
						g.state.storyVars.enemyShotJustMissed = true;
					}
					if (raycastResult.hitShip)
					{
						g.state.storyVars.enemyShotJustHit = true;
					}
					foreach (Artifact item6 in s.EnumerateAllArtifacts())
					{
						item6.OnEnemyAttack(s, c);
					}
					if (!raycastResult.hitShip && !raycastResult.hitDrone)
					{
						foreach (Artifact item7 in s.EnumerateAllArtifacts())
						{
							item7.OnPlayerDodgeHit(s, c);
						}
					}
				}
				else
				{
					if (raycastResult.hitDrone)
					{
						g.state.storyVars.playerJustShotAMidrowObject = true;
					}
					if (!raycastResult.hitShip && !raycastResult.hitDrone)
					{
						g.state.storyVars.playerShotJustMissed = true;
					}
					else
					{
						g.state.storyVars.playerShotJustMissed = false;
					}
					if (raycastResult.hitShip)
					{
						g.state.storyVars.playerShotJustHit = true;
					}
					g.state.storyVars.playerShotWasFromStrafe = storyFromStrafe;
					g.state.storyVars.playerShotWasFromPayback = storyFromPayback;
					if (!fromDroneX.HasValue)
					{
						foreach (Artifact item8 in s.EnumerateAllArtifacts())
						{
							item8.OnPlayerAttack(s, c);
						}
					}
					if (raycastResult.hitShip)
					{
						if (c.otherShip.ai != null)
						{
							c.otherShip.ai.OnHitByAttack(s, c, raycastResult.worldX, this);
						}
						foreach (Artifact item9 in s.EnumerateAllArtifacts())
						{
							item9.OnEnemyGetHit(s, c, c.otherShip.GetPartAtWorldX(raycastResult.worldX));
						}
					}
					if (!raycastResult.hitShip && !raycastResult.hitDrone && !fromDroneX.HasValue)
					{
						foreach (Artifact item10 in s.EnumerateAllArtifacts())
						{
							item10.OnEnemyDodgePlayerAttack(s, c);
						}
					}
					if (!raycastResult.hitShip && !raycastResult.hitDrone)
					{
						bool flag3 = false;
						for (int i = -1; i <= 1; i += 2)
						{
							if (CombatUtils.RaycastGlobal(c, ship, fromDrone: true, raycastResult.worldX + i).hitShip)
							{
								flag3 = true;
							}
						}
						if (flag3)
						{
							foreach (Artifact item11 in s.EnumerateAllArtifacts())
							{
								item11.OnEnemyDodgePlayerAttackByOneTile(s, c);
							}
						}
					}
				}
				if (ship.hull <= 0 && onKillActions != null)
				{
					List<CardAction> list = Mutil.DeepCopy(onKillActions);
					foreach (CardAction item12 in list)
					{
						item12.canRunAfterKill = true;
					}
					c.QueueImmediate(list);
				}
			}
			if (fromDroneX.HasValue)
			{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                c.stuff.TryGetValue(fromDroneX.Value, out StuffBase value);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (value is AttackDrone attackDrone)
				{
					attackDrone.pulse = 1.0;
				}
				if (value is EnergyDrone energyDrone)
				{
					energyDrone.pulse = 1.0;
				}
				if (value is ShieldDrone shieldDrone)
				{
					shieldDrone.pulse = 1.0;
				}
				if (value is JupiterDrone jupiterDrone)
				{
					jupiterDrone.pulse = 1.0;
				}
				if (value is DualDrone dualDrone)
				{
					dualDrone.pulse = 1.0;
				}
			}
			else
			{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8629 // Nullable value type may be null.
                Part partAtLocalX = ship2.GetPartAtLocalX(num.Value);
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (partAtLocalX != null)
				{
					partAtLocalX.pulse = 1.0;
				}
			}
			EffectSpawner.Cannon(g, targetPlayer, raycastResult, dmg, isBeam);
		}
	}

    private int? GetFromXCockpit(State s, Combat c)
	{
		if (fromX.HasValue)
		{
			return fromX;
		}
		int num = (targetPlayer ? c.otherShip : s.ship).parts.FindIndex((Part p) => p.type == PType.cockpit && p.active);
		if (num != -1)
		{
			return num;
		}
		return null;
	}
}