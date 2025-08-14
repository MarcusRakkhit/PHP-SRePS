using NUnit.Framework;

namespace ValidationTest
{
    public class FunctionTests
    {
        // Fields

        private Functions testInput;

        [SetUp]
        public void Setup()
        {
            testInput = new Functions();
        }

        // Valid Input

        [Test]
        public void ValidStockInput()
        {
            testInput.item = "Test";
            testInput.quantity = "1";
            testInput.category = "Test";
            testInput.price = "1";
            Assert.IsTrue(testInput.CheckForValidStockInput());
        }

        [Test]
        public void ValidSalesInput()
        {
            testInput.productID = "1";
            testInput.orderID = "1";
            testInput.quantity = "1";
            Assert.IsTrue(testInput.CheckForValidSaleInput());
        }

        // Item Validation

        [Test]
        public void EmptyItem()
        {
            testInput.item = "";
            testInput.quantity = "1";
            testInput.category = "Test";
            testInput.price = "1";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        // Category Validation

        [Test]
        public void EmptyCategory()
        {
            testInput.item = "Test";
            testInput.quantity = "1";
            testInput.category = "";
            testInput.price = "1";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        // Item Quantity Validation

        [Test]
        public void EmptyItemQuantity()
        {
            testInput.item = "Test";
            testInput.quantity = "1";
            testInput.category = "Test";
            testInput.price = "";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        [Test]
        public void ItemQuantityIsString()
        {
            testInput.item = "Test";
            testInput.quantity = "Test";
            testInput.category = "Test";
            testInput.price = "1";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        [Test]
        public void ItemQuantityIs0()
        {
            testInput.item = "Test";
            testInput.quantity = "0";
            testInput.category = "Test";
            testInput.price = "1";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        public void ItemQuantityIsNegative()
        {
            testInput.item = "Test";
            testInput.quantity = "0";
            testInput.category = "Test";
            testInput.price = "-1";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        // Price Validation

        [Test]
        public void EmptyPrice()
        {
            testInput.item = "Test";
            testInput.quantity = "1";
            testInput.category = "Test";
            testInput.price = "";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        [Test]
        public void PriceIsString()
        {
            testInput.item = "Test";
            testInput.quantity = "1";
            testInput.category = "Test";
            testInput.price = "Test";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        public void PriceIs0()
        {
            testInput.item = "Test";
            testInput.quantity = "1";
            testInput.category = "Test";
            testInput.price = "0";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        [Test]
        public void PriceIsNegative()
        {
            testInput.item = "Test";
            testInput.quantity = "1";
            testInput.category = "Test";
            testInput.price = "-1";
            Assert.IsFalse(testInput.CheckForValidStockInput());
        }

        // ProductID Validation

        [Test]
        public void EmptyProductID()
        {
            testInput.productID = "";
            testInput.orderID = "1";
            testInput.quantity = "1";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        [Test]
        public void ProductIDIsString()
        {
            testInput.productID = "Test";
            testInput.orderID = "1";
            testInput.quantity = "1";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        [Test]
        public void ProductIDIs0()
        {
            testInput.productID = "0";
            testInput.orderID = "1";
            testInput.quantity = "1";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        [Test]
        public void ProductIDIsNegative()
        {
            testInput.productID = "-1";
            testInput.orderID = "1";
            testInput.quantity = "1";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        // OrderID Validation

        [Test]
        public void EmptyOrderID()
        {
            testInput.productID = "1";
            testInput.orderID = "";
            testInput.quantity = "1";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        [Test]
        public void OrderIDIsString()
        {
            testInput.productID = "1";
            testInput.orderID = "Test";
            testInput.quantity = "1";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        [Test]
        public void OrderIDIs0()
        {
            testInput.productID = "1";
            testInput.orderID = "0";
            testInput.quantity = "1";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        [Test]
        public void OrderIDIsNegative()
        {
            testInput.productID = "1";
            testInput.orderID = "-1";
            testInput.quantity = "1";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        // Sales Quantity Validation

        [Test]
        public void EmptySalesQuantity()
        {
            testInput.productID = "1";
            testInput.orderID = "1";
            testInput.quantity = "";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        [Test]
        public void SalesQuantityIsString()
        {
            testInput.productID = "1";
            testInput.orderID = "1";
            testInput.quantity = "Test";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        [Test]
        public void SalesQuantityIs0()
        {
            testInput.productID = "1";
            testInput.orderID = "1";
            testInput.quantity = "0";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }

        [Test]
        public void SalesQuantityIsNegative()
        {
            testInput.productID = "1";
            testInput.orderID = "1";
            testInput.quantity = "-1";
            Assert.IsFalse(testInput.CheckForValidSaleInput());
        }
    }
}