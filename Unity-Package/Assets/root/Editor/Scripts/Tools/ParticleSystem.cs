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
using com.IvanMurzak.McpPlugin;
using com.IvanMurzak.Unity.MCP.Runtime.Data;

namespace com.IvanMurzak.Unity.MCP.ParticleSystem.Editor.API
{
    [McpPluginToolType]
    public partial class Tool_ParticleSystem
    {
        public static class Error
        {
            public static string GameObjectNotFound()
                => "GameObject not found.";

            public static string ParticleSystemNotFound()
                => "ParticleSystem component not found on the GameObject.";

            public static string InvalidGameObjectRef()
                => "Invalid GameObjectRef. Provide instanceID, path, or name.";

            public static string InvalidComponentRef()
                => "Invalid ComponentRef. Provide instanceID, index, or typeName.";

            public static string NoDataProvided()
                => "No data provided to modify.";
        }
    }
}
