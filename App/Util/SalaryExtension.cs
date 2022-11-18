using System;

namespace amantiq.util
{
    public static class SalaryExtension
    {
        private static int[] salaryRanges = new int[]
        {
            3000, 5000, 7000,
            10000, 14000, 20000,
            30000, 40000, 50000,
            60000, 70000, 80000,
            90000, 100000, 110000,
            120000, 130000, 140000,
            150000, 160000, 170000,
            180000, 190000, 200000,
            210000, 220000, 230000,
            240000, 250000, 260000,
            270000, 280000, 290000,
            300000, 310000, 320000,
            330000, 340000, 350000,
            360000, 370000, 380000,
            390000, 400000
        };

        private static int[] eis = new int[]
        {
            5, 1, 15,
            20, 25, 35,
            50, 70, 90,
            110, 130, 150,
            170, 190, 210,
            230, 250, 270,
            290, 310, 330,
            350, 370, 390,
            410, 430, 450,
            470, 490, 510,
            530, 550, 570,
            590, 610, 630,
            650, 670, 690,
            710, 730, 750,
            770, 790
        };

        private static int[] employerSocso = new int[]
        {
            40, 70, 110,
            150, 210, 295,
            435, 615, 785,
            965, 1135, 1315,
            1485, 1665, 1835,
            2015, 2185, 2365,
            2535, 2715, 2885,
            3065, 3235, 3415,
            3585, 3765, 3935,
            4115, 4285, 4465,
            4635, 4815, 4985,
            5165, 5335, 5515,
            5685, 5865, 6035,
            6215, 6385, 6565,
            6735, 6905
        };

        private static int[] employeeSoso = new int[]
        {
            10, 20, 30,
            40, 60, 85,
            125, 175, 225,
            275, 325, 375,
            425, 475, 525,
            575, 625, 675,
            725, 775, 825,
            875, 925, 975,
            1025, 1075, 1125,
            1175, 1225, 1275,
            1325, 1375, 1425,
            1475, 1525, 1575,
            1625, 1675, 1725,
            1775, 1825, 1875,
            1925, 1975
        };

        public static int getZakat(this int gross_salary, int percentage)
        {
            return gross_salary * percentage / 100;
        }

        public static int getEPF(this int gross_salary, int percentage)
        {
            return gross_salary * percentage / 100;
        }

        public static int getEmployerSocso(this int gross_salary)
        {
            for (int i = 0; i < salaryRanges.Length; i++)
            {
                if (i == salaryRanges.Length - 1)
                {
                    return employerSocso[i];
                }

                if (i == 0 && gross_salary.CompareTo(salaryRanges[i]) == -1)
                {
                    return employerSocso[i];
                }

                if (i != 0)
                {
                    if (gross_salary.CompareTo(salaryRanges[i - 1]) >= 0 && gross_salary.CompareTo(salaryRanges[i]) == -1)
                    {
                        return employerSocso[i];
                    }
                }
            }
            return 0;
        }

        public static int getEmployeeSocso(this int gross_salary)
        {
            for (int i = 0; i < salaryRanges.Length; i++)
            {
                if (i == salaryRanges.Length - 1)
                {
                    return employeeSoso[i];
                }

                if (i == 0 && gross_salary.CompareTo(salaryRanges[i]) == -1)
                {
                    return employeeSoso[i];
                }

                if (i != 0)
                {
                    if (gross_salary.CompareTo(salaryRanges[i - 1]) >= 0 && gross_salary.CompareTo(salaryRanges[i]) == -1)
                    {
                        return employeeSoso[i];
                    }
                }
            }
            return 0;
        }

        public static int getEIS(this int gross_salary)
        {
            for (int i = 0; i < salaryRanges.Length; i++)
            {
                if (i == salaryRanges.Length - 1)
                {
                    return eis[i];
                }

                if (i == 0 && gross_salary.CompareTo(salaryRanges[i]) == -1)
                {
                    return eis[i];
                }

                if (i != 0)
                {
                    if (gross_salary.CompareTo(salaryRanges[i - 1]) >= 0 && gross_salary.CompareTo(salaryRanges[i]) == -1)
                    {
                        return eis[i];
                    }
                }
            }
            return 0;
        }
    }
}