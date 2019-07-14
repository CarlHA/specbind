// <copyright file="ConfigurationFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using NUnit.Framework;
    using SpecBind.Configuration;

    /// <summary>
    /// Test classes for verifying configuration.
    /// </summary>
    [TestFixture]
    public class ConfigurationFixture
    {
         /// <summary>
        /// Tests that the ExcludedAssemblies property is populated if it is in the config file.
        /// </summary>
        [Test]
        public void TestLoadingExcludedAssemblies()
        {
            var fileMap = new ConfigurationBuilder().AddJsonFile("WithExcludedAssemblyConfig.json");
            var config = fileMap.Build();
            var section = config.GetSection("specBind").Get<SpecBindConfiguration>();

            Assert.IsNotNull(section);
            var assemblies = section.Application.ExcludedAssemblies.ToList();
            Assert.AreEqual(1, assemblies.Count);
            Assert.AreEqual("MyCoolApp, Version=1.2.3.0, Culture=neutral, PublicKeyToken=null", assemblies[0].Name);
        }
    }
}