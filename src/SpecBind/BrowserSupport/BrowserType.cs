// <copyright file="BrowserType.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.BrowserSupport
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumerates the various supported browsers.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
	public enum BrowserType
	{
		/// <summary>
		/// Chrome Browser
		/// </summary>
		Chrome = 3,

	    /// <summary>
	    /// Chrome Browser without a UI attached
	    /// </summary>
	    // ReSharper disable once InconsistentNaming
        ChromeHeadless = 11
	}
}