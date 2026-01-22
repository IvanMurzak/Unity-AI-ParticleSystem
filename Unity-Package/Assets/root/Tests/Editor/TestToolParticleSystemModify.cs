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
using com.IvanMurzak.ReflectorNet.Model;
using com.IvanMurzak.Unity.MCP.Runtime.Data;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace com.IvanMurzak.Unity.MCP.ParticleSystem.Editor.Tests
{
    public class TestToolParticleSystemModify : BaseTest
    {
        #region Modify Tool - Direct API Tests

        [UnityTest]
        public IEnumerator Modify_MainModule_Duration()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var mainModule = ps.main;
            var newDuration = 10.0f;

            // Create the main module diff with new duration
            // Note: Pass null as value to avoid serializing all module properties.
            // Only add the specific properties we want to modify.
            var mainDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.main),
                    type: typeof(UnityEngine.ParticleSystem.MainModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(mainModule.duration),
                    value: newDuration));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                main: mainDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newDuration, ps.main.duration, 0.001f, "Duration should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_MainModule_MaxParticles()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var mainModule = ps.main;
            var newMaxParticles = 500;

            var mainDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.main),
                    type: typeof(UnityEngine.ParticleSystem.MainModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(mainModule.maxParticles),
                    value: newMaxParticles));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                main: mainDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newMaxParticles, ps.main.maxParticles, "MaxParticles should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_MainModule_Loop()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var mainModule = ps.main;
            var originalLoop = mainModule.loop;
            var newLoop = !originalLoop;

            var mainDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.main),
                    type: typeof(UnityEngine.ParticleSystem.MainModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(mainModule.loop),
                    value: newLoop));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                main: mainDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newLoop, ps.main.loop, "Loop should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_EmissionModule_Enabled()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var emissionModule = ps.emission;
            var originalEnabled = emissionModule.enabled;
            var newEnabled = !originalEnabled;

            var emissionDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.emission),
                    type: typeof(UnityEngine.ParticleSystem.EmissionModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(emissionModule.enabled),
                    value: newEnabled));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                emission: emissionDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newEnabled, ps.emission.enabled, "Emission enabled should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_ShapeModule_Enabled()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var shapeModule = ps.shape;
            var originalEnabled = shapeModule.enabled;
            var newEnabled = !originalEnabled;

            var shapeDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.shape),
                    type: typeof(UnityEngine.ParticleSystem.ShapeModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(shapeModule.enabled),
                    value: newEnabled));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                shape: shapeDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newEnabled, ps.shape.enabled, "Shape enabled should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_ShapeModule_Radius()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var shapeModule = ps.shape;
            var newRadius = 5.0f;

            var shapeDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.shape),
                    type: typeof(UnityEngine.ParticleSystem.ShapeModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(shapeModule.radius),
                    value: newRadius));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                shape: shapeDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newRadius, ps.shape.radius, 0.001f, "Shape radius should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_NoiseModule_Enabled()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var noiseModule = ps.noise;
            var newEnabled = true;

            var noiseDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.noise),
                    type: typeof(UnityEngine.ParticleSystem.NoiseModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(noiseModule.enabled),
                    value: newEnabled));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                noise: noiseDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newEnabled, ps.noise.enabled, "Noise enabled should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_TrailsModule_Enabled()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var trailsModule = ps.trails;
            var newEnabled = true;

            var trailsDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.trails),
                    type: typeof(UnityEngine.ParticleSystem.TrailModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(trailsModule.enabled),
                    value: newEnabled));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                trails: trailsDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newEnabled, ps.trails.enabled, "Trails enabled should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_MultipleModules()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var mainModule = ps.main;
            var shapeModule = ps.shape;
            var noiseModule = ps.noise;

            var newDuration = 15.0f;
            var newShapeRadius = 3.0f;
            var noiseEnabled = true;

            var mainDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.main),
                    type: typeof(UnityEngine.ParticleSystem.MainModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(mainModule.duration),
                    value: newDuration));

            var shapeDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.shape),
                    type: typeof(UnityEngine.ParticleSystem.ShapeModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(shapeModule.radius),
                    value: newShapeRadius));

            var noiseDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.noise),
                    type: typeof(UnityEngine.ParticleSystem.NoiseModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(noiseModule.enabled),
                    value: noiseEnabled));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                main: mainDiff,
                shape: shapeDiff,
                noise: noiseDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newDuration, ps.main.duration, 0.001f, "Duration should be modified");
            Assert.AreEqual(newShapeRadius, ps.shape.radius, 0.001f, "Shape radius should be modified");
            Assert.AreEqual(noiseEnabled, ps.noise.enabled, "Noise enabled should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_WithPath_Works()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var mainModule = ps.main;
            var newDuration = 20.0f;

            var mainDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.main),
                    type: typeof(UnityEngine.ParticleSystem.MainModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(mainModule.duration),
                    value: newDuration));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef { Path = GO_ParticleSystemName },
                main: mainDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.success, "Modification should be successful");
            Assert.AreEqual(newDuration, ps.main.duration, 0.001f, "Duration should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Modify_ReturnsLogs()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();
            var reflector = McpPlugin.McpPlugin.Instance!.McpManager.Reflector;

            var mainModule = ps.main;

            var mainDiff = SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(ps.main),
                    type: typeof(UnityEngine.ParticleSystem.MainModule),
                    value: null)
                .AddProperty(SerializedMember.FromValue(
                    reflector: reflector,
                    name: nameof(mainModule.duration),
                    value: 5.0f));

            var tool = new Tool_ParticleSystem();
            var result = tool.Modify(
                gameObjectRef: new GameObjectRef(go.GetInstanceID()),
                main: mainDiff
            );

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.logs, "Logs should not be null");
            Assert.IsTrue(result.logs!.Length > 0, "Should have at least one log entry");

            yield return null;
        }

        #endregion

        #region Modify Tool - JSON API Tests

        [UnityTest]
        public IEnumerator ModifyJson_MainModule_Duration()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();

            var newDuration = 25.0f;

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""instanceID"": {go.GetInstanceID()}
                }},
                ""main"": {{
                    ""typeName"": ""UnityEngine.ParticleSystem+MainModule"",
                    ""props"": [
                        {{
                            ""name"": ""duration"",
                            ""typeName"": ""System.Single"",
                            ""value"": {newDuration}
                        }}
                    ]
                }}
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemModifyToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(newDuration, ps.main.duration, 0.001f, "Duration should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator ModifyJson_MainModule_MaxParticles()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();

            var newMaxParticles = 2000;

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""instanceID"": {go.GetInstanceID()}
                }},
                ""main"": {{
                    ""typeName"": ""UnityEngine.ParticleSystem+MainModule"",
                    ""props"": [
                        {{
                            ""name"": ""maxParticles"",
                            ""typeName"": ""System.Int32"",
                            ""value"": {newMaxParticles}
                        }}
                    ]
                }}
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemModifyToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(newMaxParticles, ps.main.maxParticles, "MaxParticles should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator ModifyJson_ShapeModule_Radius()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();

            var newRadius = 7.5f;

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""instanceID"": {go.GetInstanceID()}
                }},
                ""shape"": {{
                    ""typeName"": ""UnityEngine.ParticleSystem+ShapeModule"",
                    ""props"": [
                        {{
                            ""name"": ""radius"",
                            ""typeName"": ""System.Single"",
                            ""value"": {newRadius}
                        }}
                    ]
                }}
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemModifyToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(newRadius, ps.shape.radius, 0.001f, "Shape radius should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator ModifyJson_NoiseModule_Enable()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""instanceID"": {go.GetInstanceID()}
                }},
                ""noise"": {{
                    ""typeName"": ""UnityEngine.ParticleSystem+NoiseModule"",
                    ""props"": [
                        {{
                            ""name"": ""enabled"",
                            ""typeName"": ""System.Boolean"",
                            ""value"": true
                        }}
                    ]
                }}
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemModifyToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(ps.noise.enabled, "Noise should be enabled");

            yield return null;
        }

        [UnityTest]
        public IEnumerator ModifyJson_MultipleModules()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();

            var newDuration = 30.0f;
            var newRadius = 10.0f;

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""instanceID"": {go.GetInstanceID()}
                }},
                ""main"": {{
                    ""typeName"": ""UnityEngine.ParticleSystem+MainModule"",
                    ""props"": [
                        {{
                            ""name"": ""duration"",
                            ""typeName"": ""System.Single"",
                            ""value"": {newDuration}
                        }}
                    ]
                }},
                ""shape"": {{
                    ""typeName"": ""UnityEngine.ParticleSystem+ShapeModule"",
                    ""props"": [
                        {{
                            ""name"": ""radius"",
                            ""typeName"": ""System.Single"",
                            ""value"": {newRadius}
                        }}
                    ]
                }}
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemModifyToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(newDuration, ps.main.duration, 0.001f, "Duration should be modified");
            Assert.AreEqual(newRadius, ps.shape.radius, 0.001f, "Shape radius should be modified");

            yield return null;
        }

        [UnityTest]
        public IEnumerator ModifyJson_WithPath()
        {
            var go = CreateGameObjectWithParticleSystem(GO_ParticleSystemName);
            var ps = go.GetComponent<UnityEngine.ParticleSystem>();

            var newDuration = 35.0f;

            var json = $@"{{
                ""gameObjectRef"": {{
                    ""path"": ""{GO_ParticleSystemName}""
                }},
                ""main"": {{
                    ""typeName"": ""UnityEngine.ParticleSystem+MainModule"",
                    ""props"": [
                        {{
                            ""name"": ""duration"",
                            ""typeName"": ""System.Single"",
                            ""value"": {newDuration}
                        }}
                    ]
                }}
            }}";

            var result = RunToolAllowWarnings(Tool_ParticleSystem.ParticleSystemModifyToolId, json);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(newDuration, ps.main.duration, 0.001f, "Duration should be modified");

            yield return null;
        }

        #endregion
    }
}
