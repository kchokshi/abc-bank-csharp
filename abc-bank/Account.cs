using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace abc_bank
{
    abstract public class AbstractAccount
    {

        public const int CHECKING = 0;
        public const int SAVINGS = 1;
        public const int MAXI_SAVINGS = 2;

        //private readonly int accountType;
        //public List<Transaction> transactions;
        public BlockingCollection<Transaction> transactions;

        //public Account(int accountType) 
        //{
        //    this.accountType = accountType;
        //    this.transactions = new List<Transaction>();
        //}

        public void Deposit(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("amount must be greater than zero");
            }
            else
            {
                transactions.Add(new Transaction(amount));
            }
        }

        public void Withdraw(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("amount must be greater than zero");
            }
            else
            {
                transactions.Add(new Transaction(-amount));
            }
        }

        public abstract double InterestEarned();

        //private double DeleteMe_InterestEarned()
        //{
        //        double amount = sumTransactions();
        //        double interest = -1;

        //        try
        //        {
        //            switch (accountType)
        //            {
        //                case SAVINGS:
        //                    if (amount <= 1000)
        //                        interest = amount * 0.001;
        //                    else
        //                        interest = 1 + (amount - 1000) * 0.002;
        //                //            case SUPER_SAVINGS:
        //                //                if (amount <= 4000)
        //                //                    return 20;
        //                case MAXI_SAVINGS:
        //                    if (amount <= 1000)
        //                        interest = amount * 0.02;
        //                    if (amount <= 2000)
        //                        interest = 20 + (amount - 1000) * 0.05;
        //                    interest = 70 + (amount - 2000) * 0.1;
        //                default:
        //                    return amount * 0.001;
        //            }

        //            return interest;
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //        }
        //    }


        public double ComputeInterest(double amount, double interest)
        {
            return amount * interest;
        }



        public double sumTransactions()
        {
            return CheckIfTransactionsExist(true);
        }

        private double CheckIfTransactionsExist(bool checkAll)
        {
            double amount = 0.0;
            foreach (Transaction t in transactions)
                amount += t.amount;
            return amount;
        }

        //public int GetAccountType() 
        //{
        //    return accountType;
        //}


        public abstract string GetAccountTypeHeader();

    }



    public class CHECKINGAccount : AbstractAccount
    {

        public CHECKINGAccount()
        {
            //this.transactions = new List<Transaction>();
            this.transactions = new BlockingCollection<Transaction>();
        }

        public const double baseInterest = 0.001;
        override public double InterestEarned()
        {
            double interest = -1;
            try
            {
                double amount = sumTransactions();
                interest = ComputeInterest(amount, baseInterest);
                return interest;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        override public string GetAccountTypeHeader()
        {
            return "Checking Account\n";
        }
    }

    public class SAVINGSAccount : AbstractAccount
    {
        public const double baseInterest = 0.001;
        public const double baseAmount = 1000;
        public const double savingsInterest = 0.002;


        public SAVINGSAccount()
        {
            //this.transactions = new List<Transaction>();
            this.transactions = new BlockingCollection<Transaction>();
        }

        override public double InterestEarned()
        {
            double interest = -1;
            try
            {
                double amount = sumTransactions();

                double baseInterstAmount = amount;
                double savingInterstAmount = 0;
                if (amount > baseAmount)
                {
                    baseInterstAmount = baseAmount;
                    savingInterstAmount = amount - baseAmount;
                }

                interest = ComputeInterest(baseInterstAmount, baseInterest);
                interest += ComputeInterest(savingInterstAmount, savingsInterest);

                return interest;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }


        override public string GetAccountTypeHeader()
        {
            return "Savings Account\n";
        }

    }

    public class MAXI_SAVINGSAccount : AbstractAccount
    {
        public const double BASE_INTEREST = 0.02;
        public const double BASE_AMOUNT = 1000;

        public const double MAXI_PLUS_INTEREST = 0.05;
        public const double MAXI_PLUS_AMOUNT = 1000;

        public const double MAXI_PREMIUM_INTEREST = 0.1;


        public const double MAXI_PLUS_NO_WITHDRAW_INTEREST = 0.05;
        public const double MAXI_PLUS_PENALTY_INTEREST = 0.001;



        public MAXI_SAVINGSAccount()
        {
            //this.transactions = new List<Transaction>();
            this.transactions = new BlockingCollection<Transaction>();
        }


        public double InterestEarned_MethodWith_3_SLABS()
        {
            double interest = -1;
            try
            {
                double amount = sumTransactions();

                double baseInterstAmount = amount;
                double maxiPlusAmount = 0;
                double maxiPremiumAmount = 0;

                if (amount > BASE_AMOUNT && amount <= BASE_AMOUNT + maxiPlusAmount)
                {
                    baseInterstAmount = BASE_AMOUNT;
                    maxiPlusAmount = amount - BASE_AMOUNT;
                }
                else if (amount > BASE_AMOUNT + MAXI_PLUS_AMOUNT)
                {
                    baseInterstAmount = BASE_AMOUNT;
                    maxiPlusAmount = MAXI_PLUS_AMOUNT;
                    maxiPremiumAmount = amount - (BASE_AMOUNT + MAXI_PLUS_AMOUNT);
                }

                interest = ComputeInterest(baseInterstAmount, BASE_INTEREST);
                interest += ComputeInterest(maxiPlusAmount, MAXI_PLUS_INTEREST);
                interest += ComputeInterest(maxiPremiumAmount, MAXI_PREMIUM_INTEREST);

                return interest;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }


        override public double InterestEarned()
        {
            double interest = -1;
            try
            {
                double amount = sumTransactions();

                double rate = MAXI_PLUS_NO_WITHDRAW_INTEREST;
                if (HasWithdraw())
                {
                    rate = MAXI_PLUS_PENALTY_INTEREST;
                }

                interest = ComputeInterest(amount, rate);

                return interest;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        private bool HasWithdraw()
        {
            bool result = false;

            if (this.transactions != null)
            {
                List<Transaction> withdraws = this.transactions.Where(x => x.amount < 0).ToList();
                foreach (Transaction trans in withdraws)
                {
                    if (trans.GetTransactionDate().Date.AddDays(10) > DateProvider.getInstance().Now().Date)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        override public string GetAccountTypeHeader()
        {
            return "Maxi Savings Account\n";
        }

    }
}
