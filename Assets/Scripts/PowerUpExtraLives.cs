using UnityEngine;
using System.Collections;

public class PowerUpExtraLives : PowerUp
{
	public int extraLives = 1;

	protected override void UsePowerUpPayload()
	{
		base.UsePowerUpPayload();

		// Payload is to add lives
		gc.GainLife(extraLives);
	}
}
