using System;
using Xunit;

namespace Soluter.Tests
{
    public class FormulaTesting
    {
        [Fact]
        public void TestIntNumbers()
        {
            //Arrange
            int[] variables = new int[5] { 1, 2, 3, 4, 5 };
            int expected_1 = 9;
            int expected_2 = 2277;


            //Act
            var equation1 = new EquationDataTemplate("ax+by" + ((char)8304).ToString() + "+c", (float[] mas) =>
            {
                return mas[0] * mas[3] + mas[1] + mas[2];
            }, 0);
            var equation2 = new EquationDataTemplate("ax⁵+by⁴+c", (float[] mas) =>
            {
                return (float)(mas[0] * MathF.Pow(mas[3], 5) + mas[1] * MathF.Pow(mas[4], 4) + mas[2]);
            }, 4);
            int result_1 = (int)equation1.ToSolute(variables[0], variables[1], variables[2], variables[3], variables[4]);
            int result_2 = (int)equation2.ToSolute(variables[0], variables[1], variables[2], variables[3], variables[4]);

            //Assert
            Assert.Equal(expected_1, result_1);
            Assert.Equal(expected_2, result_2);

        }
        [Fact]
        public void TestFloatNumbers()
        {
            //Arrange
            float[] variables = new float[5] { 0.1f, 2f, 12f, 0.04f, 0.215f };
            float expected_1 = 14.004f;
            float expected_2 = 12.004273f;


            //Act
            var equation1 = new EquationDataTemplate("ax+by" + ((char)8304).ToString() + "+c", (float[] mas) =>
            {
                return mas[0] * mas[3] + mas[1] + mas[2];
            }, 0);
            var equation2 = new EquationDataTemplate("ax⁵+by⁴+c", (float[] mas) =>
            {
                return (float)(mas[0] * MathF.Pow(mas[3], 5) + mas[1] * MathF.Pow(mas[4], 4) + mas[2]);
            }, 4);
            float result_1 = equation1.ToSolute(variables[0], variables[1], variables[2], variables[3], variables[4]);
            float result_2 = equation2.ToSolute(variables[0], variables[1], variables[2], variables[3], variables[4]);

            //Assert
            Assert.Equal(expected_1, result_1);
            Assert.Equal(expected_2, result_2);
        }
        

    }
}
