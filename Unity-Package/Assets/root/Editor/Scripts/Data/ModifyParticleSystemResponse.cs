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
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Runtime.Data;

namespace com.IvanMurzak.Unity.MCP.ParticleSystem.Editor
{
    /// <summary>
    /// Response model for Modify ParticleSystem tool.
    /// </summary>
    [Description("Response containing the result of modifying a ParticleSystem.")]
    public class ModifyParticleSystemResponse
    {
        [Description("Whether the modification was successful.")]
        public bool success;

        [Description("Reference to the GameObject containing the ParticleSystem component.")]
        public GameObjectRef? gameObjectRef;

        [Description("Reference to the modified ParticleSystem component.")]
        public ComponentRef? componentRef;

        [Description("Index of the ParticleSystem component in the GameObject's component list.")]
        public int componentIndex = -1;

        [Description("Log of modifications made and any warnings/errors encountered.")]
        public string[]? logs;
    }
}
