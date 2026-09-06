using System.Collections.Generic;
using Xunit;

namespace ArmyArrowCounter.Tests {
    public class ArrowLedgerTests {
        private readonly ArrowLedger Ledger = new ArrowLedger();

        [Fact]
        public void StartsEmpty() {
            Assert.Equal(0, Ledger.Remaining);
            Assert.Equal(0, Ledger.Max);
        }

        [Fact]
        public void AddingAnAgentAddsBothTotals() {
            Ledger.Add(1, maxAmmo: 30, remainingAmmo: 20);

            Assert.Equal(20, Ledger.Remaining);
            Assert.Equal(30, Ledger.Max);
        }

        [Fact]
        public void AddingTheSameAgentTwiceCountsItOnce() {
            Ledger.Add(1, 30, 30);
            Ledger.Add(1, 30, 30);

            Assert.Equal(30, Ledger.Max);
        }

        [Fact]
        public void RemovingAnAgentReturnsTheTotalsToZero() {
            Ledger.Add(1, 30, 20);
            Ledger.Remove(1);

            Assert.Equal(0, Ledger.Remaining);
            Assert.Equal(0, Ledger.Max);
        }

        [Fact]
        public void RemovalSubtractsWhatWasRecorded_NotWhatIsObservedLater() {
            // An agent that dies or flees has already lost its equipment, so the
            // amount it contributed can only come from what the ledger stored.
            Ledger.Add(1, 30, 30);
            Ledger.RecordShot(1);
            Ledger.RecordShot(1);
            Ledger.Remove(1);

            Assert.Equal(0, Ledger.Remaining);
            Assert.Equal(0, Ledger.Max);
        }

        [Fact]
        public void RemovingAnUntrackedAgentChangesNothing() {
            Ledger.Add(1, 30, 30);
            Ledger.Remove(99);

            Assert.Equal(30, Ledger.Remaining);
            Assert.Equal(30, Ledger.Max);
        }

        [Fact]
        public void AShotSpendsOneArrow() {
            Ledger.Add(1, 30, 30);
            Ledger.RecordShot(1);

            Assert.Equal(29, Ledger.Remaining);
            Assert.Equal(30, Ledger.Max);
        }

        [Fact]
        public void AShotFromAnUntrackedAgentIsIgnored() {
            // Previously this decremented the army total with no per-agent record to
            // match, so the count drifted below zero and had to be periodically rebuilt.
            Ledger.Add(1, 30, 30);
            Ledger.RecordShot(99);

            Assert.Equal(30, Ledger.Remaining);
        }

        [Theory]
        [InlineData(25, 25)]
        [InlineData(20, 20)]
        [InlineData(15, 15)]
        public void ObservedAmmoBecomesTheNewRemaining(int observed, int expectedTotal) {
            Ledger.Add(1, 30, 20);
            Ledger.RecordObservedAmmo(1, observed);

            Assert.Equal(expectedTotal, Ledger.Remaining);
        }

        [Fact]
        public void ObservedAmmoForAnUntrackedAgentIsIgnored() {
            Ledger.Add(1, 30, 20);
            Ledger.RecordObservedAmmo(99, 100);

            Assert.Equal(20, Ledger.Remaining);
        }

        [Fact]
        public void ClearingZeroesBothTotalsAndForgetsEveryAgent() {
            Ledger.Add(1, 30, 20);
            Ledger.Add(2, 10, 10);
            Ledger.Clear();

            Assert.Equal(0, Ledger.Remaining);
            Assert.Equal(0, Ledger.Max);
            Assert.False(Ledger.IsTracking(1));
        }

        [Fact]
        public void AgentsAreIndependent() {
            Ledger.Add(1, 30, 30);
            Ledger.Add(2, 10, 10);
            Ledger.Remove(1);

            Assert.Equal(10, Ledger.Remaining);
            Assert.Equal(10, Ledger.Max);
        }

        [Fact]
        public void EveryChangeAnnouncesTheNewTotal() {
            List<int> remaining = new List<int>();
            List<int> max = new List<int>();
            Ledger.RemainingChanged += r => remaining.Add(r);
            Ledger.MaxChanged += m => max.Add(m);

            Ledger.Add(1, 30, 30);
            Ledger.RecordShot(1);

            Assert.Equal(new[] { 30, 29 }, remaining);
            Assert.Equal(new[] { 30 }, max);
        }

        [Fact]
        public void ARoundTripOfManyAgentsLeavesNothingBehind() {
            for (int id = 0; id < 200; id++) {
                Ledger.Add(id, 40, 40);
            }
            for (int id = 0; id < 200; id++) {
                Ledger.RecordShot(id);
            }
            for (int id = 0; id < 200; id++) {
                Ledger.Remove(id);
            }

            Assert.Equal(0, Ledger.Remaining);
            Assert.Equal(0, Ledger.Max);
        }
    }
}
