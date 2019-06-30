// <copyright file="SpecBindGeneratorPlugin.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>
namespace SpecBind.Plugin
{
    using System;

    using TechTalk.SpecFlow.Generator.Plugins;
    using TechTalk.SpecFlow.Generator.UnitTestProvider;
    using TechTalk.SpecFlow.UnitTestProvider;

    /// <summary>
    /// The main plugin class for generating SpecBind needed attributes
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class SpecBindGeneratorPlugin : IGeneratorPlugin
    {
        /// <summary>
        /// Initializes the plugin to change the behavior of the generator
        /// </summary>
        /// <param name="generatorPluginEvents">The generator plugin events.</param>
        /// <param name="generatorPluginParameters">Parameters to the generator.</param>
        public void Initialize(GeneratorPluginEvents generatorPluginEvents, GeneratorPluginParameters generatorPluginParameters)
        {
            generatorPluginEvents.CustomizeDependencies += CustomizeDependencies;
        }

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