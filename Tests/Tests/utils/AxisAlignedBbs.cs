﻿using System;
using FluentAssertions;
using GlLib.Utils.Math;
using NUnit.Framework;

namespace Tests.utils
{
    public class AxisAlignedBbTests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Now AxisAlignedBb is testing");
        }

        #region AxisAlignedBb

        [Test]
        public void AxisAlignedBb_Equal_Same_Boxes()
        {
            //Arrange
            var bb1 = new AxisAlignedBb(0, 0, 15, 15);
            var bb2 = new AxisAlignedBb(0, 0, 15, 15);
            //Act

            //Assert
            Assert.AreEqual(bb1, bb2);
        }

        [Test]
        public void AxisAlignedBb_Equal_With_Vectors()
        {
            //Arrange
            var v1 = new PlanarVector(5, 5);
            var v2 = new PlanarVector(15, 15);
            var expectedBb = new AxisAlignedBb(5, 5, 15, 15);
            //Act
            var bb = new AxisAlignedBb(ref v1, ref v2);
            //Assert
            Assert.AreEqual(bb, expectedBb);
        }


        [Test]
        public void AxisAlignedBb_Equal()
        {
            //Arrange
            var bb1 = new AxisAlignedBb(0, 0, 15, 15);
            var bb2 = new AxisAlignedBb(15, 15, 0, 0);
            //Act

            //Assert
            Assert.AreEqual(bb1, bb2);
        }

        private static void CheckHeights(AxisAlignedBb _bb1, AxisAlignedBb _bb2)
        {
            var height1 = _bb1.Height;
            var height2 = _bb2.Height;

            Assert.AreEqual(height1, height2);
        }

        private static void CheckWidths(AxisAlignedBb _bb1, AxisAlignedBb _bb2)
        {
            var height1 = _bb1.Width;
            var height2 = _bb2.Width;

            Assert.AreEqual(height1, height2);
        }

        [TestCase]
        public void AxisAlignedBb_Get_Heights()
        {
            var arrangePairs = new[]
            {
                (new AxisAlignedBb(0, 0, 15, 15),
                    new AxisAlignedBb(0, 0, -15, -15)),
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(0, 0, -0, 0)),
                (new AxisAlignedBb(0, 0, 42, 42),
                    new AxisAlignedBb(42, 42, 84, 84))
            };

            foreach (var pair in arrangePairs)
                CheckHeights(pair.Item1, pair.Item2);
        }

        [TestCase]
        public void AxisAlignedBb_Get_Widths()
        {
            var arrangePairs = new[]
            {
                (new AxisAlignedBb(0, 0, 15, 15),
                    new AxisAlignedBb(0, 0, -15, -15)),
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(0, 0, -0, 0)),
                (new AxisAlignedBb(0, 0, 42, 42),
                    new AxisAlignedBb(42, 42, 84, 84))
            };

            foreach (var pair in arrangePairs)
                CheckWidths(pair.Item1, pair.Item2);
        }

        private static void CheckIfIntersectsIsTrue(AxisAlignedBb _bb1, AxisAlignedBb _bb2)
        {
            Assert.IsTrue(_bb1.IntersectsWith(ref _bb2),
                $"{_bb1} should have intersection with {_bb2}");
        }

        [TestCase]
        public void AxisAlignedBb_IntersectsWith()
        {
            var arrangePairs = new[]
            {
                (new AxisAlignedBb(0, 0, 42, 42),
                    new AxisAlignedBb(13, 13, 84, 84)),
                (new AxisAlignedBb(0, 0, 42, 42),
                    new AxisAlignedBb(13, 13, 24, 24))
            };

            foreach (var pair in arrangePairs)
                CheckIfIntersectsIsTrue(pair.Item1, pair.Item2);
        }


        private static void CheckIfIntersectsIs(AxisAlignedBb _bb1, AxisAlignedBb _bb2, bool _intersects)
        {
            Assert.AreEqual(_bb1.IntersectsWith(ref _bb2), _intersects,
                $"{_bb1} shouldn't have intersection with {_bb2}");
        }

        [TestCase]
        public void AxisAlignedBb_Not_IntersectsWith()
        {
            var arrangePairs = new[]
            {
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(0, 0, 4, 4), true),
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(0, 0, 0, 0), true),
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(1, 1, 4, 4), false)
            };

            foreach (var triple in arrangePairs)
                CheckIfIntersectsIs(triple.Item1, triple.Item2, triple.Item3);
        }

        [TestCase]
        public void AxisAlignedBb_Vector_Sum()
        {
            var box = new AxisAlignedBb(0, 0, 123, 321);
            var vec = new PlanarVector(47, 51);
            var result = new AxisAlignedBb(47, 51, 170, 372);

            (box + vec).Should().BeEquivalentTo(result, "Sum with vector is translation");
        }

        [TestCase]
        public void AxisAlignedBb_Vector_Expand()
        {
            var box = new AxisAlignedBb(-2, -2, 2, 2);
            var result = new PlanarVector().ExpandBothTo(2, 2);

            box.Should().BeEquivalentTo(result, "Vector expand equally");
        }

        [TestCase]
        public void AxisAlignedBb_Vector_Expand_Sum()
        {
            var box = new AxisAlignedBb(319, 736, 323, 740);
            var vec = new PlanarVector(321, 738);
            var result = vec.ExpandBothTo(2, 2);

            box.Should().BeEquivalentTo(result, "They are at the same pos");
        }

        #endregion
    }
}