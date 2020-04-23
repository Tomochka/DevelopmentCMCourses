namespace CashpointModel.Tests
{
    using NUnit.Framework;

    [TestFixture]
    class CashpointTests
    {
        [Test]
        public void AddBanknote_OneBanknoteOnOneBill_ShouldIncrementTotal()
        {
            var cashpoint = new Cashpoint();
            cashpoint.AddBanknote(5);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(5u),
                "Добавление единственной банкноты не было произведено");
        }

        [Test]
        public void AddBanknote_MultipleBanknotesOnOneBill_ShouldIncrementTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(5);
            cashpoint.AddBanknote(10);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(15u),
                "Добавление второй банкноты не было произведено");
        }

        [Test]
        public void AddBanknote_OneBanknoteOnMultipleBill_ShouldIncrementTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(3, 4);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(12u),
                "Банкноты были добавлены неправильно");
        }
        
        [Test]
        public void AddBanknote_MultipleBanknotesOnMultipleBill_ShouldIncrementTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(3, 2);
            cashpoint.AddBanknote(10, 5);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(56u),
                "Сумма добавленых купюр не соответствует");
        }

        [Test]
        public void AddBanknote_OneBanknoteOnOneBill_ShouldIncrementCountBanknotes()
        {
            var cashpoint = new Cashpoint();
            cashpoint.AddBanknote(5);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(1u),
                "Неверное количество банкнот в банкомате");
        }

        [Test]
        public void AddBanknote_OneBanknoteOnMultipleBill_ShouldIncrementCountBanknotes()
        {
            var cashpoint = new Cashpoint();
            cashpoint.AddBanknote(5,3);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(3u),
                "Неверное количество банкнот в банкомате");
        }

        [Test]
        public void AddBanknote_MultipleBanknotesOnMultipleBill_ShouldIncrementCountBanknotes()
        {
            var cashpoint = new Cashpoint();
            cashpoint.AddBanknote(5, 3);
            cashpoint.AddBanknote(3, 4);
            cashpoint.AddBanknote(6, 5);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(12u),
                "Неверное количество банкнот в банкомате");
        }

        [Test]
        public void AddBanknote_CashpointIsEmpty_ShouldPreserveCountBanknotes()
        {
            var cashpoint = new Cashpoint();

            cashpoint.RemoveBanknote(5);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(0),
                "Извлечена несуществующая купюра из пустого банкомата");
        }

        [Test]
        public void RemoveBanknote_UnknownBanknoteOnOneBill_ShouldPreserveCountBanknotes()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(7);
            cashpoint.RemoveBanknote(5);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(1),
                "Извлечена несуществующая купюра");
        }

        [Test]
        public void RemoveBanknote_UnknownBanknoteOnMultipleBill_ShouldPreserveCountBanknotes()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(7);
            cashpoint.RemoveBanknote(5, 2);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(1),
                "Извлечены несуществующие купюры");
        }
        
        [Test]
        public void RemoveBanknote_ExistingBanknoteOnOneBill_ShouldDecrementCountBanknotes()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(5);
            cashpoint.AddBanknote(10);
            cashpoint.RemoveBanknote(5);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(1),
                "Неверное количество купюр в банкомате");
        }

        [Test]
        public void RemoveBanknote_ExistingBanknotesOnOneBill_ShouldDecrementCountBanknotes()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(3);
            cashpoint.AddBanknote(10);
            cashpoint.AddBanknote(1);
            cashpoint.AddBanknote(5);
            cashpoint.RemoveBanknote(5);
            cashpoint.RemoveBanknote(10);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(2),
                "Неверное количество купюр в банкомате");
        }

        [Test]
        public void RemoveBanknote_ExistingBanknoteOnMultipleBill_ShouldDecrementCountBanknotes()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(3, 4);
            cashpoint.AddBanknote(10, 2);
            cashpoint.RemoveBanknote(3, 2);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(4),
                "Неверное количество купюр в банкомате");
        }

        [Test]
        public void CountBanknotes_InitialState_ShouldBeZero()
        {
            var cashpoint = new Cashpoint();

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(0u),
                "Новый банкомат оказался не пустой");
        }

        [Test]
        public void RemoveBanknote_ExistingBanknotesOnMultipleBill_ShouldDecrementCountBanknotes()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(3, 4);
            cashpoint.AddBanknote(10, 2);
            cashpoint.AddBanknote(6, 3);
            cashpoint.AddBanknote(7, 5);
            cashpoint.RemoveBanknote(3, 2);
            cashpoint.RemoveBanknote(7, 3);

            Assert.That(
                cashpoint.CountBanknotes,
                Is.EqualTo(9),
                "Неверное количество купюр в банкомате");
        }

        [Test]
        public void RemoveBanknote_CashpointIsEmpty_ShouldPreserveTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.RemoveBanknote(5);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(0),
                "Извлечена несуществующая купюра из пустого банкомата");
        }

        [Test]
        public void RemoveBanknote_UnknownBanknoteOnOneBill_ShouldPreserveTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(7);
            cashpoint.RemoveBanknote(5);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(7),
                "Извлечена несуществующая купюра");
        }

        [Test]
        public void RemoveBanknote_UnknownBanknoteOnMultipleBill_ShouldPreserveTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(7);
            cashpoint.RemoveBanknote(5, 2);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(7),
                "Извлечены несуществующие купюры");
        }

        [Test]
        public void RemoveBanknote_ExistingBanknoteOnOneBill_ShouldDecrementTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(5);
            cashpoint.AddBanknote(10);
            cashpoint.RemoveBanknote(5);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(10),
                "Купюра извлечена некорректно");
        }

        [Test]
        public void RemoveBanknote_ExistingBanknotesOnOneBill_ShouldDecrementTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(3);
            cashpoint.AddBanknote(10);
            cashpoint.AddBanknote(1);
            cashpoint.AddBanknote(5);
            cashpoint.RemoveBanknote(5);
            cashpoint.RemoveBanknote(10);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(4),
                "Купюры извлечены некорректно");
        }

        [Test]
        public void RemoveBanknote_ExistingBanknoteOnMultipleBill_ShouldDecrementTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(3, 4);
            cashpoint.AddBanknote(10, 2);
            cashpoint.RemoveBanknote(3, 2);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(26),
                "Купюры извлечены некорректно");
        }

        [Test]
        public void RemoveBanknote_ExistingBanknotesOnMultipleBill_ShouldDecrementTotal()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(3, 4);
            cashpoint.AddBanknote(10, 2);
            cashpoint.AddBanknote(6, 3);
            cashpoint.AddBanknote(7, 5);
            cashpoint.RemoveBanknote(3, 2);
            cashpoint.RemoveBanknote(7, 3);

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(58),
                "Купюры извлечены некорректно");
        }
        
        [Test]
        public void Total_InitialState_ShouldBeZero()
        {
            var cashpoint = new Cashpoint();

            Assert.That(
                cashpoint.Total,
                Is.EqualTo(0u),
                "Новый банкомат оказался не пустой");
        }

        [Test]
        public void CanGrant_SumIsZero_ShouldGrant()
        {
            var cashpoint = new Cashpoint();

            Assert.That(
                cashpoint.CanGrant(0),
                "Банкомат не смог выдать 0");

            cashpoint.AddBanknote(5);

            Assert.That(
                cashpoint.CanGrant(0),
                "Банкомат не смог выдать 0 после добавления купюры");
        }

        [Test]
        public void CanGrant_SumEqualsToSingleBanknote_ShouldGrant()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(5);

            Assert.That(
                cashpoint.CanGrant(5),
                "Банкомат не смог выдать единственную банкноту");
        }

        [Test]
        public void CanGrant_SumEqualsToMultipleBanknotes_ShouldGrant()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(5, 3);

            Assert.That(
                cashpoint.CanGrant(15),
                "Банкомат не смог выдать имеющуюся в нем сумму");
        }

        [Test]
        public void CanGrant_SumNotEqualToSingleBanknote_ShouldNotGrant()
         {
             var cashpoint = new Cashpoint();

             cashpoint.AddBanknote(5);

             Assert.That(
                 cashpoint.CanGrant(4),
                 Is.False,
                 "Банкомат смог выдать значение меньше номинала единственной купюры");

             Assert.That(
                 cashpoint.CanGrant(6),
                 Is.False,
                 "Банкомат смог выдать значение больше номинала единственной купюры");
         }

        [Test]
        public void CanGrant_SumEqualsToBanknotesTotal_ShouldGrant()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(5);
            cashpoint.AddBanknote(3);

            Assert.That(
                cashpoint.CanGrant(8),
                "Банкомат не смог выдать сумму двух купюр");
        }

        [Test]
        public void CanGrant_MultipleBanknotesIntermediateValues_ShouldNotGrant()
        {
            var cashpoint = new Cashpoint();

            cashpoint.AddBanknote(5,1);
            cashpoint.AddBanknote(3,1);

            Assert.That(
                cashpoint.CanGrant(6),
                Is.False,
                "Банкомат смог выдать значение между номиналами купюр");
        }

        [Test]
        [Timeout(20000)]
        public void CanGrant_PerformanceTest()
        {
            var cashpoint = new Cashpoint();
            for (var i = 0; i < 2; i++)
            {
                cashpoint.AddBanknote(10);
                cashpoint.AddBanknote(50);
                cashpoint.AddBanknote(100);
                cashpoint.AddBanknote(200);
                cashpoint.AddBanknote(500);
                cashpoint.AddBanknote(1000);
                cashpoint.AddBanknote(2000);
                cashpoint.AddBanknote(5000);
            }

            Assert.That(cashpoint.CanGrant(3350));
            Assert.That(cashpoint.CanGrant(3980), Is.False);
        }

        //Sieve
        [Test]
        public void AddBanknote_OneBanknoteOnOneBill_ShouldRecalculateSieve()
        {
            var cashpoint = new Cashpoint();
            cashpoint.AddBanknote(5);

            Assert.That(
                cashpoint.Sieve[5],
                Is.EqualTo(1u),
                "Неправильное число в 5 ячейке решета");
        }

        [Test]
        public void AddBanknote_OneBanknoteOnMultipleBill_ShouldRecalculateSieve()
        {
            var cashpoint = new Cashpoint();
            cashpoint.AddBanknote(5, 4);

            Assert.That(
                cashpoint.Sieve[5],
                Is.EqualTo(4u),
                "Неправильное число в 5 ячейке решета");
            Assert.That(
                cashpoint.Sieve[10],
                Is.EqualTo(6u),
                "Неправильное число в 10 ячейке решета");
            Assert.That(
                cashpoint.Sieve[15],
                Is.EqualTo(4u),
                "Неправильное число в 15 ячейке решета");
            Assert.That(
                cashpoint.Sieve[20],
                Is.EqualTo(1u),
                "Неправильное число в 20 ячейке решета");
        }

        [Test]
        public void AddBanknote_MultipleBanknotesOnOneBill_ShouldRecalculateSieve()
        {
            var cashpoint = new Cashpoint();
            cashpoint.AddBanknote(5);
            cashpoint.AddBanknote(4);
            cashpoint.AddBanknote(3);
            cashpoint.AddBanknote(5);
            cashpoint.AddBanknote(2);

            Assert.That(
                cashpoint.Sieve[0],
                Is.EqualTo(1u),
                "Неправильное число в 0 ячейке решета");
            Assert.That(
              cashpoint.Sieve[1],
              Is.EqualTo(0u),
              "Неправильное число в 1 ячейке решета");
            Assert.That(
              cashpoint.Sieve[2],
              Is.EqualTo(1u),
              "Неправильное число в 2 ячейке решета");
            Assert.That(
              cashpoint.Sieve[3],
              Is.EqualTo(1u),
              "Неправильное число в 3 ячейке решета");
            Assert.That(
              cashpoint.Sieve[4],
              Is.EqualTo(1u),
              "Неправильное число в 4 ячейке решета");
            Assert.That(
              cashpoint.Sieve[5],
              Is.EqualTo(3u),
              "Неправильное число в 5 ячейке решета");
            Assert.That(
              cashpoint.Sieve[6],
              Is.EqualTo(1u),
              "Неправильное число в 6 ячейке решета");
            Assert.That(
              cashpoint.Sieve[7],
              Is.EqualTo(3u),
              "Неправильное число в 7 ячейке решета");
            Assert.That(
              cashpoint.Sieve[8],
              Is.EqualTo(2u),
              "Неправильное число в 8 ячейке решета");
            Assert.That(
              cashpoint.Sieve[9],
              Is.EqualTo(3u),
              "Неправильное число в 9 ячейке решета");
            Assert.That(
              cashpoint.Sieve[10],
              Is.EqualTo(3u),
              "Неправильное число в 10 ячейке решета");
            Assert.That(
              cashpoint.Sieve[11],
              Is.EqualTo(2u),
              "Неправильное число в 11 ячейке решета");
            Assert.That(
              cashpoint.Sieve[12],
              Is.EqualTo(3u),
             "Неправильное число в 12 ячейке решета");
            Assert.That(
              cashpoint.Sieve[13],
              Is.EqualTo(1u),
              "Неправильное число в 13 ячейке решета");
            Assert.That(
              cashpoint.Sieve[14],
              Is.EqualTo(3u),
              "Неправильное число в 14 ячейке решета");
            Assert.That(
              cashpoint.Sieve[15],
              Is.EqualTo(1u),
              "Неправильное число в 15 ячейке решета");
            Assert.That(
              cashpoint.Sieve[16],
              Is.EqualTo(1u),
              "Неправильное число в 16 ячейке решета");
            Assert.That(
             cashpoint.Sieve[17],
             Is.EqualTo(1u),
             "Неправильное число в 17 ячейке решета");
            Assert.That(
             cashpoint.Sieve[18],
             Is.EqualTo(0u),
             "Неправильное число в 18 ячейке решета");
            Assert.That(
             cashpoint.Sieve[19],
             Is.EqualTo(1u),
             "Неправильное число в 19 ячейке решета");
        }

        [Test]
        public void AddBanknote_MultipleBanknotesOnMultipleBill_ShouldRecalculateSieve()
        {
            var cashpoint = new Cashpoint();
           
            cashpoint.AddBanknote(4);
            cashpoint.AddBanknote(3);
            cashpoint.AddBanknote(5, 3);
            cashpoint.AddBanknote(2);

            Assert.That(
                cashpoint.Sieve[0],
                Is.EqualTo(1u),
                "Неправильное число в 0 ячейке решета");
            Assert.That(
              cashpoint.Sieve[1],
              Is.EqualTo(0u),
              "Неправильное число в 1 ячейке решета");
            Assert.That(
              cashpoint.Sieve[2],
              Is.EqualTo(1u),
              "Неправильное число в 2 ячейке решета");
            Assert.That(
              cashpoint.Sieve[3],
              Is.EqualTo(1u),
              "Неправильное число в 3 ячейке решета");
            Assert.That(
              cashpoint.Sieve[4],
              Is.EqualTo(1u),
              "Неправильное число в 4 ячейке решета");
            Assert.That(
              cashpoint.Sieve[5],
              Is.EqualTo(4u),
              "Неправильное число в 5 ячейке решета");
            Assert.That(
              cashpoint.Sieve[6],
              Is.EqualTo(1u),
              "Неправильное число в 6 ячейке решета");
            Assert.That(
              cashpoint.Sieve[7],
              Is.EqualTo(4u),
              "Неправильное число в 7 ячейке решета");
            Assert.That(
              cashpoint.Sieve[8],
              Is.EqualTo(3u),
              "Неправильное число в 8 ячейке решета");
            Assert.That(
              cashpoint.Sieve[9],
              Is.EqualTo(4u),
              "Неправильное число в 9 ячейке решета");
            Assert.That(
              cashpoint.Sieve[10],
              Is.EqualTo(6u),
              "Неправильное число в 10 ячейке решета");
            Assert.That(
              cashpoint.Sieve[11],
              Is.EqualTo(3u),
              "Неправильное число в 11 ячейке решета");
            Assert.That(
              cashpoint.Sieve[12],
              Is.EqualTo(6u),
             "Неправильное число в 12 ячейке решета");
            Assert.That(
              cashpoint.Sieve[13],
              Is.EqualTo(3u),
              "Неправильное число в 13 ячейке решета");
            Assert.That(
              cashpoint.Sieve[14],
              Is.EqualTo(6u),
              "Неправильное число в 14 ячейке решета");
            Assert.That(
              cashpoint.Sieve[15],
              Is.EqualTo(4u),
              "Неправильное число в 15 ячейке решета");
            Assert.That(
              cashpoint.Sieve[16],
              Is.EqualTo(3u),
              "Неправильное число в 16 ячейке решета");
            Assert.That(
             cashpoint.Sieve[17],
             Is.EqualTo(4u),
             "Неправильное число в 17 ячейке решета");
            Assert.That(
             cashpoint.Sieve[18],
             Is.EqualTo(1u),
             "Неправильное число в 18 ячейке решета");
            Assert.That(
             cashpoint.Sieve[19],
             Is.EqualTo(4u),
             "Неправильное число в 19 ячейке решета");
            Assert.That(
             cashpoint.Sieve[20],
             Is.EqualTo(1u),
             "Неправильное число в 20 ячейке решета");
            Assert.That(
             cashpoint.Sieve[21],
             Is.EqualTo(1u),
             "Неправильное число в 21 ячейке решета");
            Assert.That(
             cashpoint.Sieve[22],
             Is.EqualTo(1u),
             "Неправильное число в 22 ячейке решета");
            Assert.That(
             cashpoint.Sieve[23],
             Is.EqualTo(0u),
             "Неправильное число в 23 ячейке решета");
            Assert.That(
             cashpoint.Sieve[24],
             Is.EqualTo(1u),
             "Неправильное число в 24 ячейке решета");

        }

        [Test]
        public void RemoveBanknote_OneBanknoteOnOneBill_ShouldRecalculateSieve()
        {
            var cashpoint = new Cashpoint();
            cashpoint.AddBanknote(5);
            cashpoint.AddBanknote(4);
            cashpoint.AddBanknote(3);
            cashpoint.AddBanknote(5);
            cashpoint.AddBanknote(2);
            cashpoint.RemoveBanknote(5);

            Assert.That(
                cashpoint.Sieve[0],
                Is.EqualTo(1u),
                "Неправильное число в 0 ячейке решета");
            Assert.That(
              cashpoint.Sieve[1],
              Is.EqualTo(0u),
              "Неправильное число в 1 ячейке решета");
            Assert.That(
              cashpoint.Sieve[2],
              Is.EqualTo(1u),
              "Неправильное число в 2 ячейке решета");
            Assert.That(
              cashpoint.Sieve[3],
              Is.EqualTo(1u),
              "Неправильное число в 3 ячейке решета");
            Assert.That(
              cashpoint.Sieve[4],
              Is.EqualTo(1u),
              "Неправильное число в 4 ячейке решета");
            Assert.That(
              cashpoint.Sieve[5],
              Is.EqualTo(2u),
              "Неправильное число в 5 ячейке решета");
            Assert.That(
              cashpoint.Sieve[6],
              Is.EqualTo(1u),
              "Неправильное число в 6 ячейке решета");
            Assert.That(
              cashpoint.Sieve[7],
              Is.EqualTo(2u),
              "Неправильное число в 7 ячейке решета");
            Assert.That(
              cashpoint.Sieve[8],
              Is.EqualTo(1u),
              "Неправильное число в 8 ячейке решета");
            Assert.That(
              cashpoint.Sieve[9],
              Is.EqualTo(2u),
              "Неправильное число в 9 ячейке решета");
            Assert.That(
              cashpoint.Sieve[10],
              Is.EqualTo(1u),
              "Неправильное число в 10 ячейке решета");
            Assert.That(
              cashpoint.Sieve[11],
              Is.EqualTo(1u),
              "Неправильное число в 11 ячейке решета");
            Assert.That(
              cashpoint.Sieve[12],
              Is.EqualTo(1u),
             "Неправильное число в 12 ячейке решета");
            Assert.That(
              cashpoint.Sieve[13],
              Is.EqualTo(0u),
              "Неправильное число в 13 ячейке решета");
            Assert.That(
              cashpoint.Sieve[14],
              Is.EqualTo(1u),
              "Неправильное число в 14 ячейке решета");
        }

        [Test]
        public void RemoveBanknote_OneBanknoteOnMultipleBill_ShouldRecalculateSieve()
        {
            var cashpoint = new Cashpoint();
            cashpoint.AddBanknote(5, 2);
            cashpoint.AddBanknote(4);
            cashpoint.AddBanknote(3);
            cashpoint.AddBanknote(2);
            cashpoint.RemoveBanknote(5, 2);

            Assert.That(
                cashpoint.Sieve[0],
                Is.EqualTo(1u),
                "Неправильное число в 0 ячейке решета");
            Assert.That(
              cashpoint.Sieve[1],
              Is.EqualTo(0u),
              "Неправильное число в 1 ячейке решета");
            Assert.That(
              cashpoint.Sieve[2],
              Is.EqualTo(1u),
              "Неправильное число в 2 ячейке решета");
            Assert.That(
              cashpoint.Sieve[3],
              Is.EqualTo(1u),
              "Неправильное число в 3 ячейке решета");
            Assert.That(
              cashpoint.Sieve[4],
              Is.EqualTo(1u),
              "Неправильное число в 4 ячейке решета");
            Assert.That(
              cashpoint.Sieve[5],
              Is.EqualTo(1u),
              "Неправильное число в 5 ячейке решета");
            Assert.That(
              cashpoint.Sieve[6],
              Is.EqualTo(1u),
              "Неправильное число в 6 ячейке решета");
            Assert.That(
              cashpoint.Sieve[7],
              Is.EqualTo(1u),
              "Неправильное число в 7 ячейке решета");
            Assert.That(
              cashpoint.Sieve[8],
              Is.EqualTo(0u),
              "Неправильное число в 8 ячейке решета");
            Assert.That(
              cashpoint.Sieve[9],
              Is.EqualTo(1u),
              "Неправильное число в 9 ячейке решета");
        }
        
    }
}
