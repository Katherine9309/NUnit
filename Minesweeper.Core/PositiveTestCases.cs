using NUnit.Framework;

using Minesweeper.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minesweeper.Core.Enums;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace Minesweeper.Core
{
    [TestFixture]
    internal class PositiveTestCasesOpenMethod
    {
        bool[,] testField = new bool[9, 9];
        GameProcessor gameProcessor;

        [SetUp]
        public void CreateField()
        {
            testField[0, 7] = true;
            testField[1, 5] = true;
            testField[2, 4] = true;
            testField[3, 4] = true;
            testField[4, 3] = true;
            testField[4, 7] = true;
            testField[5, 5] = true;
            testField[6, 0] = true;
            testField[6, 3] = true;
            testField[7, 4] = true;
            gameProcessor = new GameProcessor(testField);
        }



        [Test, Order(1)]
        public void OpenCell_SetMinesOnFieldAndOpenCloseCellWithMine_GameIsOver()
        {
            //action
            var actual = gameProcessor.Open(7, 4);
            //assert
            Assert.AreEqual(GameState.Lose, actual);
        }

        [Test, Order(2)]
        public void OpenCell_SetMinesOnFieldAndOpenCloseCellWithoutMine_GameKeepsActive()
        {
            //action
            var actual = gameProcessor.Open(2, 0);

            //assert
            var expected = GameState.Active;
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(3)]
        public void OpenCell_SetMinesOnFieldAndOpenAllTheCellsWithoutMines_GameWined()
        {


            //action
            var actual = GameState.Active;
            for (int row = 0; row < testField.GetLength(0); row++) {
               for (int column = 0; column < testField.GetLength(0); column++) {
                    if (!testField[row, column] && actual!= GameState.Win)
                    {
                        actual= gameProcessor.Open(column, row);
                    
                    }
                }
           }
          
          
            //assert
        
            var expected = GameState.Win;
            Assert.AreEqual(expected, actual);
        }


        [Test, Order(4)]
        public void OpenCell_SetMinesOnFieldAndOpenAnOpenedCell_GameKeepsActive()
        {
            //action
            gameProcessor.Open(0, 0);
            var actual = gameProcessor.Open(0, 0);
            //assert
            var expected = GameState.Active;
            Assert.AreEqual(expected, actual);
        }
        [Test, Order(5)]
        public void OpenCell_SetMinesOnFieldAndOpenACellWithMineNeighbors_NumberOfNeighborsMine()
        {
            //action
            gameProcessor.Open(0, 4);
            var cell = gameProcessor.GetCurrentField();
            var actual = cell[0, 4];
            //assert
            var expected = PointState.Neighbors1;
            Assert.AreEqual(expected, actual);

        }
        [Test, Order(6)]
        public void OpenCell_SetMinesOnFieldAndOpenACellWithoutMineClose_NeighborsCellIsOpen()
        {
            //action

            gameProcessor.Open(0, 0);
            var cell = gameProcessor.GetCurrentField();
            var actual = cell[0, 3];
            //assert
            var expected = PointState.Neighbors0;
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(7)]
        public void OpenCell_SetMinesOnFieldAndOpenACellFarAwayFromAClosedCell_CellIsStillClose()
        {
            //action

            gameProcessor.Open(0, 0);
            var cell = gameProcessor.GetCurrentField();
            var actual = cell[8, 8];
            //assert
            var expected = PointState.Close;
            Assert.AreEqual(expected, actual);
        }
    }

    [TestFixture]
    internal class PositiveTestCasesGetCurrentFile
    {
        bool[,] testField = new bool[3, 3];
        PointState[,] pointStateField;
        GameProcessor gameProcessor;

        [SetUp]
        public void CreateField()
        {
            testField[0, 0] = true;
            testField[1, 0] = true;
            gameProcessor = new GameProcessor(testField);
        }


        [Test, Order(1)]
        public void GetCurrentField_SetPointStateNoneCellIsOpen_CellKeepsSameSate()
        {
            //setup

            //action
            var actual = gameProcessor.GetCurrentField();

            //Assert
            pointStateField = new PointState[3, 3] {
            { PointState.Close, PointState.Close,  PointState.Close },
            { PointState.Close, PointState.Close,  PointState.Close },
            { PointState.Close, PointState.Close, PointState.Close} };
            var expected = pointStateField;
            Assert.AreEqual(expected, actual);


        }

        [Test, Order(2)]
        public void GetCurrentField_SetPointStateAndMineCellIsOpen_AllCellStateIsUpdated()
        {
          
            //action

            gameProcessor.Open(0, 0);
            var actual = gameProcessor.GetCurrentField();

            //Assert

            pointStateField = new PointState[3, 3] {
            { PointState.Mine, PointState.Neighbors0,  PointState.Neighbors0 },
            { PointState.Mine, PointState.Neighbors0,  PointState.Neighbors0 },
            { PointState.Neighbors0, PointState.Neighbors0, PointState.Neighbors0} };

            var expected = pointStateField;
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(3)]
        public void GetCurrentField_SetPointStateNeighborCellMineIsOpen_JustCellStateIsUpdated()
        {

            //action

            gameProcessor.Open(0, 2);
            var actual = gameProcessor.GetCurrentField();

            //Assert

            pointStateField = new PointState[3, 3] {
            { PointState.Close, PointState.Close,  PointState.Close },
            { PointState.Close, PointState.Close,  PointState.Close },
            { PointState.Neighbors1, PointState.Close, PointState.Close} };


            var expected = pointStateField;
            Assert.AreEqual(expected, actual);

        }

        
    }
}
