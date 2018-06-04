// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared
{
    /// <summary>
    ///     Serves as a base class for implementing Behavior-Driven Development
    /// </summary>
    /// <remarks>
    ///     Behavior-driven development (BDD) is based on test-driven development (TDD). TDD is a software development
    ///     methodology which essentially states that for each unit of software, a software developer must:
    ///     <ul>
    ///         <li>define a test set for the unit first;</li>
    ///         <li>then implement the unit;</li>
    ///         <li>>finally verify that the implementation of the unit makes the tests succeed.</li>
    ///     </ul>
    ///     TDD tends to get far into the detail, which doesn't resonate with non-technical individuals. BDD specifies that
    ///     tests of any unit of software should be specified in terms of the desired behavior of the unit.
    ///     BDD takes advantage of user stories to represent the intended behavior of components in a vertical slice,
    ///     experienced as a user or client would experience the behavior. This perspective keeps focus on and allows
    ///     the engineer to prioritize what is and isn't important to test.
    ///     User Stories represent needed conversations:
    ///     As a [role]
    ///     I want [some feature / behavior]
    ///     So that [some benefit / value]
    ///     Scenarios document the conversation took place, and what should be tested in BDD
    ///     Given [some initial state / context]
    ///     When [action occurs]
    ///     Then [this should be the result]
    ///     Example using an e-Commerce site concept:
    ///     As a Customer with Products in my Cart
    ///     I want to remove an Item from my Cart
    ///     so that I can change my mind about buying it
    ///     The scenario of the story breaks down into several elements:
    ///     <ul>
    ///         <li>A case where there are multiple items in the cart and the user removes one, leaving items in the cart</li>
    ///         <li>A case where there is a single item in the cart and the user removes one, leaving no items in the cart</li>
    ///         <li>Permutations of the above</li>
    ///     </ul>
    ///     BDD helps the clarification in the following way:
    ///     Given a Cart with multiple Items in it
    ///     When a single Item is removed
    ///     Then the other items are still in the Cart
    ///     Given a Cart with a single Item in it
    ///     When a single Item is removed
    ///     Then the Cart is empty
    ///     The _Then_ element can be compounded with multiple assertions. For example:
    ///     Given a Cart with a single Item in it
    ///     When a single Item is removed
    ///     Then the Cart is empty
    ///     And the Cart Subtotal is 0
    ///     Notice that the language contains nouns with capital letters (Cart, Item, etc.) to drive out a native domain
    ///     language
    ///     (see Evans Fowler (2003). Domain-Driven Design: Tackling complexity in the Heart of Software. Prentice Hall.).
    ///     The <see cref="ContextSpecification" />
    ///     class uses a variation of the Arrange, Act, Assert behavior to allow BDD to work without extra tools within MSTest.
    ///     Example with code
    ///     <code>
    ///
    ///  [TestClass]
    ///  public class When_adding_a_single_item_to_an_empty_cart : ContextSpecification
    ///  {
    ///      ICart _cart;
    ///
    ///      protected void override Given()
    ///      {
    ///          _cart = CartFactory.Create("TEST");
    ///          _cart.AddItem(ProductFactory.Create("SKU"));
    ///      }
    ///
    ///      protected void override When()
    ///      {
    ///          _cart.RemoveItem("SKU");
    ///      }
    ///
    ///      [TestMethod]
    ///      public void then_the_cart_is_empty()
    ///      {
    ///          Assert.AreEqual(1, _cart.TotalItems);
    ///      }
    ///
    ///      [TestMethod]
    ///      public void then_the_cart_subtotal_is_zero()
    ///      {
    ///          Assert.AreEqual(0d, _cart.Subtotal);
    ///      }
    ///  }
    ///  </code>
    ///     In the generated TRX file from MSTest, the test class and name are specified with the pass result:
    ///     <code>
    ///  ------------------------------------------------------------------------------------------------------
    ///    Result        |   Class name                                  |   Test Name
    ///  ------------------------------------------------------------------------------------------------------
    ///   Passed             When_adding_a_single_item_to_an_empty_cart      then_the_cart_is_empty
    ///   Passed             When_adding_a_single_item_to_an_empty_cart      then_the_cart_subtotal_is_zero
    ///  </code>
    ///     The TRX file is XML, and can be transformed to HTML to generate a report of capabilities of the system
    ///     automatically
    ///     during a build activity, yielding a "living spec".
    ///     More info on BDD:
    ///     http://en.wikipedia.org/wiki/Behavior-driven_development
    ///     http://channel9.msdn.com/Events/TechEd/NorthAmerica/2010/DPR302
    /// </remarks>
    [DebuggerStepThrough]
    public abstract class ContextSpecification
    {
        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            Cleanup();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            Given();
            // Act
            When();
        }

        protected virtual void Cleanup()
        {
        }

        protected virtual void Given()
        {
        }

        protected virtual void When()
        {
        }

        protected virtual void WriteLine(string message)
        {
#if DEBUG
            Debug.WriteLine(message);
#else
            TestContext.WriteLine(message);
#endif
        }

        protected virtual void WriteLine(string format, params object[] args)
        {
#if DEBUG
            Debug.WriteLine(format, args);
#else
            TestContext.WriteLine(format, args);
#endif
        }
    }
}