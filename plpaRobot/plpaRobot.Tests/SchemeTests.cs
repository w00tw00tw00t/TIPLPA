using NUnit.Framework;

namespace plpaRobot.Tests
{
    [TestFixture]
    class SchemeTests
    {
        [SetUp]
        public void SchemeTests_Setup()
        {
            Schemer.resetEval();
            Schemer.doImports();
            Schemer.loadSchemeFile("SchemeFiles/robotactions.ss");
            Schemer.loadSchemeFile("../../../../plpaRobotScheme/floorplanFromAssignment.ss");
            Schemer.Eval("(initRobot 0 0)");
        }

        #region Absolute Movement

        [Test]
        public void AbsoluteMovement_MoveRight()
        {
            var result = Schemer.ConvertCoordinatesToString("(moveRight)");
            Assert.AreEqual("(1 8)\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveLeft()
        {
            var result = Schemer.ConvertCoordinatesToString("(moveRight)\n(moveLeft)");
            Assert.AreEqual("(0 8)\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveDown()
        {
            var result = Schemer.ConvertCoordinatesToString("(moveDown)");
            Assert.AreEqual("(0 9)\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveUp()
        {
            var result = Schemer.ConvertCoordinatesToString("(moveDown)\n(moveUp)");
            Assert.AreEqual("(0 8)\n", result);
        }

        #endregion

        #region Allowed Movement

        [Test]
        public void AllowedMovement_MoveToGreen()
        {
            var result = Schemer.ConvertCoordinatesToString("(moveRight)");
            Assert.AreNotEqual("Some error", result);
        }

        [Test]
        public void AllowedMovement_MoveToWhite()
        {
            var result = Schemer.ConvertCoordinatesToString("(moveUp)");
            Assert.AreEqual("Some error", result);
        }

        #endregion

        #region Workstations

        #endregion
    }
}
