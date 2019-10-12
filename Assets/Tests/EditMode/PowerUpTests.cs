using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PowerUpTests
    {
        private PowerUpAddInt powerUpExtraLives;

        [SetUp]
        public void Setup()
        {
            // AAA - Arrange
            powerUpExtraLives = ScriptableObject.CreateInstance("PowerUpAddInt") as PowerUpAddInt;
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
            int initialLives = powerUpExtraLives._intSetting.Value;

            // AAA - Act
            powerUpExtraLives.UsePowerUpPayload();

            // AAA - Assert
            Assert.AreEqual(initialLives + powerUpExtraLives._value, powerUpExtraLives._intSetting.Value);
        }
    }
}
