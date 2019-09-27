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
        public IEnumerator initBrick_level1_ScoreEquals10()
        {
            // AAA - Act
            b.initBrick(1, false, null);

            // Use yield to skip a frame.
            yield return new WaitForSeconds(0.1f);

            // AAA - Assert
            Assert.AreEqual(10, b.score);
        }

        [UnityTest]
        [Timeout(3 * 60 * 1000)] // max time of 3 mins
        public IEnumerator initBrick_level7_ScoreEquals70()
        {
            // AAA - Act
            b.initBrick(7, false, null);

            // Use yield to skip a frame.
            yield return new WaitForSeconds(0.1f);

            // AAA - Assert
            Assert.AreEqual(70, b.score);
        }

        [UnityTest]
        [Timeout(3 * 60 * 1000)] // max time of 3 mins
        public IEnumerator initBrick_level8_ScoreEquals10()
        {
            // AAA - Act
            b.initBrick(8, false, null);

            // Use yield to skip a frame.
            yield return new WaitForSeconds(0.1f);

            // AAA - Assert
            Assert.AreEqual(10, b.score);
        }
    }
}
