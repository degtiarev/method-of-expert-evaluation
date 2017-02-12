using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experts
{
    class EvalutionTable
    {
        // матрица, где по столбцам - эксперты, по строкам - объекты
        double[,] matrix;
        List<string> ObjectNames;

        public EvalutionTable(double[,] matrix, List<string> ObjectNames)
        {
            this.matrix = matrix;
            this.ObjectNames = ObjectNames;
        }

        // средние значения оценок экспертов
        public List<double> GetAverageValuesList()
        {
            List<double> Average = new List<double>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double summ = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                    summ += matrix[i, j];
                Average.Add(summ / matrix.GetLength(1));
            }

            return Average;
        }

        //  сумма оценок по объектам
        public List<double> GetSummOfValuesList()
        {
            List<double> SummOfValues = new List<double>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double summ = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                    summ += matrix[i, j];
                SummOfValues.Add(summ);
            }


            return SummOfValues;
        }

        // коэффициенты компетентности экспертов
        public List<double> GetExpertsCoefficientOfCompetenceList()
        {
            List<double> Coefficients = new List<double>();
            List<double> AverageValuesList = this.GetAverageValuesList();
            List<double> SummOfValuesList = this.GetSummOfValuesList();

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                double SummOfXToM = 0;
                double SummOfXToS = 0;
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    SummOfXToM += matrix[j, i] * AverageValuesList[j];
                    SummOfXToS += matrix[j, i] * SummOfValuesList[j];
                }
                Coefficients.Add(SummOfXToM / SummOfXToS);
            }


            return Coefficients;
        }

        // матрица взвешенных экспертных оценок по каждому объекту
        public double[,] GetBalancedEvaluationMatrix()
        {
            double[,] balancedEvaluationMatrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
            List<double> ExpertsCoefficientOfCompetenceList = this.GetExpertsCoefficientOfCompetenceList();

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    balancedEvaluationMatrix[i, j] =  matrix[i, j] * ExpertsCoefficientOfCompetenceList[j];

            return balancedEvaluationMatrix;
        }

        // сумма взвешенных экспертных оценок по каждому объекту
        public List<double> GetBalancedEvalutionList()
        {
            List<double> BalancedEvalutionList = new List<double>();
            double[,] balancedEvaluationMatrix = this.GetBalancedEvaluationMatrix();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double summ = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                    summ += balancedEvaluationMatrix[i, j];
                BalancedEvalutionList.Add(Math.Round(summ,2));
            }

            return BalancedEvalutionList;
        }

        // квадрат отклонение от среднего по каждому объекту
        public List<double> GetSquareOfDeviationFromTheAverageList()
        {
            List<double> SquareOfDeviationFromTheAverageList = new List<double>();
            List<double> SummOfValuesList = this.GetSummOfValuesList();

            // среднее значение суммарных оценок по всем объектам
            double AverageValueOfTotalScores = 0;
            foreach (double i in SummOfValuesList)
                AverageValueOfTotalScores += i;
            AverageValueOfTotalScores = AverageValueOfTotalScores / SummOfValuesList.Count;

            foreach (double i in SummOfValuesList)
                SquareOfDeviationFromTheAverageList.Add(Math.Pow(i - AverageValueOfTotalScores, 2));

            return SquareOfDeviationFromTheAverageList;
        }

        // коэффициент согласованности мнений экспертов (коэффициент конкордации)
        public double GetConcordanceCoefficient()
        {
            int m = matrix.GetLength(0);
            int n = matrix.GetLength(1);
            double S = 0;
            List<double> SquareOfDeviationFromTheAverageList = this.GetSquareOfDeviationFromTheAverageList();
            foreach (double i in SquareOfDeviationFromTheAverageList)
                S += i;

            return (12 * S) / (Math.Pow(n, 2) * (Math.Pow(m, 3) - m));
        }

        // отсортированный список по взвешенной экспертной оценке, содержащий имя объекта и его взвешенную экспертную оценку
        public List<Tuple<string, double>> GetResult()
        {
            List<double> BalancedEvalutionList = this.GetBalancedEvalutionList();
            List<Tuple<string, double>> Result = new List<Tuple<string, double>>();

            for (int i = 0; i < matrix.GetLength(0); i++)
                Result.Add(new Tuple<string, double>(ObjectNames[i], BalancedEvalutionList[i]));

            Result = Result.OrderByDescending(i => i.Item2).ToList();

           // Result.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            return Result;
        }


    }
}
