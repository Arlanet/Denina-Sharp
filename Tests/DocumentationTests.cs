﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeninaSharp.Core.Documentation;
using DeninaSharp.Core;
using System.Linq;
using System.Reflection;
using System.IO;
using DeninaSharp.Core.Filters;

namespace Tests
{
    [TestClass]
    public class DocumentationTests : BaseTests
    {
        [TestMethod]
        public void CorrectForCommand()
        {
            var filterAttributeOnXmlExtract = typeof(Xml).GetMethod("ExtractFromXml").GetCustomAttribute<FilterAttribute>();
            var codeSampleAttributes = typeof(Xml).GetMethod("ExtractFromXml").GetCustomAttributes<CodeSampleAttribute>();
            var argumentAttributes = typeof(Xml).GetMethod("ExtractFromXml").GetCustomAttributes<ArgumentMetaAttribute>();

            var resultingDocObject = Pipeline.CommandDocs.First(m => m.Key == "XML.Extract");

            Assert.AreEqual(filterAttributeOnXmlExtract.Name, resultingDocObject.Value.Name);
            Assert.AreEqual(filterAttributeOnXmlExtract.Description, resultingDocObject.Value.Description);
            Assert.AreEqual(codeSampleAttributes.Count(), resultingDocObject.Value.Samples.Count);
            Assert.AreEqual(argumentAttributes.Count(), resultingDocObject.Value.Arguments.Count);
        }

        [TestMethod]
        public void FilterDocLoadedEvent()
        {
            Pipeline.FilterDocLoading += (s, e) =>
            {
                e.CommandDoc.Name = "Foo";
            };
            Pipeline.Init();

            Assert.AreEqual("Foo", Pipeline.CommandDocs.First().Value.Name);
        }

        [TestMethod]
        public void CategoryDocLoadedEvent()
        {
            Pipeline.CategoryDocLoading += (s, e) =>
            {
                e.CategoryDoc.Name = "Foo";
            };
            Pipeline.Init();

            Assert.AreEqual("Foo", Pipeline.CategoryDocs.First().Value.Name);
        }
    }
}
