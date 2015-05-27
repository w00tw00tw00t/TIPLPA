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
            var result = Schemer.GetStringFromCommand("(moveRight 1)");
            Assert.AreEqual("((\"pos\" 1 8))\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveRightMultuple()
        {
            var result = Schemer.GetStringFromCommand("(moveRight 2)");
            Assert.AreEqual("((\"pos\" 1 8) (\"pos\" 2 8))\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveLeft()
        {
            var result = Schemer.GetStringFromCommand("(moveRight 1)\n(moveLeft 1)");
            Assert.AreEqual("((\"pos\" 0 8))\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveLeftMultiple()
        {
            var result = Schemer.GetStringFromCommand("(moveRight 2)\n(moveLeft 2)");
            Assert.AreEqual("((\"pos\" 1 8) (\"pos\" 0 8))\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveDown()
        {
            var result = Schemer.GetStringFromCommand("(moveDown 1)");
            Assert.AreEqual("((\"pos\" 0 9))\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveDownMultiple()
        {
            var result = Schemer.GetStringFromCommand("(moveDown 2)");
            Assert.AreEqual("((\"pos\" 0 9) (\"pos\" 0 10))\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveUp()
        {
            var result = Schemer.GetStringFromCommand("(moveDown 1)\n(moveUp 1)");
            Assert.AreEqual("((\"pos\" 0 8))\n", result);
        }

        [Test]
        public void AbsoluteMovement_MoveUpMultiple()
        {
            var result = Schemer.GetStringFromCommand("(moveDown 2)\n(moveUp 2)");
            Assert.AreEqual("((\"pos\" 0 9) (\"pos\" 0 8))\n", result);
        }

        #endregion

        #region Relative Movement

        [Test]
        public void RelativeMovement_TurnRight()
        {
            var result = Schemer.GetStringFromCommand("(turnRight 1)");
            Assert.AreEqual("((\"dir\" 1))\n", result);
        }

        [Test]
        public void RelativeMovement_TurnRightMultiple()
        {
            var result = Schemer.GetStringFromCommand("(turnRight 2)");
            Assert.AreEqual("((\"dir\" 1) (\"dir\" 2))\n", result);
        }

        [Test]
        public void RelativeMovement_TurnLeft()
        {
            var result = Schemer.GetStringFromCommand("(turnLeft 1)");
            Assert.AreEqual("((\"dir\" 3))\n", result);
        }

        [Test]
        public void RelativeMovement_TurnLeftMultiple()
        {
            var result = Schemer.GetStringFromCommand("(turnLeft 2)");
            Assert.AreEqual("((\"dir\" 3) (\"dir\" 2))\n", result);
        }

        [Test]
        public void RelativeMovement_MoveForward()
        {
            var result = Schemer.GetStringFromCommand("(turnRight 1)\n(moveForward 1)");
            Assert.AreEqual("((\"pos\" 1 8))\n", result);
        }

        [Test]
        public void RelativeMovement_MoveForwardMultiple()
        {
            var result = Schemer.GetStringFromCommand("(turnRight 1)\n(moveForward 2)");
            Assert.AreEqual("((\"pos\" 1 8) (\"pos\" 2 8))\n", result);
        }

        [Test]
        public void RelativeMovement_MoveBackward()
        {
            var result = Schemer.GetStringFromCommand("(turnLeft 1)\n(moveBackward 1)");
            Assert.AreEqual("((\"pos\" 1 8))\n", result);
        }

        [Test]
        public void RelativeMovement_MoveBackwardMultiple()
        {
            var result = Schemer.GetStringFromCommand("(turnLeft 1)\n(moveBackward 2)");
            Assert.AreEqual("((\"pos\" 1 8) (\"pos\" 2 8))\n", result);
        }

        #endregion

        #region Allowed Movement

        [Test]
        public void AllowedMovement_MoveToGreen()
        {
            var result = Schemer.GetStringFromCommand("(moveRight 1)");
            Assert.AreNotEqual("Some error", result);
        }

        [Test]
        public void AllowedMovement_MoveToWhite()
        {
            var result = Schemer.GetStringFromCommand("(moveUp 1)");
            Assert.AreEqual("((\"Error: up is not a valid move direction\"))\n", result);
        }

        #endregion

        #region Workstations

        [Test]
        public void Workstations_CorrectPickup()
        {
            var result =
                Schemer.GetStringFromCommand(
                    "(initRobot 0 0)\n" +
                    "(turnRight 1)\n" +
                    "(moveForward 29)\n" +
                    "(turnLeft 1)\n" +
                    "(moveForward 6)\n" +
                    "(pickup)");
            Assert.AreEqual("(\"pickup\" 1)\n", result);
        }

        [Test]
        public void Workstations_WrongPickup()
        {
            var result = Schemer.GetStringFromCommand(
                "(initRobot 0 0)\n" +
                "(turnRight 1)\n" +
                "(moveForward 8)\n" +
                "(turnLeft 1)\n" +
                "(moveForward 4)\n" +
                "(turnLeft 1)\n" +
                "(pickup)");
            Assert.AreEqual("Error: Not correct pickup point", result);
        }

        [Test]
        public void Workstations_CorrectDropOff()
        {
            var result =
                Schemer.GetStringFromCommand(
                    "(initRobot 0 0)\n" +
                    "(turnRight 1)\n" +
                    "(moveForward 29)\n" +
                    "(turnLeft 1)\n" +
                    "(moveForward 6)\n" +
                    "(pickup)\n" +
                    "(turnRight 2)\n" +
                    "(moveForward 6)\n" +
                    "(turnRight 1)\n" +
                    "(moveForward 21)\n" +
                    "(turnRight 1)\n" +
                    "(moveForward 7)\n" +
                    "(turnLeft 1)\n" +
                    "(moveForward 2)\n" +
                    "(turnLeft 1)\n" +
                    "(dropoff)");
            Assert.AreEqual("(\"dropoff\" 1)\n", result);
        }

        [Test]
        public void Workstations_CorrectPickupWrongDropOff()
        {
            var result = Schemer.GetStringFromCommand(
                "(initRobot 0 0)\n" +
                "(turnRight 1)\n" +
                "(moveForward 31)\n" +
                "(turnLeft 1)\n" +
                "(moveForward 6)\n" +
                "(pickup)\n" +
                "(turnLeft 2)\n" +
                "(moveForward 19)\n" +
                "(dropoff)");
            Assert.AreEqual("Error: Not correct dropoff point", result);
        }

        [Test]
        public void Workstataions_WrongPickupAfterCorrectDropOff()
        {
            var result = Schemer.GetStringFromCommand(
                "(initRobot 0 0)\n" +
                "(turnRight 1)\n" +
                "(moveForward 29)\n" +
                "(turnLeft 1)\n" +
                "(moveForward 6)\n" +
                "(pickup)\n" +
                "(turnLeft 2)\n" +
                "(moveForward 6)\n" +
                "(turnRight 1)\n" +
                "(moveForward 21)\n" +
                "(turnRight 1)\n" +
                "(moveForward 7)\n" +
                "(turnLeft 1)\n" +
                "(moveForward 2)\n" +
                "(turnLeft 1)\n" +
                "(dropoff)\n" +
                "(turnLeft 1)\n" +
                "(moveForward 2)\n" +
                "(turnRight 1)\n" +
                "(moveForward 7)\n" +
                "(turnLeft 1)\n" +
                "(moveForward 18)\n" +
                "(turnRight 1)\n" +
                "(moveForward 3)\n" +
                "(turnRight 1)\n" +
                "(pickup)");

            Assert.AreEqual("Error: Not correct pickup point", result);
        }

        #endregion
    }
}
