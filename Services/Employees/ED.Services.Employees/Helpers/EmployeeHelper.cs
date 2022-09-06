using ED.Services.Employees.Contract.Models;

using System;

namespace ED.Services.Employees.Helpers
{
    internal static class EmployeeHelper
    {
        public static Employee GenerateRandomEmployee(int gender)
        {
            var maleFirstNames = new string[] { "Иван", "Андрей", "Алексей",
                "Инокентий", "Евгений", "Станислав", "Александр",
                "Захар", "Виктор", "Юрий", "Максим", "Илья", "Дмитрий" };
            var femaleFirstNames = new string[] { "Анастасия", "Алина", "Елена",
                "Евгения", "Александра", "Мария", "Марина", "Анна",
                "Виктория", "Вероника", "Инесса", "Ольга", "Татьяна" };
            var maleLastNames = new string[] { "Иванов", "Андреев", "Алексеев",
                "Сидоров", "Баранов", "Гончаров", "Белоусов", "Захаров",
                "Бодров", "Карпатов", "Белов", "Чернов", "Волочков" };
            var femaleLastNames = new string[] { "Иванова", "Андреева", "Алексеева",
                "Сидорова", "Баранова", "Гончарова", "Белоусова", "Захарова",
                "Вишневская", "Тишкова", "Белова", "Чернова", "Волочкова" };
            var malePatronymics = new string[] { "Иванович", "Андреевич", "Алексеевич",
                "Анатольевич", "Евгеньевич", "Станиславович", "Александрович", "Захарович",
                "Викторович", "Юрьевич", "Владимирович", "Ильич", "Олегович" };
            var femalePatronymics = new string[] { "Ивановна", "Андреевна", "Алексеевна",
                "Анатольевна", "Евгеньевна", "Станиславовна", "Александровна", "Захаровна",
                "Владимировна", "Юрьевна", "Викторовна", "Ильинишна", "Олеговна" };

            var rnd = new Random();
            string firstName, lastName, patronymic;

            if (gender == 0)
            {
                firstName = maleFirstNames[rnd.Next(0, maleFirstNames.Length)];
                lastName = maleLastNames[rnd.Next(0, maleLastNames.Length)];
                patronymic = malePatronymics[rnd.Next(0, malePatronymics.Length)];
            }
            else
            {
                firstName = femaleFirstNames[rnd.Next(0, femaleFirstNames.Length)];
                lastName = femaleLastNames[rnd.Next(0, femaleLastNames.Length)];
                patronymic = femalePatronymics[rnd.Next(0, femalePatronymics.Length)];
            }

            return new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                Patronymic = patronymic
            };
        }
    }
}
