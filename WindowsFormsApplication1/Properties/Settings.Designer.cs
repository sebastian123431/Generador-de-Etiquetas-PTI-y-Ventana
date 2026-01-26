using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace WindowsFormsApplication1.Properties
{
	// Token: 0x0200002F RID: 47
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000182 RID: 386 RVA: 0x000244FC File Offset: 0x000226FC
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x04000155 RID: 341
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
