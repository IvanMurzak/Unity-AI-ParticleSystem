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
using System;
using System.ComponentModel;
using com.IvanMurzak.McpPlugin;
using com.IvanMurzak.ReflectorNet.Utils;
using AIGD;
using com.IvanMurzak.Unity.MCP.Runtime.Extensions;
using com.IvanMurzak.Unity.MCP.Utils;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.ParticleSystem.Editor
{
    public partial class Tool_ParticleSystem
    {
        public const string ParticleSystemGetToolId = "particle-system-get";

        [AiTool
        (
            ParticleSystemGetToolId,
            Title = "ParticleSystem / Get",
            ReadOnlyHint = true,
            DestructiveHint = false,
            IdempotentHint = true,
            OpenWorldHint = false
        )]
        [AiSkillDescription("Inspect a `UnityEngine.ParticleSystem` component on a GameObject — runtime state " +
            "(playing/paused/emitting/stopped, particle count, time) plus opt-in serialized data for any of the " +
            "~24 particle modules (Main, Emission, Shape, Velocity, Noise, Collision, Trails, Renderer, etc.). " +
            "Pair with '" + ParticleSystemModifyToolId + "' to write changes back.")]
        [AiSkillBody("Inspect a `UnityEngine.ParticleSystem` component on a GameObject. Returns runtime state " +
            "(`isPlaying`, `isPaused`, `isEmitting`, `isStopped`, `particleCount`, `time`) plus opt-in serialized data " +
            "for any of the particle modules. Use '" + ParticleSystemModifyToolId + "' afterwards to write changes back.\n\n" +
            "## Inputs\n\n" +
            "- `gameObjectRef` — the GameObject hosting the `ParticleSystem` component (required).\n" +
            "- `componentRef` — optional. Resolves a specific `ParticleSystem` when the GameObject has more than " +
            "one; otherwise the first `ParticleSystem` found is used.\n" +
            "- `includeMain`, `includeEmission`, `includeShape`, `includeVelocityOverLifetime`, " +
            "`includeLimitVelocityOverLifetime`, `includeInheritVelocity`, `includeLifetimeByEmitterSpeed`, " +
            "`includeForceOverLifetime`, `includeColorOverLifetime`, `includeColorBySpeed`, " +
            "`includeSizeOverLifetime`, `includeSizeBySpeed`, `includeRotationOverLifetime`, " +
            "`includeRotationBySpeed`, `includeExternalForces`, `includeNoise`, `includeCollision`, " +
            "`includeTrigger`, `includeSubEmitters`, `includeTextureSheetAnimation`, `includeLights`, " +
            "`includeTrails`, `includeCustomData`, `includeRenderer` — per-module toggles. `includeMain` defaults " +
            "to `true`; everything else defaults to `false`.\n" +
            "- `includeAll` — when `true`, overrides every per-module flag and emits all modules.\n" +
            "- `deepSerialization` — when `true`, recurses through nested objects via ReflectorNet; otherwise only " +
            "top-level members are serialized (cheaper, smaller payload).\n\n" +
            "## Behavior\n\n" +
            "Only the modules whose include-flags resolve to `true` are serialized — this keeps responses small " +
            "when you only need one or two modules. `includeRenderer` reads the sibling " +
            "`UnityEngine.ParticleSystemRenderer` component on the same GameObject and is skipped silently when " +
            "no renderer is present. The whole call runs on the Unity main thread.")]
        [Description("Get detailed information about a ParticleSystem component on a GameObject. " +
            "Returns particle system state and optionally serialized data for each module. " +
            "Use the boolean flags to request specific modules. " +
            "Use this to inspect ParticleSystem data before modifying it.")]
        public GetParticleSystemResponse Get
        (
            [Description("Reference to the GameObject containing the ParticleSystem component.")]
            GameObjectRef gameObjectRef,

            [Description("Optional reference to a specific ParticleSystem component if the GameObject has multiple. " +
                "If not provided, uses the first ParticleSystem found.")]
            ComponentRef? componentRef = null,

            [Description("Include Main module data (duration, looping, prewarm, startDelay, startLifetime, startSpeed, startSize, startRotation, startColor, gravityModifier, simulationSpace, scalingMode, playOnAwake, maxParticles, etc.).")]
            bool includeMain = true,

            [Description("Include Emission module data (rateOverTime, rateOverDistance, bursts).")]
            bool includeEmission = false,

            [Description("Include Shape module data (shapeType, radius, angle, arc, position, rotation, scale, mesh, texture, etc.).")]
            bool includeShape = false,

            [Description("Include Velocity over Lifetime module data (x, y, z, space, orbital, radial, speedModifier).")]
            bool includeVelocityOverLifetime = false,

            [Description("Include Limit Velocity over Lifetime module data (limit, dampen, separateAxes, drag).")]
            bool includeLimitVelocityOverLifetime = false,

            [Description("Include Inherit Velocity module data (mode, curve).")]
            bool includeInheritVelocity = false,

            [Description("Include Lifetime by Emitter Speed module data (curve, range).")]
            bool includeLifetimeByEmitterSpeed = false,

            [Description("Include Force over Lifetime module data (x, y, z, space, randomized).")]
            bool includeForceOverLifetime = false,

            [Description("Include Color over Lifetime module data (color gradient).")]
            bool includeColorOverLifetime = false,

            [Description("Include Color by Speed module data (color, range).")]
            bool includeColorBySpeed = false,

            [Description("Include Size over Lifetime module data (size curve, separateAxes).")]
            bool includeSizeOverLifetime = false,

            [Description("Include Size by Speed module data (size, range).")]
            bool includeSizeBySpeed = false,

            [Description("Include Rotation over Lifetime module data (angular velocity, separateAxes).")]
            bool includeRotationOverLifetime = false,

            [Description("Include Rotation by Speed module data (angular velocity, range).")]
            bool includeRotationBySpeed = false,

            [Description("Include External Forces module data (multiplier, influenceFilter).")]
            bool includeExternalForces = false,

            [Description("Include Noise module data (strength, frequency, scrollSpeed, damping, octaves, quality, remap).")]
            bool includeNoise = false,

            [Description("Include Collision module data (type, mode, planes, dampen, bounce, lifetimeLoss).")]
            bool includeCollision = false,

            [Description("Include Trigger module data (inside, outside, enter, exit actions).")]
            bool includeTrigger = false,

            [Description("Include Sub Emitters module data (birth, collision, death, trigger, manual emitters).")]
            bool includeSubEmitters = false,

            [Description("Include Texture Sheet Animation module data (mode, tiles, animation, frameOverTime).")]
            bool includeTextureSheetAnimation = false,

            [Description("Include Lights module data (ratio, light, color, range, intensity).")]
            bool includeLights = false,

            [Description("Include Trails module data (mode, ratio, lifetime, width, color).")]
            bool includeTrails = false,

            [Description("Include Custom Data module data (modes, vectors, colors).")]
            bool includeCustomData = false,

            [Description("Include Renderer module data (renderMode, material, sortMode, alignment, shadows).")]
            bool includeRenderer = false,

            [Description("Include ALL modules data. Overrides individual flags.")]
            bool includeAll = false,

            [Description("Performs deep serialization including all nested objects. Otherwise, only serializes top-level members.")]
            bool deepSerialization = false
        )
        {
            if (gameObjectRef == null)
                throw new ArgumentNullException(nameof(gameObjectRef));

            if (!gameObjectRef.IsValid(out var gameObjectValidationError))
                throw new ArgumentException(gameObjectValidationError, nameof(gameObjectRef));

            return MainThread.Instance.Run(() =>
            {
                var go = gameObjectRef.FindGameObject(out var error);
                if (error != null)
                    throw new Exception(error);

                if (go == null)
                    throw new Exception("GameObject not found.");

                // Find the ParticleSystem component
                UnityEngine.ParticleSystem? ps = null;
                int psIndex = -1;

                var allComponents = go.GetComponents<UnityEngine.Component>();
                for (int i = 0; i < allComponents.Length; i++)
                {
                    var comp = allComponents[i] as UnityEngine.ParticleSystem;
                    if (comp == null)
                        continue;

                    if (componentRef != null && componentRef.IsValid(out _))
                    {
                        if (componentRef.Matches(allComponents[i], i))
                        {
                            ps = comp;
                            psIndex = i;
                            break;
                        }
                    }
                    else
                    {
                        // Use the first ParticleSystem found
                        ps = comp;
                        psIndex = i;
                        break;
                    }
                }

                if (ps == null)
                    throw new Exception("ParticleSystem component not found on the specified GameObject.");

                var response = new GetParticleSystemResponse
                {
                    gameObjectRef = new GameObjectRef(go),
                    componentRef = new ComponentRef(ps),
                    componentIndex = psIndex,
                    isPlaying = ps.isPlaying,
                    isPaused = ps.isPaused,
                    isEmitting = ps.isEmitting,
                    isStopped = ps.isStopped,
                    particleCount = ps.particleCount,
                    time = ps.time
                };

                var reflector = UnityMcpPluginEditor.Instance.Reflector ?? throw new Exception("Reflector not available.");
                var logger = UnityLoggerFactory.LoggerFactory.CreateLogger<Tool_ParticleSystem>();

                // Serialize requested modules
                if (includeAll || includeMain)
                {
                    var mainModule = ps.main;
                    response.main = reflector.Serialize(
                        obj: mainModule,
                        name: nameof(ps.main),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeEmission)
                {
                    var emissionModule = ps.emission;
                    response.emission = reflector.Serialize(
                        obj: emissionModule,
                        name: nameof(ps.emission),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeShape)
                {
                    var shapeModule = ps.shape;
                    response.shape = reflector.Serialize(
                        obj: shapeModule,
                        name: nameof(ps.shape),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeVelocityOverLifetime)
                {
                    var velocityModule = ps.velocityOverLifetime;
                    response.velocityOverLifetime = reflector.Serialize(
                        obj: velocityModule,
                        name: nameof(ps.velocityOverLifetime),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeLimitVelocityOverLifetime)
                {
                    var limitVelocityModule = ps.limitVelocityOverLifetime;
                    response.limitVelocityOverLifetime = reflector.Serialize(
                        obj: limitVelocityModule,
                        name: nameof(ps.limitVelocityOverLifetime),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeInheritVelocity)
                {
                    var inheritVelocityModule = ps.inheritVelocity;
                    response.inheritVelocity = reflector.Serialize(
                        obj: inheritVelocityModule,
                        name: nameof(ps.inheritVelocity),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeLifetimeByEmitterSpeed)
                {
                    var lifetimeByEmitterSpeedModule = ps.lifetimeByEmitterSpeed;
                    response.lifetimeByEmitterSpeed = reflector.Serialize(
                        obj: lifetimeByEmitterSpeedModule,
                        name: nameof(ps.lifetimeByEmitterSpeed),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeForceOverLifetime)
                {
                    var forceModule = ps.forceOverLifetime;
                    response.forceOverLifetime = reflector.Serialize(
                        obj: forceModule,
                        name: nameof(ps.forceOverLifetime),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeColorOverLifetime)
                {
                    var colorModule = ps.colorOverLifetime;
                    response.colorOverLifetime = reflector.Serialize(
                        obj: colorModule,
                        name: nameof(ps.colorOverLifetime),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeColorBySpeed)
                {
                    var colorBySpeedModule = ps.colorBySpeed;
                    response.colorBySpeed = reflector.Serialize(
                        obj: colorBySpeedModule,
                        name: nameof(ps.colorBySpeed),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeSizeOverLifetime)
                {
                    var sizeModule = ps.sizeOverLifetime;
                    response.sizeOverLifetime = reflector.Serialize(
                        obj: sizeModule,
                        name: nameof(ps.sizeOverLifetime),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeSizeBySpeed)
                {
                    var sizeBySpeedModule = ps.sizeBySpeed;
                    response.sizeBySpeed = reflector.Serialize(
                        obj: sizeBySpeedModule,
                        name: nameof(ps.sizeBySpeed),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeRotationOverLifetime)
                {
                    var rotationModule = ps.rotationOverLifetime;
                    response.rotationOverLifetime = reflector.Serialize(
                        obj: rotationModule,
                        name: nameof(ps.rotationOverLifetime),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeRotationBySpeed)
                {
                    var rotationBySpeedModule = ps.rotationBySpeed;
                    response.rotationBySpeed = reflector.Serialize(
                        obj: rotationBySpeedModule,
                        name: nameof(ps.rotationBySpeed),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeExternalForces)
                {
                    var externalForcesModule = ps.externalForces;
                    response.externalForces = reflector.Serialize(
                        obj: externalForcesModule,
                        name: nameof(ps.externalForces),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeNoise)
                {
                    var noiseModule = ps.noise;
                    response.noise = reflector.Serialize(
                        obj: noiseModule,
                        name: nameof(ps.noise),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeCollision)
                {
                    var collisionModule = ps.collision;
                    response.collision = reflector.Serialize(
                        obj: collisionModule,
                        name: nameof(ps.collision),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeTrigger)
                {
                    var triggerModule = ps.trigger;
                    response.trigger = reflector.Serialize(
                        obj: triggerModule,
                        name: nameof(ps.trigger),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeSubEmitters)
                {
                    var subEmittersModule = ps.subEmitters;
                    response.subEmitters = reflector.Serialize(
                        obj: subEmittersModule,
                        name: nameof(ps.subEmitters),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeTextureSheetAnimation)
                {
                    var textureSheetModule = ps.textureSheetAnimation;
                    response.textureSheetAnimation = reflector.Serialize(
                        obj: textureSheetModule,
                        name: nameof(ps.textureSheetAnimation),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeLights)
                {
                    var lightsModule = ps.lights;
                    response.lights = reflector.Serialize(
                        obj: lightsModule,
                        name: nameof(ps.lights),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeTrails)
                {
                    var trailsModule = ps.trails;
                    response.trails = reflector.Serialize(
                        obj: trailsModule,
                        name: nameof(ps.trails),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeCustomData)
                {
                    var customDataModule = ps.customData;
                    response.customData = reflector.Serialize(
                        obj: customDataModule,
                        name: nameof(ps.customData),
                        recursive: deepSerialization,
                        logger: logger
                    );
                }

                if (includeAll || includeRenderer)
                {
                    var rendererComponent = go.GetComponent<UnityEngine.ParticleSystemRenderer>();
                    if (rendererComponent != null)
                    {
                        response.renderer = reflector.Serialize(
                            obj: rendererComponent,
                            name: nameof(response.renderer),
                            recursive: deepSerialization,
                            logger: logger
                        );
                    }
                }

                return response;
            });
        }
    }
}
