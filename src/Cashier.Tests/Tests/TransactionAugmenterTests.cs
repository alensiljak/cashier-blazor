﻿using Cashier.Lib;
using Cashier.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.Tests
{
    public class TransactionAugmenterTests
    {
        private TransactionAugmenter _out;  // object under test

        public TransactionAugmenterTests()
        {
            _out = new TransactionAugmenter();
        }

        [Fact]
        public void TestCalculateEmptyPostingAmounts()
        {
            // Arrange
            var xacts = CreateDummyXactList();

            // Act
            _out.calculateEmptyPostingAmounts(xacts);

            // Assert
            Assert.NotNull(xacts);
            Assert.NotNull(xacts.First());
            Assert.NotNull(xacts.First().Postings);

            Assert.Equal(2, xacts.First().Postings!.Count);
            // The second posting should have the -25 EUR amount.
            var actual = xacts.First().Postings![1];
            Assert.Equivalent(new Money(-25, "EUR"), actual.Money);
        }

        [Fact]
        public void TestCalculateTxAmounts()
        {
            // Arrange
            var xacts = CreateDummyXactList();

            // Act
            var actual = _out.calculateTxAmounts(xacts);

            // Assert
            // Assert.NotNull(xacts);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.Equivalent(new Money(-25, "EUR"), actual.First());
        }

        /// <summary>
        /// Test the Xact balance calculation for a transfer.
        /// Test the 2 postings scenario.
        /// </summary>
        [Fact]
        public void TestCalculateTxAmountsForTransfer()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var xacts = new List<Xact>
            {
                new Xact(today, null, null, new List<Posting>
                {
                    new Posting("Assets:EUR", new Money(150, "EUR")),
                    new Posting("Assets:USD", new Money(100, "USD")),
                })
            };

            // Act
            var actual = _out.calculateTxAmounts(xacts);

            // Assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            // The amount is the first item.
            Assert.Equivalent(new Money(150, "EUR"), actual.First());
        }

        // Private
        private List<Xact> CreateDummyXactList()
        {
            var xact = new Xact(DateOnly.Parse("2023-11-26"), "Supermarket", null, new List<Posting> {
                new Posting("Expenses:Food", new Money(25, "EUR")),
                new Posting("Assets:Cash")
            });
            var xacts = new List<Xact> { xact };
            return xacts;
        }
    }
}
