using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Banks.Services
{
    public class ConsoleUi
    {
        private UserService _userService;

        public ConsoleUi(UserService userService)
        {
            _userService = userService;
        }

        public bool IsFinished { get; private set; }

        public void WaitForInput()
        {
            string command = Console.ReadLine();
            if (string.IsNullOrEmpty(command))
                return;
            var splitCommand = command.Split(' ').ToList();
            string method = "User" + char.ToUpper(splitCommand[0][0]) + splitCommand[0].ToLower()[1..];
            var args = new List<object>();
            args.AddRange(splitCommand.Skip(1));
            MethodInfo methodInfo = GetType().GetMethod(method);
            if (methodInfo == null)
            {
                Console.WriteLine("No such command");
                return;
            }

            methodInfo.Invoke(this, args.ToArray());
        }

        public void UserLogin(string bankName)
        {
            Console.WriteLine(_userService.LogInBank(bankName));
        }

        public void UserTransfer(string destination, string amount)
        {
            if (!double.TryParse(amount, out double actualAmount))
            {
                Console.WriteLine("Not a number!");
                return;
            }

            Console.WriteLine("From which account?\n" + _userService.ShowAccounts());
            int fromNumber = GetChoice();
            Console.WriteLine("To which account?\n" + _userService.ShowAccounts(destination));
            int toNumber = GetChoice();
            Console.WriteLine(_userService.Transfer(fromNumber, destination, toNumber, actualAmount));
        }

        public void UserQuit()
        {
            IsFinished = true;
        }

        private int GetChoice()
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Not a number!");
            }

            return choice;
        }
    }
}