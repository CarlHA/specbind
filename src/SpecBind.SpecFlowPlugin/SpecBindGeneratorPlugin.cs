﻿// <copyright file="SpecBindGeneratorPlugin.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>

using SpecBind.Plugin;
using TechTalk.SpecFlow.Infrastructure;

[assembly: GeneratorPlugin(typeof(SpecBindGeneratorPlugin))]

namespace SpecBind.Plugin
{
    using TechTalk.SpecFlow.Generator.Plugins;
    using TechTalk.SpecFlow.UnitTestProvider;

    /// <summary>
    /// The main plugin class for generating SpecBind needed attributes
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class SpecBindGeneratorPlugin : IGeneratorPlugin
    {
        private static void CustomizeDependencies(object sender, CustomizeDependenciesEventArgs eventArgs)
        {
            var container = eventArgs.ObjectContainer;

            container.RegisterTypeAs<SpecBindConfigurationProvider, ISpecBindConfigurationProvider>();

        }

        public void Initialize(GeneratorPluginEvents generatorPluginEvents,
                               GeneratorPluginParameters generatorPluginParameters,
                               UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            generatorPluginEvents.CustomizeDependencies += CustomizeDependencies;
        }
    }
}