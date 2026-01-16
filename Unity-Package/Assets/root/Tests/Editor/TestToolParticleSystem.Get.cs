/*
┌─────────────────────────────────────────────────────────────────────────────┐
│  Author: Ivan Murzak (https://github.com/IvanMurzak)                        │
│  Repository: GitHub (https://github.com/IvanMurzak/Unity-AI-ParticleSystem) │
│  Copyright (c) 2025 Ivan Murzak                                             │
│  Licensed under the MIT License.                                            │
│  See the LICENSE file in the project root for more information.             │
└─────────────────────────────────────────────────────────────────────────────┘
*/

#nullable enable
using System.Collections;
using com.IvanMurzak.Unity.MCP.ParticleSystem.Editor.API;
using com.IvanMurzak.Unity.MCP.Runtime.Data;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace com.IvanMurzak.Unity.MCP.ParticleSystem.Editor.Tests
{
    public partial class TestToolParticleSystem : BaseTest
    {
        private const string GO_ParticleSystemName = "TestParticleSystem";

        #region Get Tool - Direct API Tests

        [UnityTest]
        public IEnumerator Get_WithInstanceID_ReturnsMainModule()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            Assert.IsNotNull(ps, "ParticleSystem component should exist");

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                includeMain: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.gameObjectRef, "GameObjectRef should not be null");
            Assert.IsNotNull(result.componentRef, "ComponentRef should not be null");
            Assert.IsNotNull(result.main, "Main module should not be null");
            Assert.IsTrue(result.componentIndex >= 0, "Component index should be valid");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_WithPath_ReturnsMainModule()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef { Path = GO_ParticleSystemName },
                includeMain: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.main, "Main module should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_WithName_ReturnsMainModule()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef { Name = GO_ParticleSystemName },
                includeMain: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.main, "Main module should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_IncludeEmission_ReturnsEmissionModule()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                includeMain: false,
                includeEmission: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNull(result.main, "Main module should be null when not requested");
            Assert.IsNotNull(result.emission, "Emission module should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_IncludeShape_ReturnsShapeModule()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                includeMain: false,
                includeShape: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNull(result.main, "Main module should be null when not requested");
            Assert.IsNotNull(result.shape, "Shape module should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_IncludeColorOverLifetime_ReturnsColorModule()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                includeMain: false,
                includeColorOverLifetime: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.colorOverLifetime, "ColorOverLifetime module should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_IncludeNoise_ReturnsNoiseModule()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                includeMain: false,
                includeNoise: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.noise, "Noise module should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_IncludeRenderer_ReturnsRendererModule()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                includeMain: false,
                includeRenderer: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.renderer, "Renderer module should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_IncludeAll_ReturnsAllModules()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                includeAll: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.main, "Main module should not be null");
            Assert.IsNotNull(result.emission, "Emission module should not be null");
            Assert.IsNotNull(result.shape, "Shape module should not be null");
            Assert.IsNotNull(result.velocityOverLifetime, "VelocityOverLifetime module should not be null");
            Assert.IsNotNull(result.colorOverLifetime, "ColorOverLifetime module should not be null");
            Assert.IsNotNull(result.sizeOverLifetime, "SizeOverLifetime module should not be null");
            Assert.IsNotNull(result.noise, "Noise module should not be null");
            Assert.IsNotNull(result.collision, "Collision module should not be null");
            Assert.IsNotNull(result.trails, "Trails module should not be null");
            Assert.IsNotNull(result.renderer, "Renderer module should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_ReturnsParticleSystemState()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();

            // Stop the particle system to have a predictable state
            ps.Stop(true, UnityEngine.ParticleSystemStopBehavior.StopEmittingAndClear);

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                includeMain: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.isPlaying, "ParticleSystem should not be playing after Stop");
            Assert.IsTrue(result.isStopped, "ParticleSystem should be stopped");
            Assert.AreEqual(0, result.particleCount, "Particle count should be 0 after clear");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_WithMultipleParticleSystems_FindsCorrectOne()
        {
            var go = new GameObject(GO_ParticleSystemName);
            var ps1 = go.AddComponent<UnityEngine.ParticleSystem>();
            var ps2 = go.AddComponent<UnityEngine.ParticleSystem>();

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                componentRef: new ComponentRef(ps2.GetInstanceID()),
                includeMain: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(ps2.GetInstanceID(), result.componentRef!.InstanceID, "Should find the correct ParticleSystem");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_WithComponentIndex_FindsCorrectParticleSystem()
        {
            var go = new GameObject(GO_ParticleSystemName);
            go.AddComponent<UnityEngine.ParticleSystem>();
            var ps2 = go.AddComponent<UnityEngine.ParticleSystem>();

            // Get index of ps2
            var components = go.GetComponents<UnityEngine.Component>();
            int ps2Index = -1;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == ps2)
                {
                    ps2Index = i;
                    break;
                }
            }

            var tool = new Tool_ParticleSystem();
            var result = tool.Get(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                componentRef: new ComponentRef { Index = ps2Index },
                includeMain: true
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(ps2.GetInstanceID(), result.componentRef!.InstanceID, "Should find the correct ParticleSystem by index");

            yield return null;
        }

        #endregion

        #region Get Tool - JSON API Tests

        [UnityTest]
        public IEnumerator GetJson_WithInstanceID_ReturnsMainModule()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""instanceID"": {go.GetInstanceID()}
                }},
                ""includeMain"": true
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemGetToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.Value, "Result value should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetJson_WithPath_ReturnsMainModule()
        {
            CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""path"": ""{GO_ParticleSystemName}""
                }},
                ""includeMain"": true
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemGetToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.Value, "Result value should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetJson_IncludeMultipleModules()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""instanceID"": {go.GetInstanceID()}
                }},
                ""includeMain"": true,
                ""includeEmission"": true,
                ""includeShape"": true
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemGetToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.Value, "Result value should not be null");

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetJson_IncludeAll()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""instanceID"": {go.GetInstanceID()}
                }},
                ""includeAll"": true
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemGetToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.Value, "Result value should not be null");

            yield return null;
        }

        #endregion
    }
}
