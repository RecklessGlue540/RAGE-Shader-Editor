using System;
using System.IO;

namespace ShaderEditor
{
	internal class Settings
	{
		public static int game;
		public static int shaderExportModePC;
		public static int shaderExportModeXenon;
		public static int platform;
		
		public static void CheckShaderExport()
		{
			shaderExportModePC = 1;

            if (!File.Exists($"{Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}\\fx.dll"))
            {
                throw new Exception("fx.dll not found");
            }
		}
	}
}
