using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using abc_bank;
using System.Threading;

namespace abc_bank_tests
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void TestApp()
        {
            AbstractAccount checkingAccount = new CHECKINGAccount();// Account(Account.CHECKING);
            AbstractAccount savingsAccount = new SAVINGSAccount();// Account(Account.SAVINGS);

            Customer henry = new Customer("Henry").OpenAccount(checkingAccount).OpenAccount(savingsAccount);

            checkingAccount.Deposit(100.0);
            savingsAccount.Deposit(4000.0);
            savingsAccount.Withdraw(200.0);

            Assert.AreEqual("Statement for Henry\n" +
                    "\n" +
                    "Checking Account\n" +
                    "  deposit $100.00\n" +
                    "Total $100.00\n" +
                    "\n" +
                    "Savings Account\n" +
                    "  deposit $4,000.00\n" +
                    "  withdrawal $200.00\n" +
                    "Total $3,800.00\n" +
                    "\n" +
                    "Total In All Accounts $3,900.00", henry.GetStatement());
        }

        [TestMethod]
        public void TestOneAccount()
        {
            Customer oscar = new Customer("Oscar").OpenAccount(new SAVINGSAccount());// Account(Account.SAVINGS));
            Assert.AreEqual(1, oscar.GetNumberOfAccounts());
        }

        [TestMethod]
        public void TestTwoAccount()
        {
            Customer oscar = new Customer("Oscar")
                 .OpenAccount(new SAVINGSAccount());// Account(Account.SAVINGS));
            oscar.OpenAccount(new CHECKINGAccount());// Account(Account.CHECKING));
            Assert.AreEqual(2, oscar.GetNumberOfAccounts());
        }

        [TestMethod]
        //[Ignore]
        public void TestThreeAccounts()
        {
            Customer oscar = new Customer("Oscar")
                    .OpenAccount(new SAVINGSAccount());// Account(Account.SAVINGS));
            oscar.OpenAccount(new CHECKINGAccount());// Account(Account.CHECKING));
            oscar.OpenAccount(new MAXI_SAVINGSAccount());
            Assert.AreEqual(3, oscar.GetNumberOfAccounts());
        }


        [TestMethod]
        public void TestTransferFromChckingToSavings()
        {
            AbstractAccount checkingAccount = new CHECKINGAccount();// Account(Account.CHECKING);
            AbstractAccount savingsAccount = new SAVINGSAccount();// Account(Account.SAVINGS);

            Customer henry = new Customer("Bob").OpenAccount(checkingAccount).OpenAccount(savingsAccount);

            checkingAccount.Deposit(1000.0);
            savingsAccount.Deposit(100.0);

            bool result = henry.Transfer(checkingAccount, savingsAccount, 300);


            Assert.AreEqual(bool.TrueString, result.ToString());

            Assert.AreEqual(700, checkingAccount.sumTransactions());

            Assert.AreEqual(400, savingsAccount.sumTransactions());
        }

        [TestMethod]
        public void TestTransferWhenNotEnoughMoney()
        {
            AbstractAccount checkingAccount = new CHECKINGAccount();// Account(Account.CHECKING);
            AbstractAccount savingsAccount = new SAVINGSAccount();// Account(Account.SAVINGS);

            Customer rich = new Customer("Rich").OpenAccount(checkingAccount).OpenAccount(checkingAccount);
            rich.OpenAccount(checkingAccount).OpenAccount(savingsAccount);

            checkingAccount.Deposit(10);
            savingsAccount.Deposit(5);

            bool result = rich.Transfer(checkingAccount, savingsAccount, 20);

            Assert.AreEqual(bool.FalseString, result.ToString());

            Assert.AreEqual(10, checkingAccount.sumTransactions());

            Assert.AreEqual(5, savingsAccount.sumTransactions());
        }
    }
}
