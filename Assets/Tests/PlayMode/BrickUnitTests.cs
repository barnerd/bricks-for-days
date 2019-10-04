using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BrickUnitTests
    {
        private GameObject go;
        private Brick b;

        [SetUp]
        public void Setup()
        {
            // AAA - Arrange
            /*go = new GameObject();
            go.AddComponent<Brick>();
            b = go.GetComponent<Brick>(); //*/

            GameObject prefab = Resources.Load("Prefabs/Brick") as GameObject;

            Debug.Log(prefab);
            go = Object.Instantiate(prefab) as GameObject;
            b = go.GetComponent<Brick>();//*/
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(go.gameObject);
        }

        // A Test behaves as an ordinary method
        /*
        [Test]
        public void NewTestScriptSimplePasses()
        {
            // Use the Assert class to test conditions
        }
        */

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        [Timeout(3 * 60 * 1000)] // max time of 3 mins
        public IEnumerator initBrick_level1noPowerUp_CheckValues()
        {
            // AAA - Act
            b.SetLevel(1);

            // Use yield to skip a frame.
            yield return null;

            // AAA - Assert
            Assert.AreEqual(1, b.level);
            Assert.AreEqual(1, b.score);
        }

        [UnityTest]
        [Timeout(3 * 60 * 1000)] // max time of 3 mins
        public IEnumerator initBrick_level7noPowerUp_CheckValues()
        {
            // AAA - Act
            b.SetLevel(7);

            // Use yield to skip a frame.
            yield return null;

            // AAA - Assert
            Assert.AreEqual(7, b.level);
            Assert.AreEqual(7, b.score);
        }

        [UnityTest]
        [Timeout(3 * 60 * 1000)] // max time of 3 mins
        public IEnumerator initBrick_level8noPowerUp_CheckValues()
        {
            // AAA - Act
            b.SetLevel(8);

            // Use yield to skip a frame.
            yield return null;

            // AAA - Assert
            Assert.AreEqual(1, b.level);
            Assert.AreEqual(1, b.score);
        }
    }
}
