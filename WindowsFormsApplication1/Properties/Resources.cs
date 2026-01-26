using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace WindowsFormsApplication1.Properties
{
	// Token: 0x0200002E RID: 46
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00024485 File Offset: 0x00022685
		internal Resources()
		{
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00024490 File Offset: 0x00022690
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("WindowsFormsApplication1.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000244DC File Offset: 0x000226DC
		// (set) Token: 0x06000181 RID: 385 RVA: 0x000244F3 File Offset: 0x000226F3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x04000153 RID: 339
		private static ResourceManager resourceMan;

		// Token: 0x04000154 RID: 340
		private static CultureInfo resourceCulture;
	}
}
