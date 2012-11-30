﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Moq.Protected;
using CustomerManagement.Model;
using CustomerManagement;
using CustomerManagement.DAL;



namespace CustomerManagementTest
{
    [TestFixture]
    public class CustomerTest
    {
        Customer customer;
        CustomerManagementService cs;
        string dbConnStr;

        [SetUp]
        public void TestSetup() {

            dbConnStr = buildConnectionString(CustomerManagementTest.Properties.Settings.Default.TestDb, () => DateTimeOffset.UtcNow.ToString("yyyy-MM-dd_hh:mm:ssZ"));



            customer = new Customer("type", 1);
            cs = new CustomerManagementService();

        }

        [TearDown]
        public void Teardown()
        {
            using (var dbCtx = new CustomerContext(dbConnStr))
            {
                if (!dbCtx.Database.Delete())
                    throw new Exception("Could not delete temporary test db with connection string: " + dbConnStr);
            }
        }

        [Test]
        public void TestModifyBalance()
        {
            customer.Balance = 12.00M;
            customer.ModifyBalance(5.00M);
            Assert.AreEqual(customer.Balance, 17.00M, "Balance is not updated");

        }

        [Test]
        public void TestRead()
        {

        }


        private string buildConnectionString(string connString, Func<string> randomGenerator)
        {
            var sb = new StringBuilder(connString);
            int posStartCatalog = connString.IndexOf("Initial Catalog");
            int posEndCatalog = connString.IndexOf(';', posStartCatalog);
            string oldDbName = connString.Substring(posStartCatalog, posEndCatalog - posStartCatalog);
            sb.Replace(oldDbName, string.Format("{0}_{1}", oldDbName, randomGenerator()));
            return sb.ToString();
        }


    }
}
