using System;
using System.Collections;
using MeshDeformation.JobSystemDeformer;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace PerformanceTests
{
    public class MeshDeformationTests
    {
        [UnityTest, Performance]
        public IEnumerator SingleThreaded()
        {
            yield return StartTest("SingleThreaded");
        }

        [UnityTest, Performance]
        public IEnumerator JobSystem()
        {
            yield return StartTest("JobSystem");
        }

        [UnityTest, Performance]
        [TestCase(1, ExpectedResult = (IEnumerator) null)]
        [TestCase(2, ExpectedResult = (IEnumerator) null)]
        [TestCase(4, ExpectedResult = (IEnumerator) null)]
        [TestCase(8, ExpectedResult = (IEnumerator) null)]
        [TestCase(16, ExpectedResult = (IEnumerator) null)]
        [TestCase(32, ExpectedResult = (IEnumerator) null)]
        [TestCase(64, ExpectedResult = (IEnumerator) null)]
        [TestCase(128, ExpectedResult = (IEnumerator) null)]
        [TestCase(256, ExpectedResult = (IEnumerator) null)]
        [TestCase(512, ExpectedResult = (IEnumerator) null)]
        [TestCase(1024, ExpectedResult = (IEnumerator) null)]
        [TestCase(1048576, ExpectedResult = (IEnumerator) null)]
        public IEnumerator JobSystemBatchSize(int batchSize)
        {
            JobSystemModel.BatchSize = batchSize;
            yield return StartTest("JobSystem");
        }

        [UnityTest, Performance]
        public IEnumerator MeshData()
        {
            yield return StartTest("MeshData");
        }

        [UnityTest, Performance]
        public IEnumerator ComputeShader()
        {
            yield return StartTest("ComputeShader");
        }

        [UnityTest, Performance]
        public IEnumerator VertexShader()
        {
            yield return StartTest("VertexShader");
        }

        private static IEnumerator StartTest(string sceneName)
        {
            yield return LoadScene(sceneName);
            yield return RunTest();
            yield return UnloadScene(sceneName);
        }

        private static IEnumerator UnloadScene(string sceneName)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            yield return null;
        }

        private static IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
        }

        private static IEnumerator RunTest()
        {
            using (Measure.Frames().Scope())
            {
                yield return new WaitForSeconds(3);
            }
        }
    }
}