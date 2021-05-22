﻿namespace TheBureau.Models.DataManipulating
{
    public static class ValidationConst
    {
        public const string LettersHyphenRegex = @"^[а-яА-Я-]+$";
        public const string EmailRegex = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}"; //todo Г на П
        public const string ContactNumberRegex = @"^375((17|25|29|33|44))[0-9]{7}$";

        public const string FieldCannotBeEmpty = "Поле не может быть пустым";
        public const string EmailLengthExceeded = "Превышена максимальная длина адреса почты (255)";
        public const string NameLengthExceeded = "Длина поля должна находиться в пределах от 2 до 20 символов";
        public const string CommentLengthExceeded = "Максимальное количество символов в комментарии к заявке - 200";

        
        public const string IncorrectEmailStructure = "Некорректная структура адреса электронной почты";
        public const string IncorrectNumberStructure = "Номер телефона должен начинаться с 375, иметь один из зональных кодов: 17, 25, 29, 33, 44 и содержать 12 цифр";
        public const string IncorrectFirstname = "Имя может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectSurname = "Фамилия может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectPatronymic = "Отчество может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectCity = "Название города может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectStreet = "Название улицы может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectHouse = "Номер дома не может быть больше 300";
        public const string IncorrectCorpus = "Номер корпуса не может быть содержать более 2 символов";
        public const string IncorrectFlat = "Номер квартиры не может быть больше 1011";
        public const string IncorrectPassword = "Длина пароля должна быть от 8 до 40 символов";
        
        
    }
}