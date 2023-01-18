using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI.WebUI
{
	public static class Version
	{

		public const string VersionJs = "1.02";
		public const string VersionCss = "1.02";

		/// <summary>
		/// Force a reload of all js files for development, only active in DEBUG mode
		/// </summary>
		public const bool ForceContinuousReloadJs = true;

		/// <summary>
		/// Force a reload of all css files for development, only active in DEBUG mode
		/// </summary>
		public const bool ForceContinuousReloadCss = true;

        /// <summary>
        /// Global version control over includes to force reload where applied:
        ///
        /// E.g.:  <script src="/js/product/list.js?v=@Core.UI.WebUI.Version.JS" type="text/javascript"></script>
        ///
        /// </summary>
        // Force reloadevet 
        public static string JS
		{
			get
			{
#if DEBUG
				if (ForceContinuousReloadJs)
					return Guid.NewGuid().ToString();
#endif

				return VersionJs;
			}
		}

		public static string CSS
		{
			get
			{
#if DEBUG
				if (ForceContinuousReloadCss)
					return Guid.NewGuid().ToString();
#endif

				return VersionCss;
			}
		}
	}
}
