using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PowerUpTests
    {
        private PowerUpExtraLives powerUpExtraLives;

        [SetUp]
        public void Setup()
        {
            // AAA - Arrange
            powerUpExtraLives = ScriptableObject.CreateInstance("PowerUpExtraLives") as PowerUpExtraLives;
            powerUpExtraLives.playerLives = ScriptableObject.CreateInstance("IntVariable") as IntVariable;
            powerUpExtraLives.gameScore = ScriptableObject.CreateInstance("IntVariable") as IntVariable;
            powerUpExtraLives.playerLives.Value = 5;
            powerUpExtraLives.gameScore.Value = 0;
            powerUpExtraLives.score = ScriptableObject.CreateInstance("IntVariable") as IntVariable;
            powerUpExtraLives.score.Value = 1000;
            powerUpExtraLives.extraLives = 1;
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(powerUpExtraLives);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void UsePowerUpPayload_powerUpExtraLives_CheckValues()
        {
            int initialScore = powerUpExtraLives.gameScore.Value;
            int initialLives = powerUpExtraLives.playerLives.Value;

            // AAA - Act
            powerUpExtraLives.UsePowerUpPayload();

            // AAA - Assert
            Assert.AreEqual(initialScore + powerUpExtraLives.score.Value, powerUpExtraLives.gameScore.Value);
            Assert.AreEqual(initialLives + powerUpExtraLives.extraLives, powerUpExtraLives.playerLives.Value);
        }
    }
}
