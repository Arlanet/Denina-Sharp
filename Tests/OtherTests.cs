﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeninaSharp.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeninaSharp.Core.Filters;

namespace Tests
{
    [TestClass]
    public class OtherTests : BaseTests
    {
        [TestMethod]
        public void DebugData()
        {
            var pipeline = new Pipeline();
            pipeline.AddCommand("Text.Prepend -prefix:Deane");
            pipeline.AddCommand("Text.Append -suffix:Barker");
            pipeline.Execute();

            Assert.AreEqual(pipeline.LogEntries.Count, 2);
            Assert.AreEqual(pipeline.LogEntries.First().InputValue.Length, 0);
            Assert.AreEqual(pipeline.LogEntries.First().OutputValue.Length, 5);
            Assert.AreEqual(pipeline.LogEntries.Last().InputValue.Length, 5);
            Assert.AreEqual(pipeline.LogEntries.Last().OutputValue.Length, 11);
        }

        [TestMethod]
        public void RemoveFilter()
        {
            Assert.IsTrue(Pipeline.CommandMethods.ContainsKey("text.append"));
            Pipeline.RemoveCommand("text.append", "Never append text to anything!!!");
            Assert.IsFalse(Pipeline.CommandMethods.ContainsKey("text.append"));

            var pipeline = new Pipeline();
            pipeline.AddCommand("Text.Append -prefix:foo");

            try
            {
                pipeline.Execute("bar");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("!!!"));
            }

            Pipeline.ReflectMethod(typeof(Text).GetMethod("Append"), "text", "append");
        }


        [TestMethod]
        public void RemoveFilterCategory()
        {
            Assert.IsTrue(Pipeline.CommandMethods.ContainsKey("text.append"));
            Pipeline.RemoveCommandCategory("text");
            Assert.IsFalse(Pipeline.CommandMethods.ContainsKey("text.append"));
        }
    }
}
